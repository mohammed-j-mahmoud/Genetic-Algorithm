using System;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Genetics;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Application
{

    public sealed class GeneticOptimizationService
    {
        private readonly SimulationService _simulationService = new SimulationService();

        public OptimizationRunResult Run(
            GeneticOptimizationRequest request,
            IProgress<GenerationProgress> progress = null)
        {
            SimulationRequest sim = request.Simulation;

            DumpTruckSimulation.SimulationOutput Fitness(float trucks, float loaders, float scalers)
            {
                var sub = CloneSimulation(sim);
                sub.TruckCount = trucks;
                sub.LoaderCount = loaders;
                sub.ScalerCount = scalers;
                return _simulationService.Run(sub);
            }

            using (var algorithm = new GeneticOptimizer.GeneticAlgorithm(
                request.PopulationSize,
                request.MaxTrucks,
                request.MaxLoaders,
                request.MaxScalers,
                Fitness,
                sim.CoalVolume,
                request.MutationRate))
            {
                for (int generation = 1; generation <= request.Generations; generation++)
                {
                    progress?.Report(new GenerationProgress
                    {
                        Generation = generation,
                        BestFitness = algorithm.BestGene.Fitness
                    });

                    if (generation < request.Generations)
                        algorithm.NewGeneration();
                }

                return new OptimizationRunResult
                {
                    BestGeneration = algorithm.BestGeneGeneration,
                    BestChromosome = algorithm.BestGene
                };
            }
        }

        private static SimulationRequest CloneSimulation(SimulationRequest source) =>
            new SimulationRequest
            {
                CoalVolume = source.CoalVolume,
                TruckCount = source.TruckCount,
                TruckLoadVolume = source.TruckLoadVolume,
                TruckCostPerDay = source.TruckCostPerDay,
                LoaderCount = source.LoaderCount,
                LoaderCostPerDay = source.LoaderCostPerDay,
                ScalerCount = source.ScalerCount,
                ScalerCostPerDay = source.ScalerCostPerDay,
                ProjectDurationDays = source.ProjectDurationDays,
                DelayCostPerDay = source.DelayCostPerDay,
                LoadingDistribution = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>>(source.LoadingDistribution),
                WeighingDistribution = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>>(source.WeighingDistribution),
                TravelingDistribution = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>>(source.TravelingDistribution)
            };
    }
}
