using System;

namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Samples operation durations from precomputed <see cref="DiscreteDistribution"/> instances.
    /// </summary>
    public sealed class TimeDistributionSampler
    {
        private readonly Random _random;

        /// <summary>
        /// Creates a sampler with an optional seeded random source.
        /// </summary>
        /// <param name="random">Random number generator; uses a new instance if null.</param>
        public TimeDistributionSampler(Random random = null)
        {
            _random = random ?? new Random();
        }

        /// <summary>
        /// Draws a duration in minutes from the given distribution.
        /// </summary>
        public int SampleDurationMinutes(DiscreteDistribution distribution) =>
            distribution.Sample(_random);
    }
}
