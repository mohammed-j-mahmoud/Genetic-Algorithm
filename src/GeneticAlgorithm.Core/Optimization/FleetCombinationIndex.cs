using System;

namespace GeneticAlgorithm.Core.Optimization
{
    /// <summary>
    /// Maps flat indices to (trucks, loaders, scalers) in the same order as nested brute-force loops.
    /// </summary>
    internal static class FleetCombinationIndex
    {
        public static long TotalCombinationsLong(int maxTrucks, int maxLoaders, int maxScalers) =>
            (long)maxTrucks * maxLoaders * maxScalers;

        public static int TotalCombinations(int maxTrucks, int maxLoaders, int maxScalers)
        {
            long total = TotalCombinationsLong(maxTrucks, maxLoaders, maxScalers);
            if (total <= 0 || total > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxTrucks),
                    "Combination count exceeds supported parallel search limit.");
            }

            return (int)total;
        }

        public static void Decode(
            int index,
            int maxTrucks,
            int maxLoaders,
            int maxScalers,
            out int trucks,
            out int loaders,
            out int scalers)
        {
            int total = TotalCombinations(maxTrucks, maxLoaders, maxScalers);
            if (index < 0 || index >= total)
                throw new ArgumentOutOfRangeException(nameof(index));

            int slice = maxLoaders * maxScalers;
            trucks = index / slice + 1;
            int remainder = index % slice;
            loaders = remainder / maxScalers + 1;
            scalers = remainder % maxScalers + 1;
        }
    }
}
