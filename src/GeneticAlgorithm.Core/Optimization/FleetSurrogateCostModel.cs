using System;
using GeneticAlgorithm.Core.Genetics;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Optimization
{
    /// <summary>
    /// Fast analytical cost estimate for ranking fleet sizes when the search space is too large
    /// for expected-time simulation on every combo (Phase 3 fallback).
    /// </summary>
    internal static class FleetSurrogateCostModel
    {
        private const double MinutesPerWorkDay = SimulationCostCalculator.MinutesPerWorkDay;

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
            double totalMinutes = EstimateTotalTimeMinutes(
                coalVolume,
                truckLoadVolume,
                loading,
                weighing,
                traveling,
                trucks,
                loaders,
                scalers);

            return EstimateTotalCostFromMinutes(
                totalMinutes,
                trucks,
                loaders,
                scalers,
                truckCostPerDay,
                loaderCostPerDay,
                scalerCostPerDay,
                projectDurationDays,
                delayCostPerDay);
        }

        internal static double EstimateFitness(
            float coalVolume,
            double fitnessScale,
            double totalCost) =>
            fitnessScale * coalVolume / (totalCost + 1);

        internal static double EstimateTotalTimeMinutes(
            float coalVolume,
            float truckLoadVolume,
            DiscreteDistribution loading,
            DiscreteDistribution weighing,
            DiscreteDistribution traveling,
            int trucks,
            int loaders,
            int scalers)
        {
            if (coalVolume <= 0 || truckLoadVolume <= 0)
                return 0;

            double trips = Math.Ceiling(coalVolume / truckLoadVolume);
            double loadingMinutes = Math.Max(1, Math.Round(loading.ExpectedMinutes()));
            double weighingMinutes = Math.Max(1, Math.Round(weighing.ExpectedMinutes()));
            double travelingMinutes = Math.Max(1, Math.Round(traveling.ExpectedMinutes()));

            int truckCount = Math.Max(1, trucks);
            int loaderCount = Math.Max(1, loaders);
            int scalerCount = Math.Max(1, scalers);

            double loadThroughput = loaderCount / loadingMinutes;
            double weighThroughput = scalerCount / weighingMinutes;
            double travelThroughput = truckCount / travelingMinutes;
            double systemThroughput = Math.Min(loadThroughput, Math.Min(weighThroughput, travelThroughput));
            if (systemThroughput <= 0)
                systemThroughput = 1e-9;

            double firstTripSpan = loadingMinutes + weighingMinutes + travelingMinutes;
            if (trips <= 1)
                return firstTripSpan;

            return (trips - 1) / systemThroughput + firstTripSpan;
        }

        internal static double EstimateTotalCostFromMinutes(
            double totalTimeMinutes,
            int trucks,
            int loaders,
            int scalers,
            float truckCostPerDay,
            float loaderCostPerDay,
            float scalerCostPerDay,
            float projectDurationDays,
            float delayCostPerDay)
        {
            int truckCount = Math.Max(1, trucks);
            int loaderCount = Math.Max(1, loaders);
            int scalerCount = Math.Max(1, scalers);

            double totalDays = Math.Ceiling(totalTimeMinutes / MinutesPerWorkDay);

            double costTruck = totalDays * truckCostPerDay * truckCount;
            double costLoader = totalDays * loaderCostPerDay * loaderCount;
            double costScaler = totalDays * scalerCostPerDay * scalerCount;

            double delayDays = totalDays - projectDurationDays;
            double delayCost = delayDays > 0 ? delayDays * delayCostPerDay : 0;

            return costTruck + costLoader + costScaler + delayCost;
        }
    }
}
