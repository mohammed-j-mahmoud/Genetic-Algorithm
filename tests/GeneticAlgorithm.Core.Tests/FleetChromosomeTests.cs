using GeneticAlgorithm.Core.Genetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.Core.Tests
{
    [TestClass]
    public class FleetChromosomeTests
    {
        [TestMethod]
        public void FleetChromosome_Genes_StayWithinBounds()
        {
            var chromosome = new FleetChromosome(6, 2, 2);

            Assert.IsTrue(chromosome.Trucks >= 1 && chromosome.Trucks <= 6);
            Assert.IsTrue(chromosome.Loaders >= 1 && chromosome.Loaders <= 2);
            Assert.IsTrue(chromosome.Scalers >= 1 && chromosome.Scalers <= 2);
        }
    }
}
