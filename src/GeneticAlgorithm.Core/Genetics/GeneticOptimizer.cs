using System;
using System.Threading;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Entry facade for genetic-algorithm runs backed by GeneticSharp.
    /// </summary>
    public static class GeneticOptimizer
    {
        /// <summary>
        /// Configures and runs a generational genetic search over fleet sizes.
        /// </summary>
        public sealed class GeneticAlgorithm : IDisposable
        {
            private readonly GeneticAlgorithmRunner _runner;
            private bool _disposed;

            public DNA[] Population => _runner.Population;

            public DNA BestGene => _runner.BestGene;

            public int BestGeneGeneration => _runner.BestGeneGeneration;

            public int Generation => _runner.Generation;

            public DNA BestOfCurrentGeneration => _runner.BestOfCurrentGeneration;

            public long SimulationCalls => _runner.FitnessCache.SimulationCalls;

            public long CacheHits => _runner.FitnessCache.CacheHits;

            public GeneticAlgorithm(
                int populationSize,
                int maxTrucks,
                int maxLoaders,
                int maxScalers,
                Func<float, float, float, DumpTruckSimulation.SimulationOutput> fitnessFunction,
                float coalVolume,
                double mutationRate = 0.01,
                bool prewarmCache = true)
            {
                var config = GeneticAlgorithmConfig.Create(
                    populationSize,
                    maxTrucks,
                    maxLoaders,
                    maxScalers,
                    coalVolume,
                    mutationRate);

                _runner = new GeneticAlgorithmRunner(config, fitnessFunction, prewarmCache);
            }

            /// <summary>
            /// Runs the configured number of generations and reports progress after each one.
            /// </summary>
            public void RunGenerations(
                int generations,
                Action<int, double> onGenerationCompleted = null,
                CancellationToken cancellationToken = default) =>
                _runner.RunGenerations(generations, onGenerationCompleted, cancellationToken);

            public void Dispose()
            {
                if (_disposed)
                    return;

                _runner.Dispose();
                _disposed = true;
            }
        }
    }
}
