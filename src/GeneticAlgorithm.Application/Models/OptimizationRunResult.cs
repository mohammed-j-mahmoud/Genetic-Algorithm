using GeneticAlgorithm.Core.Genetics;

namespace GeneticAlgorithm.Application.Models
{
    /// <summary>
    /// Result of a genetic or phase optimization run.
    /// </summary>
    public sealed class OptimizationRunResult
    {
        public OptimizationPhase Phase { get; set; }
        /// <summary>Generation number when the best chromosome was last improved.</summary>
        public int BestGeneration { get; set; }
        /// <summary>Total generations executed (equals the requested generation count on a full run).</summary>
        public int GenerationsCompleted { get; set; }

        /// <summary>True when the user stopped the run before it finished normally.</summary>
        public bool StoppedEarly { get; set; }
        public DNA BestChromosome { get; set; }
        public DNA[] FinalPopulation { get; set; }
        public string MethodSummary { get; set; }
        public long SimulationCalls { get; set; }
        public long CacheHits { get; set; }
        public int CombinationsEvaluated { get; set; }
    }
}
