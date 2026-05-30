using GeneticAlgorithm.Core.Genetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.Core.Tests
{
    [TestClass]
    public class FleetFitnessCalculatorTests
    {
        private const float CoalVolume = 10000f;
        private const double FitnessScale = 1000.0;

        [TestMethod]
        public void Compute_FavorsFewerDays_WhenTotalCostIsEqual()
        {
            const double totalCost = 50000;

            double fewerDays = FleetFitnessCalculator.Compute(CoalVolume, FitnessScale, totalCost, 10);
            double moreDays = FleetFitnessCalculator.Compute(CoalVolume, FitnessScale, totalCost, 20);

            Assert.IsTrue(fewerDays > moreDays, "Equal cost should favor fewer project days.");
        }

        [TestMethod]
        public void Compute_FavorsLowerCost_OverFewerDays()
        {
            double lowerCostMoreDays = FleetFitnessCalculator.Compute(CoalVolume, FitnessScale, 100000, 5);
            double higherCostFewerDays = FleetFitnessCalculator.Compute(CoalVolume, FitnessScale, 100001, 1);

            Assert.IsTrue(lowerCostMoreDays > higherCostFewerDays,
                "A one-dollar cost improvement must outweigh any days tie-break.");
        }

        [TestMethod]
        public void Compute_FavorsLowerCost_OverFewerDays_AtLowCost()
        {
            Assert.IsTrue(
                FleetFitnessCalculator.Compute(CoalVolume, FitnessScale, 0, 1000)
                > FleetFitnessCalculator.Compute(CoalVolume, FitnessScale, 1, 0),
                "A one-dollar cost improvement must win even at the lowest costs.");
        }
    }
}
