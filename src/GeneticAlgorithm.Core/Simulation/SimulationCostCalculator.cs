namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Computes monetary costs and utilization metrics from simulated time totals.
    /// </summary>
    internal static class SimulationCostCalculator
    {
        internal const float MinutesPerWorkDay = 480f;

        /// <summary>
        /// Builds the final <see cref="SimulationResult"/> from accumulated simulation metrics.
        /// </summary>
        /// <param name="parameters">Input costs and resource counts.</param>
        /// <param name="totalTimeMinutes">Total simulated time in minutes.</param>
        /// <param name="totalTimeTruck">Truck-active minutes accumulated.</param>
        /// <param name="totalTimeLoader">Loader-active minutes accumulated.</param>
        /// <param name="totalTimeScaler">Scale-active minutes accumulated.</param>
        public static SimulationResult BuildResult(
            SimulationParameters parameters,
            float totalTimeMinutes,
            float totalTimeTruck,
            float totalTimeLoader,
            float totalTimeScaler)
        {
            double totalDays = System.Math.Ceiling(totalTimeMinutes / MinutesPerWorkDay);

            double costTruck = totalDays * parameters.TruckCostPerDay * parameters.TruckCount;
            double costLoader = totalDays * parameters.LoaderCostPerDay * parameters.LoaderCount;
            double costScaler = totalDays * parameters.ScalerCostPerDay * parameters.ScalerCount;

            double delayDuration = totalDays - parameters.ProjectDurationDays;
            double costOfDelay = delayDuration * parameters.DelayCostPerDay;

            if (costOfDelay < 0)
            {
                delayDuration = 0;
                costOfDelay = 0;
            }

            return new SimulationResult
            {
                UtilTruck = SafeUtilization(totalTimeTruck, totalTimeMinutes, parameters.TruckCount),
                UtilLoader = SafeUtilization(totalTimeLoader, totalTimeMinutes, parameters.LoaderCount),
                UtilScaler = SafeUtilization(totalTimeScaler, totalTimeMinutes, parameters.ScalerCount),
                TotalCost = costTruck + costLoader + costScaler + costOfDelay,
                TotalCostTruck = costTruck,
                TotalCostLoader = costLoader,
                TotalCostScaler = costScaler,
                CostOfDelay = costOfDelay,
                DelayDuration = delayDuration,
                TotalDays = totalDays
            };
        }

        private static float SafeUtilization(float resourceMinutes, float totalMinutes, float resourceCount)
        {
            if (totalMinutes <= 0 || resourceCount <= 0)
                return 0f;

            return resourceMinutes / totalMinutes / resourceCount;
        }
    }
}
