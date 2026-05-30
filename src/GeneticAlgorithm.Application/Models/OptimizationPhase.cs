namespace GeneticAlgorithm.Application.Models
{
    /// <summary>
    /// Optimization strategy for comparison tabs.
    /// </summary>
    public enum OptimizationPhase
    {
        /// <summary>Exhaustive Search — brute force with expected-time simulation.</summary>
        Phase1 = 1,

        /// <summary>Genetic Search — GA on expected-time fitness, then stochastic verify.</summary>
        Phase2 = 2,

        /// <summary>Surrogate Search — analytical ranking, then stochastic verify.</summary>
        Phase3 = 3,

        /// <summary>Dynamic Programming Search — 3D DP table on expected-time simulation, then stochastic verify.</summary>
        Phase4 = 4
    }
}
