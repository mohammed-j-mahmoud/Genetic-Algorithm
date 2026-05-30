namespace GeneticAlgorithm.Application.Models
{
    public sealed class GenerationProgress
    {
        public int Generation { get; set; }

        /// <summary>Best fitness among individuals in this generation.</summary>
        public double BestFitness { get; set; }
    }
}
