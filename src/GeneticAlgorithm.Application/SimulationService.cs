using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Application
{
    /// <summary>
    /// Runs a simulation from a request DTO.
    /// </summary>
    public sealed class SimulationService
    {
        /// <summary>
        /// Runs a stochastic simulation (legacy default; seed 0).
        /// </summary>
        public DumpTruckSimulation.SimulationOutput Run(SimulationRequest request) =>
            Run(request, SimulationEvaluationMode.Stochastic, simulationSeed: 0);

        /// <summary>
        /// Runs a simulation with explicit evaluation mode and reproducible seed.
        /// </summary>
        public DumpTruckSimulation.SimulationOutput Run(
            SimulationRequest request,
            SimulationEvaluationMode mode,
            int simulationSeed)
        {
            return DumpTruckSimulation.Run(
                request.CoalVolume,
                request.TruckCount,
                request.TruckLoadVolume,
                request.TruckCostPerDay,
                request.LoaderCount,
                request.LoaderCostPerDay,
                request.ScalerCount,
                request.ScalerCostPerDay,
                request.ProjectDurationDays,
                request.DelayCostPerDay,
                DistributionNormalizer.Normalize(request.LoadingDistribution),
                DistributionNormalizer.Normalize(request.WeighingDistribution),
                DistributionNormalizer.Normalize(request.TravelingDistribution),
                mode,
                simulationSeed);
        }
    }
}
