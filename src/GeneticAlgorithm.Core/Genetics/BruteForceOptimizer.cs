using System;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Core.Optimization;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Evaluates every fleet combination in the search bounds (Phase 1).
    /// </summary>
    internal sealed class BruteForceOptimizer
    {
        private readonly GeneticAlgorithmConfig _config;
        private readonly SimulationFitnessCache _cache;

        /// <summary>Number of unique combinations evaluated.</summary>
        public int CombinationsEvaluated { get; private set; }

        /// <summary>Simulation runs executed on cache miss.</summary>
        public long SimulationCalls => _cache.SimulationCalls;

        /// <summary>Cache hits while evaluating combinations.</summary>
        public long CacheHits => _cache.CacheHits;

        public BruteForceOptimizer(
            GeneticAlgorithmConfig config,
            Func<int, int, int, DumpTruckSimulation.SimulationOutput> simulate,
            bool prewarmCache = true)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            if (simulate == null)
                throw new ArgumentNullException(nameof(simulate));

            _cache = new SimulationFitnessCache(
                (trucks, loaders, scalers) =>
                    FitnessSnapshot.FromSimulation(
                        simulate(trucks, loaders, scalers),
                        config.NumCoal,
                        config.FitnessScale),
                config.MaxTrucks,
                config.MaxLoaders,
                config.MaxScalers);

            if (prewarmCache)
                _cache.Prewarm();
        }

        /// <summary>
        /// Finds the best fleet triple by exhaustive search.
        /// </summary>
        public DNA Run(IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            DNA best = new DNA();
            double bestFitness = double.MinValue;
            int evaluated = 0;
            int lastReportedPercent = 0;
            int total = FleetCombinationIndex.TotalCombinations(
                _config.MaxTrucks,
                _config.MaxLoaders,
                _config.MaxScalers);
            object bestLock = new object();
            var parallelOptions = new ParallelOptions { CancellationToken = cancellationToken };

            try
            {
                Parallel.For(0, total, parallelOptions, index =>
                {
                    FleetCombinationIndex.Decode(
                        index,
                        _config.MaxTrucks,
                        _config.MaxLoaders,
                        _config.MaxScalers,
                        out int trucks,
                        out int loaders,
                        out int scalers);

                    FitnessSnapshot snapshot = _cache.GetOrEvaluate(trucks, loaders, scalers);
                    lock (bestLock)
                    {
                        if (snapshot.Fitness > bestFitness)
                        {
                            bestFitness = snapshot.Fitness;
                            best.CopyFrom(trucks, loaders, scalers, snapshot, _config.NumCoal);
                        }
                    }

                    ParallelProgress.ReportCompletion(progress, ref evaluated, ref lastReportedPercent, total);
                });
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
            }

            CombinationsEvaluated = evaluated;
            return best;
        }
    }
}
