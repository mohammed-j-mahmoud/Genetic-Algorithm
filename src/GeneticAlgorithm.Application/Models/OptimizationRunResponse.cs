using GeneticAlgorithm.Application.Models;

namespace GeneticAlgorithm.Application
{
    /// <summary>
    /// API-friendly summary of an optimization run.
    /// </summary>
    public sealed class OptimizationRunResponse
    {
        public string Application { get; set; }
        public string Phase { get; set; }
        public string Strategy { get; set; }
        public string Tab { get; set; }
        public int BestGeneration { get; set; }
        public int GenerationsCompleted { get; set; }
        public bool StoppedEarly { get; set; }
        public int Trucks { get; set; }
        public int Loaders { get; set; }
        public int Scalers { get; set; }
        public double Fitness { get; set; }
        public double TotalCost { get; set; }
        public double TotalDays { get; set; }
        public int CombinationsEvaluated { get; set; }
        public long SimulationCalls { get; set; }
        public long CacheHits { get; set; }
        public string MethodSummary { get; set; }

        public static OptimizationRunResponse FromResult(OptimizationRunResult result)
        {
            if (result == null)
                throw new System.ArgumentNullException(nameof(result));

            var best = result.BestChromosome;
            return new OptimizationRunResponse
            {
                Application = OptimizationPhaseDisplay.ApplicationTitle,
                Phase = result.Phase.ToString(),
                Strategy = OptimizationPhaseDisplay.GetStrategyName(result.Phase),
                Tab = OptimizationPhaseDisplay.GetStrategyName(result.Phase),
                BestGeneration = result.BestGeneration,
                GenerationsCompleted = result.GenerationsCompleted,
                StoppedEarly = result.StoppedEarly,
                Trucks = best?.Genes[0] ?? 0,
                Loaders = best?.Genes[1] ?? 0,
                Scalers = best?.Genes[2] ?? 0,
                Fitness = best?.Fitness ?? 0,
                TotalCost = best?.TotalCost ?? 0,
                TotalDays = best?.TotalDays ?? 0,
                CombinationsEvaluated = result.CombinationsEvaluated,
                SimulationCalls = result.SimulationCalls,
                CacheHits = result.CacheHits,
                MethodSummary = result.MethodSummary
            };
        }

        public static OptimizationRunResponse FromGeneticAlgorithm(OptimizationRunResult result)
        {
            if (result == null)
                throw new System.ArgumentNullException(nameof(result));

            var best = result.BestChromosome;
            return new OptimizationRunResponse
            {
                Application = OptimizationPhaseDisplay.ApplicationTitle,
                Phase = "GeneticAlgorithm",
                Strategy = OptimizationPhaseDisplay.GeneticAlgorithmTab,
                Tab = OptimizationPhaseDisplay.GeneticAlgorithmTab,
                BestGeneration = result.BestGeneration,
                GenerationsCompleted = result.GenerationsCompleted,
                StoppedEarly = result.StoppedEarly,
                Trucks = best?.Genes[0] ?? 0,
                Loaders = best?.Genes[1] ?? 0,
                Scalers = best?.Genes[2] ?? 0,
                Fitness = best?.Fitness ?? 0,
                TotalCost = best?.TotalCost ?? 0,
                TotalDays = best?.TotalDays ?? 0,
                CombinationsEvaluated = result.CombinationsEvaluated,
                SimulationCalls = result.SimulationCalls,
                CacheHits = result.CacheHits,
                MethodSummary = result.MethodSummary
                    ?? "GA with seeded stochastic simulation fitness (same as Genetic Algorithm tab)."
            };
        }
    }
}
