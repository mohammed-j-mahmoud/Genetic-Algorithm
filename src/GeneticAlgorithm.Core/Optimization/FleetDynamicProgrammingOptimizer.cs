using System;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Core.Genetics;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Optimization
{
    /// <summary>
    /// Bottom-up 3D dynamic programming over fleet bounds using expected-time simulation (cached).
    /// Each cell (t, l, s) stores the best fitness achievable with at most t trucks, l loaders, and s scalers.
    /// </summary>
    internal sealed class FleetDynamicProgrammingOptimizer
    {
        private const int MaxDpTableCells = 500_000;

        private readonly GeneticAlgorithmConfig _config;
        private readonly SimulationFitnessCache _cache;

        /// <summary>Number of unique combinations evaluated at their exact (t, l, s) cell.</summary>
        public int CombinationsEvaluated { get; private set; }

        /// <summary>Simulation runs executed on cache miss.</summary>
        public long SimulationCalls => _cache.SimulationCalls;

        /// <summary>Cache hits while filling the DP table.</summary>
        public long CacheHits => _cache.CacheHits;

        public FleetDynamicProgrammingOptimizer(
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
        /// Fills the DP table and returns the best fleet within the configured bounds.
        /// </summary>
        public DNA Run(IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            int maxTrucks = _config.MaxTrucks;
            int maxLoaders = _config.MaxLoaders;
            int maxScalers = _config.MaxScalers;
            int total = maxTrucks * maxLoaders * maxScalers;

            long tableCells = (long)(maxTrucks + 1) * (maxLoaders + 1) * (maxScalers + 1);
            if (tableCells > MaxDpTableCells)
            {
                throw new InvalidOperationException(
                    $"DP table ({tableCells:N0} cells) exceeds limit ({MaxDpTableCells:N0}). " +
                    "Reduce max bounds or use Exhaustive or Surrogate search.");
            }

            var dp = new double[maxTrucks + 1, maxLoaders + 1, maxScalers + 1];
            var bestTrucks = new int[maxTrucks + 1, maxLoaders + 1, maxScalers + 1];
            var bestLoaders = new int[maxTrucks + 1, maxLoaders + 1, maxScalers + 1];
            var bestScalers = new int[maxTrucks + 1, maxLoaders + 1, maxScalers + 1];
            var bestSnapshot = new FitnessSnapshot[maxTrucks + 1, maxLoaders + 1, maxScalers + 1];

            for (int trucks = 0; trucks <= maxTrucks; trucks++)
            {
                for (int loaders = 0; loaders <= maxLoaders; loaders++)
                {
                    for (int scalers = 0; scalers <= maxScalers; scalers++)
                        dp[trucks, loaders, scalers] = double.MinValue;
                }
            }

            int evaluated = 0;
            int lastReportedPercent = 0;
            DNA globalBest = new DNA();
            double globalBestFitness = double.MinValue;
            object bestLock = new object();
            var parallelOptions = new ParallelOptions { CancellationToken = cancellationToken };
            int minSum = 3;
            int maxSum = maxTrucks + maxLoaders + maxScalers;

            try
            {
                for (int sum = minSum; sum <= maxSum; sum++)
                {
                    Parallel.For(1, maxTrucks + 1, parallelOptions, trucks =>
                    {
                        for (int loaders = 1; loaders <= maxLoaders; loaders++)
                        {
                            int scalers = sum - trucks - loaders;
                            if (scalers < 1 || scalers > maxScalers)
                                continue;

                            FitnessSnapshot snapshot = _cache.GetOrEvaluate(trucks, loaders, scalers);
                            double fitness = snapshot.Fitness;

                            dp[trucks, loaders, scalers] = fitness;
                            bestTrucks[trucks, loaders, scalers] = trucks;
                            bestLoaders[trucks, loaders, scalers] = loaders;
                            bestScalers[trucks, loaders, scalers] = scalers;
                            bestSnapshot[trucks, loaders, scalers] = snapshot;

                            ConsiderPredecessor(
                                dp, bestTrucks, bestLoaders, bestScalers, bestSnapshot,
                                trucks, loaders, scalers, trucks - 1, loaders, scalers);
                            ConsiderPredecessor(
                                dp, bestTrucks, bestLoaders, bestScalers, bestSnapshot,
                                trucks, loaders, scalers, trucks, loaders - 1, scalers);
                            ConsiderPredecessor(
                                dp, bestTrucks, bestLoaders, bestScalers, bestSnapshot,
                                trucks, loaders, scalers, trucks, loaders, scalers - 1);

                            double cellFitness = dp[trucks, loaders, scalers];
                            lock (bestLock)
                            {
                                if (cellFitness > globalBestFitness)
                                {
                                    globalBestFitness = cellFitness;
                                    int bt = bestTrucks[trucks, loaders, scalers];
                                    int bl = bestLoaders[trucks, loaders, scalers];
                                    int bs = bestScalers[trucks, loaders, scalers];
                                    globalBest.CopyFrom(bt, bl, bs, bestSnapshot[trucks, loaders, scalers], _config.NumCoal);
                                }
                            }

                            ParallelProgress.ReportCompletion(progress, ref evaluated, ref lastReportedPercent, total);
                        }
                    });
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
            }

            CombinationsEvaluated = evaluated;
            return globalBest;
        }

        private static void ConsiderPredecessor(
            double[,,] dp,
            int[,,] bestTrucks,
            int[,,] bestLoaders,
            int[,,] bestScalers,
            FitnessSnapshot[,,] bestSnapshot,
            int trucks,
            int loaders,
            int scalers,
            int predTrucks,
            int predLoaders,
            int predScalers)
        {
            if (predTrucks < 1 || predLoaders < 1 || predScalers < 1)
                return;

            double predFitness = dp[predTrucks, predLoaders, predScalers];
            if (predFitness <= dp[trucks, loaders, scalers])
                return;

            dp[trucks, loaders, scalers] = predFitness;
            bestTrucks[trucks, loaders, scalers] = bestTrucks[predTrucks, predLoaders, predScalers];
            bestLoaders[trucks, loaders, scalers] = bestLoaders[predTrucks, predLoaders, predScalers];
            bestScalers[trucks, loaders, scalers] = bestScalers[predTrucks, predLoaders, predScalers];
            bestSnapshot[trucks, loaders, scalers] = bestSnapshot[predTrucks, predLoaders, predScalers];
        }
    }
}
