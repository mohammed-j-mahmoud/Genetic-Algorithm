using GeneticAlgorithm.Core.Genetics;

namespace GeneticAlgorithm.Application.Models
{
    public sealed class OptimizationRunResult
    {
        public int BestGeneration { get; set; }
        public DNA BestChromosome { get; set; }
    }
}
