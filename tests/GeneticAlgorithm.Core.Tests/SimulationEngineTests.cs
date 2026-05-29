using System;
using System.Collections.Generic;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Core.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.Core.Tests
{
    [TestClass]
    public class SimulationEngineTests
    {
        [TestMethod]
        public void Run_Completes_WithValidParameters()
        {
            var output = DumpTruckSimulation.Run(
                100f, 2f, 20f, 100f, 1f, 200f, 1f, 300f, 30f, 50f,
                DefaultDistribution(5), DefaultDistribution(10), DefaultDistribution(20));

            Assert.IsTrue(output.TotalCost > 0);
            Assert.IsTrue(output.TotalDays >= 0);
        }

        [TestMethod]
        public void Parameters_RejectsZeroLoaders()
        {
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
                new SimulationParameters(100, 2, 20, 100, 0, 200, 1, 300, 30, 50, null, null, null));
        }

        [TestMethod]
        public void Parameters_RejectsNegativeCosts()
        {
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
                new SimulationParameters(100, 2, 20, -1, 1, 200, 1, 300, 30, 50, null, null, null));
        }

        [TestMethod]
        public void DistributionNormalizer_ScalesToOne()
        {
            var normalized = DistributionNormalizer.Normalize(new List<KeyValuePair<int, double>>
            {
                new KeyValuePair<int, double>(5, 0.25),
                new KeyValuePair<int, double>(10, 0.25)
            });

            double sum = 0;
            foreach (var entry in normalized)
                sum += entry.Value;

            Assert.AreEqual(1.0, sum, 1e-9);
        }

        private static List<KeyValuePair<int, double>> DefaultDistribution(int minutes) =>
            new List<KeyValuePair<int, double>> { new KeyValuePair<int, double>(minutes, 1.0) };
    }
}
