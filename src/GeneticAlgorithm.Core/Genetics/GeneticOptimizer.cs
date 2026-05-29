using System;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Entry facade for genetic-algorithm runs.
    /// </summary>
    public static class GeneticOptimizer
    {
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
                double mutationRate = 0.01)
            {
                var config = GeneticAlgorithmConfig.Create(
                    populationSize,
                    maxTrucks,
                    maxLoaders,
                    maxScalers,
                    coalVolume,
                    mutationRate);

                _runner = new GeneticAlgorithmRunner(config, fitnessFunction);
            }

            public void NewGeneration() => _runner.NewGeneration();

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
