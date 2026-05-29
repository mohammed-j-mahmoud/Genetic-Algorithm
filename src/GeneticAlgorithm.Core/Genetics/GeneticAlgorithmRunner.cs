using System;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Runs the generational loop: selection, crossover, mutation, and best-individual tracking.
    /// </summary>
    internal sealed class GeneticAlgorithmRunner : IDisposable
    {
        private readonly GeneticAlgorithmConfig _config;
        private readonly SimulationFitnessCache _fitnessCache;
        private readonly Random _selectionRandom = new Random();
        private readonly Random _geneRandom = new Random();

        private DNA[] _population;
        private DNA[] _offspringBuffer;
        private readonly DNA _bestGeneRecord;
        private double[] _cumulativeFitness;
        private double _fitnessSum;

        /// <summary>Current generation individuals.</summary>
        public DNA[] Population => _population;

        /// <summary>Best individual found so far.</summary>
        public DNA BestGene { get; private set; }

        /// <summary>Generation index when <see cref="BestGene"/> was last improved.</summary>
        public int BestGeneGeneration { get; private set; }

        /// <summary>Current generation number (1-based).</summary>
        public int Generation { get; private set; }

        /// <summary>Best individual in the current generation.</summary>
        public DNA BestOfCurrentGeneration { get; private set; }

        /// <summary>Simulation result cache used for fitness evaluation.</summary>
        internal SimulationFitnessCache FitnessCache => _fitnessCache;

        /// <summary>
        /// Initializes the population and prewarms the fitness cache.
        /// </summary>
        public GeneticAlgorithmRunner(GeneticAlgorithmConfig config, Func<float, float, float, DumpTruckSimulation.SimulationOutput> simulate)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            if (simulate == null)
                throw new ArgumentNullException(nameof(simulate));

            Generation = 1;

            _fitnessCache = new SimulationFitnessCache(
                (trucks, loaders, scalers) =>
                    FitnessSnapshot.FromSimulation(simulate(trucks, loaders, scalers), config.NumCoal, config.FitnessScale),
                config.MaxTrucks,
                config.MaxLoaders,
                config.MaxScalers);

            _fitnessCache.Prewarm();

            int size = config.PopulationSize;
            _population = new DNA[size];
            _offspringBuffer = new DNA[size];
            _cumulativeFitness = new double[size];

            _bestGeneRecord = new DNA(_config, _fitnessCache, _geneRandom);
            BestGene = _bestGeneRecord;

            for (int i = 0; i < size; i++)
            {
                _population[i] = DNA.CreateRandom(_config, _fitnessCache, _geneRandom);
                _offspringBuffer[i] = new DNA(_config, _fitnessCache, _geneRandom);
            }

            RecalculatePopulationStatistics();
        }

        /// <summary>
        /// Produces the next generation in <see cref="_offspringBuffer"/> and swaps buffers (no population array reallocation).
        /// </summary>
        public void NewGeneration()
        {
            int size = _population.Length;

            for (int i = 0; i < size; i++)
            {
                int parentIndex1 = RouletteWheelSelector.SelectIndex(_cumulativeFitness, _fitnessSum, size, _selectionRandom);
                int parentIndex2 = RouletteWheelSelector.SelectIndex(_cumulativeFitness, _fitnessSum, size, _selectionRandom);

                DNA offspring = _offspringBuffer[i];
                offspring.ReproduceWith(_population[parentIndex1], _population[parentIndex2]);
            }

            SwapPopulationBuffers();
            RecalculatePopulationStatistics();
            Generation++;
        }

        private void RecalculatePopulationStatistics()
        {
            int size = _population.Length;
            double sum = 0;
            BestOfCurrentGeneration = _population[0];

            for (int i = 0; i < size; i++)
            {
                DNA individual = _population[i];
                sum += individual.Fitness;
                _cumulativeFitness[i] = sum;

                if (individual.Fitness > BestGene.Fitness)
                {
                    _bestGeneRecord.CopyStateFrom(individual);
                    BestGeneGeneration = Generation;
                }

                if (individual.Fitness > BestOfCurrentGeneration.Fitness)
                    BestOfCurrentGeneration = individual;
            }

            _fitnessSum = sum;
        }

        private void SwapPopulationBuffers()
        {
            DNA[] temp = _population;
            _population = _offspringBuffer;
            _offspringBuffer = temp;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _population = null;
            _offspringBuffer = null;
            _cumulativeFitness = null;
        }
    }
}
