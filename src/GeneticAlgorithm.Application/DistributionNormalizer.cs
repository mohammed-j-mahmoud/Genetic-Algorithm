using System;
using System.Collections.Generic;

namespace GeneticAlgorithm.Application
{
    public static class DistributionNormalizer
    {
        public static List<KeyValuePair<int, double>> Normalize(IReadOnlyList<KeyValuePair<int, double>> entries)
        {
            if (entries == null || entries.Count == 0)
                return new List<KeyValuePair<int, double>>();

            double sum = 0;
            for (int i = 0; i < entries.Count; i++)
                sum += entries[i].Value;

            if (sum <= 0)
                throw new InvalidOperationException("Distribution probabilities must sum to a positive value.");

            if (Math.Abs(sum - 1.0) < 1e-9)
                return new List<KeyValuePair<int, double>>(entries);

            var normalized = new List<KeyValuePair<int, double>>(entries.Count);
            for (int i = 0; i < entries.Count; i++)
                normalized.Add(new KeyValuePair<int, double>(entries[i].Key, entries[i].Value / sum));

            return normalized;
        }
    }
}
