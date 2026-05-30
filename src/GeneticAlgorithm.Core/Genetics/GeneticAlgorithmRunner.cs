using System;
using System.Threading;
using GeneticSharp;
using GeneticAlgorithm.Core.Simulation;
using SharpGeneticAlgorithm = GeneticSharp.GeneticAlgorithm;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Runs GeneticSharp with simulation-backed fitness and maps results to <see cref="DNA"/>.
    /// </summary>
    internal sealed class GeneticAlgorithmRunner : IDisposable
    {
        private readonly GeneticAlgorithmConfig _config;
        private readonly SimulationFitnessCache _fitnessCache;
        private readonly SharpGeneticAlgorithm _geneticAlgorithm;
        private readonly DNA _bestGeneRecord;
        private DNA[] _populationSnapshot = Array.Empty<DNA>();
        private DNA _bestOfCurrentGeneration = new DNA();
        private bool _disposed;
        private bool _hasRun;

        public DNA[] Population => _populationSnapshot;

        public DNA BestGene { get; private set; }

        public int BestGeneGeneration { get; private set; }

        public int Generation { get; private set; }

        public DNA BestOfCurrentGeneration => _bestOfCurrentGeneration;

        internal SimulationFitnessCache FitnessCache => _fitnessCache;

        public GeneticAlgorithmRunner(
            GeneticAlgorithmConfig config,
            Func<float, float, float, DumpTruckSimulation.SimulationOutput> simulate,
            bool prewarmCache = true)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            if (simulate == null)
                throw new ArgumentNullException(nameof(simulate));

            _fitnessCache = new SimulationFitnessCache(
                (trucks, loaders, scalers) =>
                    FitnessSnapshot.FromSimulation(
                        simulate(trucks, loaders, scalers),
                        config.NumCoal,
                        config.FitnessScale),
                config.MaxTrucks,
                config.MaxLoaders,
                config.MaxScalers);

            if (prewarmCache)
                _fitnessCache.Prewarm();

            var adamChromosome = new FleetChromosome(config.MaxTrucks, config.MaxLoaders, config.MaxScalers);
            var fitness = new FleetSimulationFitness(_fitnessCache);
            var population = new TplPopulation(config.PopulationSize, config.PopulationSize, adamChromosome);

            _geneticAlgorithm = new SharpGeneticAlgorithm(
                population,
                fitness,
                new EliteSelection(),
                new UniformCrossover(),
                new FleetGeneMutation(config.MutationRate))
            {
                MutationProbability = 1.0f // GeneticSharp gate is always open; per-gene rate is FleetGeneMutation._mutationRate.
            };

            _bestGeneRecord = new DNA();
            BestGene = _bestGeneRecord;
            BestGeneGeneration = 1;
            Generation = 0;
        }

        public void RunGenerations(
            int generations,
            Action<int, double> onGenerationCompleted = null,
            CancellationToken cancellationToken = default)
        {
            if (_hasRun)
                throw new InvalidOperationException("This genetic algorithm runner can only be started once.");
            if (generations <= 0 || generations > 10_000)
                throw new ArgumentOutOfRangeException(nameof(generations), "Generations must be between 1 and 10,000.");

            _hasRun = true;

            _geneticAlgorithm.GenerationRan += (_, __) =>
            {
                Generation = _geneticAlgorithm.Population.GenerationsNumber;
                RefreshPopulationSnapshot();
                RefreshBestOfCurrentGeneration();
                TrackBestIndividual();

                onGenerationCompleted?.Invoke(Generation, _bestOfCurrentGeneration.Fitness);

                if (cancellationToken.IsCancellationRequested)
                    _geneticAlgorithm.Stop();
            };

            _geneticAlgorithm.Termination = new GenerationNumberTermination(generations);
            _geneticAlgorithm.Start();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _populationSnapshot = Array.Empty<DNA>();
            _disposed = true;
        }

        private void TrackBestIndividual()
        {
            FleetChromosome best = (FleetChromosome)_geneticAlgorithm.Population.BestChromosome;
            if (!best.Fitness.HasValue)
                return;

            if (best.Fitness.Value > _bestGeneRecord.Fitness)
            {
                _bestGeneRecord.CopyFrom(best, _config.NumCoal);
                BestGeneGeneration = Generation;
            }
        }

        private void RefreshBestOfCurrentGeneration()
        {
            FleetChromosome currentBest = (FleetChromosome)_geneticAlgorithm.Population.CurrentGeneration.BestChromosome;
            if (!currentBest.Fitness.HasValue)
                return;

            _bestOfCurrentGeneration.CopyFrom(currentBest, _config.NumCoal);
        }

        private void RefreshPopulationSnapshot()
        {
            var chromosomes = _geneticAlgorithm.Population.CurrentGeneration.Chromosomes;
            var snapshot = new DNA[chromosomes.Count];

            for (int i = 0; i < chromosomes.Count; i++)
            {
                var fleet = (FleetChromosome)chromosomes[i];
                var dna = new DNA();
                if (fleet.Fitness.HasValue)
                    dna.CopyFrom(fleet, _config.NumCoal);
                snapshot[i] = dna;
            }

            _populationSnapshot = snapshot;
        }
    }
}
