namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Output of a completed genetic-algorithm run.
    /// </summary>
    public sealed class GeneticAlgoResults
    {
        /// <summary>Generation in which the best chromosome was found.</summary>
        public int BestGeneration;

        /// <summary>Best chromosome across all generations.</summary>
        public DNA BestGene;
    }
}
