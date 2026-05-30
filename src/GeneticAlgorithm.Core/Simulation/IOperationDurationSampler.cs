namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Draws operation durations for loading, weighing, and travel steps.
    /// </summary>
    public interface IOperationDurationSampler
    {
        /// <summary>Draws a duration in minutes from the given distribution.</summary>
        int SampleDurationMinutes(DiscreteDistribution distribution);
    }
}
