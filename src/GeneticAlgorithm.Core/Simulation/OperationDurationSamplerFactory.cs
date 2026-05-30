using System;

namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Creates duration samplers for stochastic or expected-time simulation.
    /// </summary>
    public static class OperationDurationSamplerFactory
    {
        /// <summary>
        /// Creates a sampler for the requested evaluation mode.
        /// </summary>
        /// <param name="mode">Stochastic uses <paramref name="simulationSeed"/>; expected-time ignores it.</param>
        public static IOperationDurationSampler Create(SimulationEvaluationMode mode, int simulationSeed)
        {
            if (mode == SimulationEvaluationMode.ExpectedTimes)
                return new ExpectedTimeDistributionSampler();

            return new TimeDistributionSampler(new Random(simulationSeed));
        }
    }
}
