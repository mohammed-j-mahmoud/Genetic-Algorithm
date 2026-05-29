using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Application
{
    public sealed class SimulationService
    {
        public DumpTruckSimulation.SimulationOutput Run(SimulationRequest request)
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
                DistributionNormalizer.Normalize(request.TravelingDistribution));
        }
    }
}
