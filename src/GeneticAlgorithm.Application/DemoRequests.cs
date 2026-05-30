using System.Collections.Generic;
using GeneticAlgorithm.Application.Models;

namespace GeneticAlgorithm.Application
{
    /// <summary>
    /// Shared demo inputs for CLI and samples.
    /// </summary>
    public static class DemoRequests
    {
        public static SimulationRequest CreateSimulation() =>
            new SimulationRequest
            {
                CoalVolume = 10000,
                TruckCount = 6,
                TruckLoadVolume = 20,
                TruckCostPerDay = 1000,
                LoaderCount = 2,
                LoaderCostPerDay = 2000,
                ScalerCount = 2,
                ScalerCostPerDay = 3000,
                ProjectDurationDays = 120,
                DelayCostPerDay = 10000,
                LoadingDistribution = new List<KeyValuePair<int, double>>
                {
                    new KeyValuePair<int, double>(5, 0.3),
                    new KeyValuePair<int, double>(10, 0.5),
                    new KeyValuePair<int, double>(15, 0.2)
                },
                WeighingDistribution = new List<KeyValuePair<int, double>>
                {
                    new KeyValuePair<int, double>(12, 0.7),
                    new KeyValuePair<int, double>(16, 0.3)
                },
                TravelingDistribution = new List<KeyValuePair<int, double>>
                {
                    new KeyValuePair<int, double>(40, 0.4),
                    new KeyValuePair<int, double>(60, 0.3),
                    new KeyValuePair<int, double>(80, 0.2),
                    new KeyValuePair<int, double>(100, 0.1)
                }
            };

        /// <summary>Economic/timing inputs shared by search tabs — no fixed fleet counts (same as WinForms GA inputs).</summary>
        public static SimulationRequest CreateSearchSimulation()
        {
            SimulationRequest demo = CreateSimulation();
            return new SimulationRequest
            {
                CoalVolume = demo.CoalVolume,
                TruckLoadVolume = demo.TruckLoadVolume,
                TruckCostPerDay = demo.TruckCostPerDay,
                LoaderCostPerDay = demo.LoaderCostPerDay,
                ScalerCostPerDay = demo.ScalerCostPerDay,
                ProjectDurationDays = demo.ProjectDurationDays,
                DelayCostPerDay = demo.DelayCostPerDay,
                LoadingDistribution = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>>(demo.LoadingDistribution),
                WeighingDistribution = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>>(demo.WeighingDistribution),
                TravelingDistribution = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>>(demo.TravelingDistribution)
            };
        }

        public static GeneticOptimizationRequest CreateOptimization(int generations = 20) =>
            new GeneticOptimizationRequest
            {
                PopulationSize = 20,
                MaxTrucks = 6,
                MaxLoaders = 2,
                MaxScalers = 2,
                Generations = generations,
                MutationRate = 0.01,
                Simulation = CreateSearchSimulation()
            };
    }
}
