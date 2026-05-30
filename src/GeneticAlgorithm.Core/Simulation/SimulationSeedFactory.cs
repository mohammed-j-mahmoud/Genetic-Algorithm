using System;

namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Builds reproducible simulation seeds for a fleet triple within an optimization run.
    /// </summary>
    public static class SimulationSeedFactory
    {
        /// <summary>
        /// Returns a deterministic seed for stochastic simulation of the given fleet sizes.
        /// </summary>
        public static int ForFleet(int trucks, int loaders, int scalers, int runSeed) =>
            HashCode.Combine(trucks, loaders, scalers, runSeed);
    }
}
