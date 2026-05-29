using System;
using System.Collections.Generic;

namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Precomputed discrete probability distribution for O(1)-ish sampling (small n: linear scan).
    /// Stores durations and cumulative probabilities in parallel arrays to avoid KVP overhead.
    /// </summary>
    public sealed class DiscreteDistribution
    {
        private readonly int[] _durationMinutes;
        private readonly double[] _cumulativeProbability;

        /// <summary>Default loading-time distribution.</summary>
        internal static readonly DiscreteDistribution DefaultLoading = new DiscreteDistribution(
            new[] { 5, 10, 15 },
            new[] { 0.3, 0.5, 0.2 });

        /// <summary>Default weighing-time distribution.</summary>
        internal static readonly DiscreteDistribution DefaultWeighing = new DiscreteDistribution(
            new[] { 12, 16 },
            new[] { 0.7, 0.3 });

        /// <summary>Default travel-time distribution.</summary>
        internal static readonly DiscreteDistribution DefaultTraveling = new DiscreteDistribution(
            new[] { 40, 60, 80, 100 },
            new[] { 0.4, 0.3, 0.2, 0.1 });

        private DiscreteDistribution(int[] durationMinutes, double[] weights)
        {
            _durationMinutes = durationMinutes;
            _cumulativeProbability = BuildCumulative(weights);
        }

        /// <summary>
        /// Builds a distribution from (minutes, weight) pairs. Returns a built-in default when empty.
        /// </summary>
        internal static DiscreteDistribution FromEntries(
            IReadOnlyList<KeyValuePair<int, double>> entries,
            DiscreteDistribution fallback)
        {
            if (entries == null || entries.Count == 0)
                return fallback;

            int count = entries.Count;
            var minutes = new int[count];
            var weights = new double[count];
            for (int i = 0; i < count; i++)
            {
                minutes[i] = entries[i].Key;
                weights[i] = entries[i].Value;
            }

            return new DiscreteDistribution(minutes, weights);
        }

        /// <summary>
        /// Samples a duration in minutes.
        /// </summary>
        public int Sample(Random random)
        {
            if (_durationMinutes.Length == 0)
                return 0;

            double roll = random.NextDouble();
            for (int i = 0; i < _cumulativeProbability.Length; i++)
            {
                if (roll < _cumulativeProbability[i])
                    return _durationMinutes[i];
            }

            return _durationMinutes[_durationMinutes.Length - 1];
        }

        private static double[] BuildCumulative(double[] weights)
        {
            var cumulative = new double[weights.Length];
            double sum = 0;
            for (int i = 0; i < weights.Length; i++)
                sum += weights[i];

            if (sum <= 0)
                throw new ArgumentException("Distribution weights must sum to a positive value.");

            double running = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                running += weights[i] / sum;
                cumulative[i] = running;
            }

            return cumulative;
        }
    }
}
