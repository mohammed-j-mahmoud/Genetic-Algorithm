using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.Core.Tests
{
    [TestClass]
    public class SimulationServiceTests
    {
        [TestMethod]
        public void Run_ReturnsPositiveCost()
        {
            var output = new SimulationService().Run(new SimulationRequest
            {
                CoalVolume = 50,
                TruckCount = 2,
                TruckLoadVolume = 20,
                TruckCostPerDay = 100,
                LoaderCount = 1,
                LoaderCostPerDay = 200,
                ScalerCount = 1,
                ScalerCostPerDay = 300,
                ProjectDurationDays = 30,
                DelayCostPerDay = 50
            });

            Assert.IsTrue(output.TotalCost > 0);
        }
    }
}
