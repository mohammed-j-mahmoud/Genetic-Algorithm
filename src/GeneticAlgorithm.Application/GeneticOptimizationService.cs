using System;
using System.Threading;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Genetics;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Application
{
    public sealed class GeneticOptimizationService
    {
        private readonly SimulationEvaluator _evaluator = new SimulationEvaluator();

        /// <summary>
        /// Runs GA with seeded stochastic simulation (reproducible cache per fleet triple).
        /// </summary>
        public OptimizationRunResult Run(
            GeneticOptimizationRequest request,
            IProgress<GenerationProgress> progress = null,
            CancellationToken cancellationToken = default) =>
            Run(request, SimulationEvaluationMode.Stochastic, Environment.TickCount, progress, prewarmCache: true, cancellationToken);

        /// <summary>
        /// Runs GA using the specified simulation evaluation mode and optimization run seed.
        /// </summary>
        public OptimizationRunResult Run(
            GeneticOptimizationRequest request,
            SimulationEvaluationMode fitnessMode,
            int runSeed,
            IProgress<GenerationProgress> progress = null,
            bool prewarmCache = true,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.Simulation == null)
                throw new ArgumentNullException(nameof(request.Simulation));

            SimulationRequest sim = request.Simulation;

            DumpTruckSimulation.SimulationOutput Fitness(float trucks, float loaders, float scalers) =>
                _evaluator.Evaluate(
                    sim,
                    (int)trucks,
                    (int)loaders,
                    (int)scalers,
                    fitnessMode,
                    runSeed);

            using (var algorithm = new GeneticOptimizer.GeneticAlgorithm(
                request.PopulationSize,
                request.MaxTrucks,
                request.MaxLoaders,
                request.MaxScalers,
                Fitness,
                sim.CoalVolume,
                request.MutationRate,
                prewarmCache))
            {
                algorithm.RunGenerations(request.Generations, (generation, bestFitness) =>
                {
                    progress?.Report(new GenerationProgress
                    {
                        Generation = generation,
                        BestFitness = bestFitness
                    });
                }, cancellationToken);

                return new OptimizationRunResult
                {
                    BestGeneration = algorithm.BestGeneGeneration,
                    GenerationsCompleted = algorithm.Generation,
                    StoppedEarly = cancellationToken.IsCancellationRequested,
                    BestChromosome = algorithm.BestGene,
                    FinalPopulation = algorithm.Population,
                    SimulationCalls = algorithm.SimulationCalls,
                    CacheHits = algorithm.CacheHits,
                    CombinationsEvaluated = request.MaxTrucks * request.MaxLoaders * request.MaxScalers,
                    MethodSummary = "GA with seeded stochastic simulation fitness (same as Genetic Algorithm tab)."
                };
            }
        }
    }
}
