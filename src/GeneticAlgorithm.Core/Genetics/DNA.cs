using System;
using System.Runtime.CompilerServices;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Chromosome with three integer genes (trucks, loaders, scalers) and cached simulation metrics.
    /// </summary>
    public sealed class DNA
    {
        private readonly int[] _genes = new int[3];

        /// <summary>Chromosome genes: [trucks, loaders, scalers] (read-only view).</summary>
        public System.Collections.Generic.IReadOnlyList<int> Genes => Array.AsReadOnly(_genes);

        /// <summary>Total project cost from last evaluation.</summary>
        public double TotalCost { get; private set; }

        /// <summary>Fitness score (higher is better).</summary>
        public double Fitness { get; private set; }

        /// <summary>Coal volume used in fitness calculation.</summary>
        public float numCoal { get; private set; }

        /// <summary>Total project days from last evaluation.</summary>
        public double TotalDays { get; private set; }

        /// <summary>Days of delay from last evaluation.</summary>
        public double DaysofDelay { get; private set; }

        /// <summary>Delay cost from last evaluation.</summary>
        public double CostofDelay { get; private set; }

        /// <summary>Truck utilization from last evaluation.</summary>
        public float utilTruck { get; private set; }

        /// <summary>Loader utilization from last evaluation.</summary>
        public float utilLoader { get; private set; }

        /// <summary>Scale utilization from last evaluation.</summary>
        public float utilScaler { get; private set; }

        private readonly GeneticAlgorithmConfig _config;
        private readonly SimulationFitnessCache _fitnessCache;
        private readonly Random _random;

        internal DNA(GeneticAlgorithmConfig config, SimulationFitnessCache fitnessCache, Random random)
        {
            _config = config;
            _fitnessCache = fitnessCache;
            _random = random;
            numCoal = config.NumCoal;
        }

        /// <summary>
        /// Creates a randomly initialized and evaluated individual.
        /// </summary>
        internal static DNA CreateRandom(GeneticAlgorithmConfig config, SimulationFitnessCache cache, Random random)
        {
            var dna = new DNA(config, cache, random);
            dna.RandomizeGenes();
            dna.Evaluate();
            return dna;
        }

        /// <summary>
        /// Re-rolls all genes uniformly within configured bounds.
        /// </summary>
        internal void RandomizeGenes()
        {
            _genes[0] = _random.Next(1, _config.MaxTrucks + 1);
            _genes[1] = _random.Next(1, _config.MaxLoaders + 1);
            _genes[2] = _random.Next(1, _config.MaxScalers + 1);
        }

        /// <summary>
        /// Loads metrics from the fitness cache for the current genes.
        /// </summary>
        public void Evaluate()
        {
            ApplySnapshot(_fitnessCache.GetOrEvaluate(_genes[0], _genes[1], _genes[2]));
        }

        /// <summary>
        /// Uniform crossover into an existing offspring slot (no allocation).
        /// </summary>
        public void CrossOverInto(DNA parent2, DNA offspring)
        {
            if (offspring == null)
                throw new ArgumentNullException(nameof(offspring));

            offspring.ReproduceWith(this, parent2);
        }

        /// <summary>
        /// Uniform crossover and optional mutation in-place (reuses an existing offspring slot).
        /// </summary>
        internal void ReproduceWith(DNA parent1, DNA parent2)
        {
            ReproduceUniform(parent1, parent2);
            ApplyMutations();
            // Crossover always changes genes; fitness must be refreshed every offspring.
            Evaluate();
        }

        /// <summary>
        /// Applies gene mutation with configured rate, then re-evaluates fitness.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Mutate()
        {
            ApplyMutations();
            Evaluate();
        }

        /// <summary>
        /// Mutates each gene independently with probability <see cref="GeneticAlgorithmConfig.MutationRate"/>.
        /// </summary>
        private void ApplyMutations()
        {
            if (_random.NextDouble() < _config.MutationRate)
                _genes[0] = _random.Next(1, _config.MaxTrucks + 1);

            if (_random.NextDouble() < _config.MutationRate)
                _genes[1] = _random.Next(1, _config.MaxLoaders + 1);

            if (_random.NextDouble() < _config.MutationRate)
                _genes[2] = _random.Next(1, _config.MaxScalers + 1);
        }

        private void ReproduceUniform(DNA parent1, DNA parent2)
        {
            _genes[0] = _random.NextDouble() < 0.5 ? parent1._genes[0] : parent2._genes[0];
            _genes[1] = _random.NextDouble() < 0.5 ? parent1._genes[1] : parent2._genes[1];
            _genes[2] = _random.NextDouble() < 0.5 ? parent1._genes[2] : parent2._genes[2];
        }

        private void ApplySnapshot(FitnessSnapshot snapshot)
        {
            TotalCost = snapshot.TotalCost;
            Fitness = snapshot.Fitness;
            TotalDays = snapshot.TotalDays;
            DaysofDelay = snapshot.DaysOfDelay;
            CostofDelay = snapshot.CostOfDelay;
            utilTruck = snapshot.UtilTruck;
            utilLoader = snapshot.UtilLoader;
            utilScaler = snapshot.UtilScaler;
        }

        /// <summary>Copies genes and evaluated metrics from another individual (for best-gene tracking).</summary>
        internal void CopyStateFrom(DNA source)
        {
            _genes[0] = source._genes[0];
            _genes[1] = source._genes[1];
            _genes[2] = source._genes[2];
            TotalCost = source.TotalCost;
            Fitness = source.Fitness;
            TotalDays = source.TotalDays;
            DaysofDelay = source.DaysofDelay;
            CostofDelay = source.CostofDelay;
            utilTruck = source.utilTruck;
            utilLoader = source.utilLoader;
            utilScaler = source.utilScaler;
        }

        /// <inheritdoc />
        public override string ToString() =>
            $"Number of Trucks: {_genes[0]}, Number of Loaders: {_genes[1]}, Number of Scalers: {_genes[2]}, Fitness: {Fitness}";
    }
}
