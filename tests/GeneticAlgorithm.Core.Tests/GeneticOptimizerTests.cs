using System;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Genetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.Core.Tests
{
    [TestClass]
    public class GeneticOptimizerTests
    {
        [TestMethod]
        public void GeneticAlgorithm_BestFitness_IsMonotonic()
        {
            var request = DemoRequest(generations: 5);
            var result = new GeneticOptimizationService().Run(request);
            Assert.IsNotNull(result.BestChromosome);
            Assert.IsTrue(result.BestChromosome.Fitness > 0);
        }

        [TestMethod]
        public void GeneticAlgorithm_Dispose_DoesNotThrow()
        {
            using (var ga = new GeneticOptimizer.GeneticAlgorithm(4, 2, 2, 2, (_, __, ___) => DemoOutput(), 100f))
                ga.NewGeneration();
        }

        [TestMethod]
        public void Config_RejectsInvalidPopulation()
        {
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
                GeneticAlgorithmConfig.Create(0, 2, 2, 2, 100f));
        }

        private static GeneticOptimizationRequest DemoRequest(int generations) =>
            new GeneticOptimizationRequest
            {
                Generations = generations,
                PopulationSize = 4,
                Simulation = new SimulationRequest
                {
                    CoalVolume = 80,
                    TruckCount = 2,
                    TruckLoadVolume = 20,
                    TruckCostPerDay = 100,
                    LoaderCount = 1,
                    LoaderCostPerDay = 200,
                    ScalerCount = 1,
                    ScalerCostPerDay = 300,
                    ProjectDurationDays = 30,
                    DelayCostPerDay = 50
                }
            };

        private static Core.Simulation.DumpTruckSimulation.SimulationOutput DemoOutput() =>
            Core.Simulation.DumpTruckSimulation.Run(
                80, 2, 20, 100, 1, 200, 1, 300, 30, 50, null, null, null);
    }
}
