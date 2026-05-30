using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Genetics;
using GeneticAlgorithm.Core.Optimization;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Application
{
    /// <summary>
    /// Runs Phase 1–4 optimization strategies for side-by-side comparison.
    /// </summary>
    public sealed class PhaseOptimizationService
    {
        private const int VerifyTopCount = 5;
        private const double FitnessScale = 1000.0;
        /// <summary>When combos are at or below this count, rank with expected-time simulation (same model as exhaustive/DP).</summary>
        private const int ExpectedTimeRankingComboLimit = 50_000;

        private readonly SimulationEvaluator _evaluator = new SimulationEvaluator();

        /// <summary>
        /// Phase 1: brute force all combos using expected-time simulation (cached), then seeded stochastic verify on the best.
        /// </summary>
        public OptimizationRunResult RunPhase1(
            GeneticOptimizationRequest request,
            IProgress<int> progress = null,
            CancellationToken cancellationToken = default)
        {
            ValidateRequest(request);
            int runSeed = Environment.TickCount;
            GeneticAlgorithmConfig config = CreateConfig(request);

            var optimizer = new BruteForceOptimizer(
                config,
                (trucks, loaders, scalers) =>
                    _evaluator.Evaluate(
                        request.Simulation,
                        trucks,
                        loaders,
                        scalers,
                        SimulationEvaluationMode.ExpectedTimes,
                        runSeed),
                prewarmCache: true);

            DNA bestExpected = optimizer.Run(progress, cancellationToken);
            bool stoppedEarly = cancellationToken.IsCancellationRequested;
            DNA best = stoppedEarly
                ? bestExpected
                : VerifySingle(
                    request.Simulation,
                    bestExpected.Genes[0],
                    bestExpected.Genes[1],
                    bestExpected.Genes[2],
                    runSeed);

            return new OptimizationRunResult
            {
                Phase = OptimizationPhase.Phase1,
                BestGeneration = 1,
                GenerationsCompleted = 1,
                StoppedEarly = stoppedEarly,
                BestChromosome = best,
                CombinationsEvaluated = optimizer.CombinationsEvaluated,
                SimulationCalls = optimizer.SimulationCalls + (stoppedEarly ? 0 : 1),
                CacheHits = optimizer.CacheHits,
                MethodSummary = stoppedEarly
                    ? $"Stopped early after {optimizer.CombinationsEvaluated} combinations (expected-time best shown)."
                    : "Brute force (expected-time + cache). Final metrics from seeded stochastic verify."
            };
        }

        /// <summary>
        /// Phase 2: GA on expected-time fitness, then parallel seeded stochastic verify on top candidates.
        /// </summary>
        public OptimizationRunResult RunPhase2(
            GeneticOptimizationRequest request,
            IProgress<int> progress = null,
            CancellationToken cancellationToken = default)
        {
            ValidateRequest(request);
            int runSeed = Environment.TickCount;

            OptimizationRunResult gaResult = new GeneticOptimizationService().Run(
                request,
                SimulationEvaluationMode.ExpectedTimes,
                runSeed,
                new Progress<GenerationProgress>(p =>
                    progress?.Report(p.Generation * 100 / Math.Max(1, request.Generations))),
                prewarmCache: true,
                cancellationToken);

            bool stoppedEarly = cancellationToken.IsCancellationRequested;
            if (stoppedEarly)
            {
                return new OptimizationRunResult
                {
                    Phase = OptimizationPhase.Phase2,
                    BestGeneration = gaResult.BestGeneration,
                    GenerationsCompleted = gaResult.GenerationsCompleted,
                    StoppedEarly = true,
                    BestChromosome = gaResult.BestChromosome,
                    FinalPopulation = gaResult.FinalPopulation,
                    SimulationCalls = gaResult.SimulationCalls,
                    CacheHits = gaResult.CacheHits,
                    CombinationsEvaluated = request.MaxTrucks * request.MaxLoaders * request.MaxScalers,
                    MethodSummary =
                        $"Stopped early during GA after {gaResult.GenerationsCompleted} generations (expected-time best shown)."
                };
            }

            var candidates = CollectTopCandidates(gaResult.BestChromosome, gaResult.FinalPopulation);
            DNA best = VerifyCandidates(request.Simulation, candidates, runSeed);

            return new OptimizationRunResult
            {
                Phase = OptimizationPhase.Phase2,
                BestGeneration = gaResult.BestGeneration,
                GenerationsCompleted = gaResult.GenerationsCompleted,
                BestChromosome = best,
                SimulationCalls = gaResult.SimulationCalls + candidates.Count,
                CacheHits = gaResult.CacheHits,
                CombinationsEvaluated = request.MaxTrucks * request.MaxLoaders * request.MaxScalers,
                MethodSummary =
                    $"GA (expected-time fitness) + top {candidates.Count} seeded stochastic verify (parallel)."
            };
        }

        /// <summary>
        /// Phase 3: rank all combos (expected-time simulation when the search space is small, otherwise
        /// pipelined surrogate formula), then parallel seeded stochastic verify on top candidates.
        /// </summary>
        public OptimizationRunResult RunPhase3(
            GeneticOptimizationRequest request,
            IProgress<int> progress = null,
            CancellationToken cancellationToken = default)
        {
            ValidateRequest(request);
            int runSeed = Environment.TickCount;
            SimulationRequest sim = request.Simulation;
            int total = request.MaxTrucks * request.MaxLoaders * request.MaxScalers;
            bool useExpectedTimeRanking = total <= ExpectedTimeRankingComboLimit;

            List<(int trucks, int loaders, int scalers, double fitness)> ranked;
            long simulationCalls;
            long cacheHits;
            string rankingSummary;

            if (useExpectedTimeRanking)
            {
                ranked = RankCombinationsWithExpectedTime(
                    request,
                    runSeed,
                    progress,
                    cancellationToken,
                    out simulationCalls,
                    out cacheHits,
                    out int evaluated);
                rankingSummary =
                    $"Expected-time ranking ({evaluated} combos, cached) + top {VerifyTopCount} seeded stochastic verify.";
            }
            else
            {
                ranked = RankCombinationsWithSurrogate(request, sim, progress, cancellationToken, out int evaluated);
                simulationCalls = 0;
                cacheHits = 0;
                rankingSummary =
                    $"Surrogate formula ranking ({evaluated} combos, large search space) + top {VerifyTopCount} seeded stochastic verify.";
            }

            var topTuples = ranked
                .OrderByDescending(r => r.fitness)
                .Take(VerifyTopCount)
                .ToList();

            if (cancellationToken.IsCancellationRequested)
            {
                DNA partialBest = topTuples.Count == 0
                    ? new DNA()
                    : CreateRankedDna(topTuples[0], sim);

                return new OptimizationRunResult
                {
                    Phase = OptimizationPhase.Phase3,
                    BestGeneration = 1,
                    GenerationsCompleted = 1,
                    StoppedEarly = true,
                    BestChromosome = partialBest,
                    CombinationsEvaluated = ranked.Count,
                    SimulationCalls = simulationCalls,
                    CacheHits = cacheHits,
                    MethodSummary = $"Stopped early during ranking ({ranked.Count} of {total} combinations)."
                };
            }

            var candidateDnas = topTuples
                .Select(r => CreateRankedDna(r, sim))
                .ToList();

            DNA best = VerifyCandidates(sim, candidateDnas, runSeed);

            return new OptimizationRunResult
            {
                Phase = OptimizationPhase.Phase3,
                BestGeneration = 1,
                GenerationsCompleted = 1,
                BestChromosome = best,
                CombinationsEvaluated = total,
                SimulationCalls = simulationCalls + candidateDnas.Count,
                CacheHits = cacheHits,
                MethodSummary = rankingSummary
            };
        }

        private List<(int trucks, int loaders, int scalers, double fitness)> RankCombinationsWithExpectedTime(
            GeneticOptimizationRequest request,
            int runSeed,
            IProgress<int> progress,
            CancellationToken cancellationToken,
            out long simulationCalls,
            out long cacheHits,
            out int evaluated)
        {
            var cache = new SimulationFitnessCache(
                (trucks, loaders, scalers) =>
                    FitnessSnapshot.FromSimulation(
                        _evaluator.Evaluate(
                            request.Simulation,
                            trucks,
                            loaders,
                            scalers,
                            SimulationEvaluationMode.ExpectedTimes,
                            runSeed),
                        request.Simulation.CoalVolume,
                        FitnessScale),
                request.MaxTrucks,
                request.MaxLoaders,
                request.MaxScalers);

            cache.Prewarm();

            var ranked = new List<(int trucks, int loaders, int scalers, double fitness)>();
            int total = request.MaxTrucks * request.MaxLoaders * request.MaxScalers;
            evaluated = 0;

            for (int trucks = 1; trucks <= request.MaxTrucks && !cancellationToken.IsCancellationRequested; trucks++)
            {
                for (int loaders = 1; loaders <= request.MaxLoaders && !cancellationToken.IsCancellationRequested; loaders++)
                {
                    for (int scalers = 1; scalers <= request.MaxScalers && !cancellationToken.IsCancellationRequested; scalers++)
                    {
                        evaluated++;
                        FitnessSnapshot snapshot = cache.GetOrEvaluate(trucks, loaders, scalers);
                        ranked.Add((trucks, loaders, scalers, snapshot.Fitness));
                        progress?.Report(evaluated * 100 / Math.Max(1, total));
                    }
                }
            }

            simulationCalls = cache.SimulationCalls;
            cacheHits = cache.CacheHits;
            return ranked;
        }

        private static List<(int trucks, int loaders, int scalers, double fitness)> RankCombinationsWithSurrogate(
            GeneticOptimizationRequest request,
            SimulationRequest sim,
            IProgress<int> progress,
            CancellationToken cancellationToken,
            out int evaluated)
        {
            var loading = DiscreteDistribution.FromEntries(
                DistributionNormalizer.Normalize(sim.LoadingDistribution),
                DiscreteDistribution.DefaultLoading);
            var weighing = DiscreteDistribution.FromEntries(
                DistributionNormalizer.Normalize(sim.WeighingDistribution),
                DiscreteDistribution.DefaultWeighing);
            var traveling = DiscreteDistribution.FromEntries(
                DistributionNormalizer.Normalize(sim.TravelingDistribution),
                DiscreteDistribution.DefaultTraveling);

            var ranked = new List<(int trucks, int loaders, int scalers, double fitness)>();
            int total = request.MaxTrucks * request.MaxLoaders * request.MaxScalers;
            evaluated = 0;

            for (int trucks = 1; trucks <= request.MaxTrucks && !cancellationToken.IsCancellationRequested; trucks++)
            {
                for (int loaders = 1; loaders <= request.MaxLoaders && !cancellationToken.IsCancellationRequested; loaders++)
                {
                    for (int scalers = 1; scalers <= request.MaxScalers && !cancellationToken.IsCancellationRequested; scalers++)
                    {
                        evaluated++;
                        double cost = FleetSurrogateCostModel.EstimateTotalCost(
                            sim.CoalVolume,
                            sim.TruckLoadVolume,
                            sim.TruckCostPerDay,
                            sim.LoaderCostPerDay,
                            sim.ScalerCostPerDay,
                            sim.ProjectDurationDays,
                            sim.DelayCostPerDay,
                            loading,
                            weighing,
                            traveling,
                            trucks,
                            loaders,
                            scalers);
                        double fitness = FleetSurrogateCostModel.EstimateFitness(sim.CoalVolume, FitnessScale, cost);
                        ranked.Add((trucks, loaders, scalers, fitness));

                        progress?.Report(evaluated * 100 / Math.Max(1, total));
                    }
                }
            }

            return ranked;
        }

        /// <summary>
        /// Phase 4: bottom-up 3D DP table using expected-time simulation (cached), then seeded stochastic verify on the optimum.
        /// </summary>
        public OptimizationRunResult RunPhase4(
            GeneticOptimizationRequest request,
            IProgress<int> progress = null,
            CancellationToken cancellationToken = default)
        {
            ValidateRequest(request);
            int runSeed = Environment.TickCount;
            GeneticAlgorithmConfig config = CreateConfig(request);

            var optimizer = new FleetDynamicProgrammingOptimizer(
                config,
                (trucks, loaders, scalers) =>
                    _evaluator.Evaluate(
                        request.Simulation,
                        trucks,
                        loaders,
                        scalers,
                        SimulationEvaluationMode.ExpectedTimes,
                        runSeed),
                prewarmCache: true);

            DNA bestExpected = optimizer.Run(progress, cancellationToken);
            bool stoppedEarly = cancellationToken.IsCancellationRequested;
            DNA best = stoppedEarly
                ? bestExpected
                : VerifySingle(
                    request.Simulation,
                    bestExpected.Genes[0],
                    bestExpected.Genes[1],
                    bestExpected.Genes[2],
                    runSeed);

            return new OptimizationRunResult
            {
                Phase = OptimizationPhase.Phase4,
                BestGeneration = 1,
                GenerationsCompleted = 1,
                StoppedEarly = stoppedEarly,
                BestChromosome = best,
                CombinationsEvaluated = optimizer.CombinationsEvaluated,
                SimulationCalls = optimizer.SimulationCalls + (stoppedEarly ? 0 : 1),
                CacheHits = optimizer.CacheHits,
                MethodSummary = stoppedEarly
                    ? $"Stopped early after {optimizer.CombinationsEvaluated} DP cells (expected-time best shown)."
                    : "3D DP table (expected-time + cache, optimal under that model) + seeded stochastic verify."
            };
        }

        private static DNA CreateRankedDna((int trucks, int loaders, int scalers, double fitness) ranked, SimulationRequest sim)
        {
            var dna = new DNA();
            dna.CopyFrom(
                ranked.trucks,
                ranked.loaders,
                ranked.scalers,
                FitnessSnapshot.FromRank(ranked.fitness, sim.CoalVolume, FitnessScale),
                sim.CoalVolume);
            return dna;
        }

        private static void ValidateRequest(GeneticOptimizationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.Simulation == null)
                throw new ArgumentNullException(nameof(request.Simulation));
        }

        private static GeneticAlgorithmConfig CreateConfig(GeneticOptimizationRequest request) =>
            GeneticAlgorithmConfig.Create(
                request.PopulationSize,
                request.MaxTrucks,
                request.MaxLoaders,
                request.MaxScalers,
                request.Simulation.CoalVolume,
                request.MutationRate,
                FitnessScale);

        private DNA VerifySingle(SimulationRequest sim, int trucks, int loaders, int scalers, int runSeed)
        {
            DumpTruckSimulation.SimulationOutput output = _evaluator.Evaluate(
                sim, trucks, loaders, scalers, SimulationEvaluationMode.Stochastic, runSeed);
            var dna = new DNA();
            dna.CopyFrom(
                trucks,
                loaders,
                scalers,
                FitnessSnapshot.FromSimulation(output, sim.CoalVolume, FitnessScale),
                sim.CoalVolume);
            return dna;
        }

        private DNA VerifyCandidates(SimulationRequest sim, IReadOnlyList<DNA> candidates, int runSeed)
        {
            if (candidates == null || candidates.Count == 0)
                throw new InvalidOperationException("No candidates available for stochastic verification.");

            var verified = new DNA[candidates.Count];
            Parallel.For(0, candidates.Count, i =>
            {
                DNA candidate = candidates[i];
                verified[i] = VerifySingle(
                    sim,
                    candidate.Genes[0],
                    candidate.Genes[1],
                    candidate.Genes[2],
                    runSeed);
            });

            return verified.OrderByDescending(d => d.Fitness).First();
        }

        private static List<DNA> CollectTopCandidates(DNA best, DNA[] population)
        {
            var unique = new Dictionary<string, DNA>();
            if (best != null)
                unique[Key(best.Genes[0], best.Genes[1], best.Genes[2])] = best;

            if (population != null)
            {
                foreach (DNA dna in population)
                {
                    if (dna == null)
                        continue;
                    string key = Key(dna.Genes[0], dna.Genes[1], dna.Genes[2]);
                    if (!unique.ContainsKey(key) || dna.Fitness > unique[key].Fitness)
                        unique[key] = dna;
                }
            }

            return unique.Values
                .OrderByDescending(d => d.Fitness)
                .Take(VerifyTopCount)
                .ToList();
        }

        private static string Key(int trucks, int loaders, int scalers) =>
            $"{trucks}:{loaders}:{scalers}";
    }
}
