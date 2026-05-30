using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Application
{
    /// <summary>
    /// Runs simulations with seeded stochastic or expected-time evaluation.
    /// </summary>
    public sealed class SimulationEvaluator
    {
        private readonly SimulationService _simulationService = new SimulationService();

        /// <summary>
        /// Evaluates a fleet configuration using the given mode and optimization run seed.
        /// </summary>
        public DumpTruckSimulation.SimulationOutput Evaluate(
            SimulationRequest baseRequest,
            int trucks,
            int loaders,
            int scalers,
            SimulationEvaluationMode mode,
            int runSeed)
        {
            SimulationRequest request = CloneWithFleet(baseRequest, trucks, loaders, scalers);
            int simulationSeed = SimulationSeedFactory.ForFleet(trucks, loaders, scalers, runSeed);
            return _simulationService.Run(request, mode, simulationSeed);
        }

        private static SimulationRequest CloneWithFleet(
            SimulationRequest source,
            int trucks,
            int loaders,
            int scalers) =>
            new SimulationRequest
            {
                CoalVolume = source.CoalVolume,
                TruckCount = trucks,
                TruckLoadVolume = source.TruckLoadVolume,
                TruckCostPerDay = source.TruckCostPerDay,
                LoaderCount = loaders,
                LoaderCostPerDay = source.LoaderCostPerDay,
                ScalerCount = scalers,
                ScalerCostPerDay = source.ScalerCostPerDay,
                ProjectDurationDays = source.ProjectDurationDays,
                DelayCostPerDay = source.DelayCostPerDay,
                LoadingDistribution = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>>(source.LoadingDistribution),
                WeighingDistribution = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>>(source.WeighingDistribution),
                TravelingDistribution = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>>(source.TravelingDistribution)
            };
    }
}
