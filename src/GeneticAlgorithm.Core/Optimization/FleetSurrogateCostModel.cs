using System;
using GeneticAlgorithm.Core.Genetics;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Optimization
{
    /// <summary>
    /// Fast analytical cost estimate for ranking fleet sizes (Phase 3 search).
    /// </summary>
    internal static class FleetSurrogateCostModel
    {
        internal static double EstimateTotalCost(
            float coalVolume,
            float truckLoadVolume,
            float truckCostPerDay,
            float loaderCostPerDay,
            float scalerCostPerDay,
            float projectDurationDays,
            float delayCostPerDay,
            DiscreteDistribution loading,
            DiscreteDistribution weighing,
            DiscreteDistribution traveling,
            int trucks,
            int loaders,
            int scalers)
        {
            double tripMinutes = loading.ExpectedMinutes()
                + weighing.ExpectedMinutes()
                + traveling.ExpectedMinutes();
            if (tripMinutes <= 0)
                tripMinutes = 1;

            double totalTrips = Math.Ceiling(coalVolume / truckLoadVolume);
            double cycles = Math.Ceiling(totalTrips / Math.Max(1, trucks));
            double totalMinutes = cycles * tripMinutes;
            double totalDays = Math.Max(totalMinutes / (60.0 * 24.0), projectDurationDays);
            double delayDays = Math.Max(0, totalDays - projectDurationDays);

            return trucks * truckCostPerDay * totalDays
                + loaders * loaderCostPerDay * totalDays
                + scalers * scalerCostPerDay * totalDays
                + delayDays * delayCostPerDay;
        }

        internal static double EstimateFitness(
            float coalVolume,
            double fitnessScale,
            double totalCost) =>
            fitnessScale * coalVolume / (totalCost + 1);
    }
}
