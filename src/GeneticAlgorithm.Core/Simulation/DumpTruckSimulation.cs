using System.Collections.Generic;

namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Facade for running dump-truck discrete-event simulations.
    /// </summary>
    public static class DumpTruckSimulation
    {
        /// <summary>
        /// Simulation metrics returned to callers (GA fitness, UI, CLI, API).
        /// </summary>
        public sealed class SimulationOutput
        {
            public float TruckUtilization { get; set; }
            public float LoaderUtilization { get; set; }
            public float ScalerUtilization { get; set; }
            public double TotalDays { get; set; }
            public double TotalCost { get; set; }
            public double TruckCost { get; set; }
            public double LoaderCost { get; set; }
            public double ScalerCost { get; set; }
            public double DelayCost { get; set; }
            public double DelayDays { get; set; }
        }

        public static SimulationOutput Run(
            float coalVolume,
            float truckCount,
            float truckLoadVolume,
            float truckCostPerDay,
            float loaderCount,
            float loaderCostPerDay,
            float scalerCount,
            float scalerCostPerDay,
            float projectDurationDays,
            float delayCostPerDay,
            IReadOnlyList<KeyValuePair<int, double>> loadingDistribution,
            IReadOnlyList<KeyValuePair<int, double>> weighingDistribution,
            IReadOnlyList<KeyValuePair<int, double>> travelingDistribution)
        {
            return Run(
                coalVolume,
                truckCount,
                truckLoadVolume,
                truckCostPerDay,
                loaderCount,
                loaderCostPerDay,
                scalerCount,
                scalerCostPerDay,
                projectDurationDays,
                delayCostPerDay,
                loadingDistribution,
                weighingDistribution,
                travelingDistribution,
                SimulationEvaluationMode.Stochastic,
                simulationSeed: 0);
        }

        /// <summary>
        /// Runs a simulation using stochastic (seeded) or expected-time durations.
        /// </summary>
        public static SimulationOutput Run(
            float coalVolume,
            float truckCount,
            float truckLoadVolume,
            float truckCostPerDay,
            float loaderCount,
            float loaderCostPerDay,
            float scalerCount,
            float scalerCostPerDay,
            float projectDurationDays,
            float delayCostPerDay,
            IReadOnlyList<KeyValuePair<int, double>> loadingDistribution,
            IReadOnlyList<KeyValuePair<int, double>> weighingDistribution,
            IReadOnlyList<KeyValuePair<int, double>> travelingDistribution,
            SimulationEvaluationMode mode,
            int simulationSeed)
        {
            var parameters = new SimulationParameters(
                coalVolume,
                truckCount,
                truckLoadVolume,
                truckCostPerDay,
                loaderCount,
                loaderCostPerDay,
                scalerCount,
                scalerCostPerDay,
                projectDurationDays,
                delayCostPerDay,
                loadingDistribution,
                weighingDistribution,
                travelingDistribution);

            IOperationDurationSampler sampler = OperationDurationSamplerFactory.Create(mode, simulationSeed);
            SimulationResult result = new DumpTruckSimulationEngine(parameters, sampler).Run();
            return ToOutput(result);
        }

        private static SimulationOutput ToOutput(SimulationResult result) =>
            new SimulationOutput
            {
                TruckUtilization = result.UtilTruck,
                LoaderUtilization = result.UtilLoader,
                ScalerUtilization = result.UtilScaler,
                TotalDays = result.TotalDays,
                TotalCost = result.TotalCost,
                TruckCost = result.TotalCostTruck,
                LoaderCost = result.TotalCostLoader,
                ScalerCost = result.TotalCostScaler,
                DelayCost = result.CostOfDelay,
                DelayDays = result.DelayDuration
            };
    }
}
