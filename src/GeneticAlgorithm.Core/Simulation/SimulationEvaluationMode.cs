namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// How operation durations are chosen during a simulation run.
    /// </summary>
    public enum SimulationEvaluationMode
    {
        /// <summary>Random sample per operation; use a fixed seed per fleet triple for reproducible caching.</summary>
        Stochastic,

        /// <summary>Deterministic mean duration from each probability distribution.</summary>
        ExpectedTimes
    }
}
