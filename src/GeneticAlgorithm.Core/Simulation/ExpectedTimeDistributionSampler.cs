namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Uses the weighted mean duration from each distribution (deterministic).
    /// </summary>
    public sealed class ExpectedTimeDistributionSampler : IOperationDurationSampler
    {
        /// <inheritdoc />
        public int SampleDurationMinutes(DiscreteDistribution distribution) =>
            (int)System.Math.Round(distribution.ExpectedMinutes());
    }
}
