using System;
using System.Collections.Generic;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Genetics;
using GeneticAlgorithm.Core.Optimization;
using GeneticAlgorithm.Core.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.Core.Tests
{
    [TestClass]
    public class FleetSurrogateCostModelTests
    {
        [TestMethod]
        public void SurrogateFormula_UsesWorkDayMinutesAndFleetBottlenecks()
        {
            // All stages deterministic at 10 min. trips = ceil(500/10) = 50.
            // 1 scaler: bottleneck throughput = min(2/10, 1/10, 6/10) = 0.1 → (49/0.1) + 30 = 520 min.
            // 2 scalers: bottleneck throughput = min(2/10, 2/10, 6/10) = 0.2 → (49/0.2) + 30 = 275 min.
            var loading = DiscreteDistribution.FromEntries(
                new List<KeyValuePair<int, double>> { new KeyValuePair<int, double>(10, 1) },
                DiscreteDistribution.DefaultLoading);
            var weighing = DiscreteDistribution.FromEntries(
                new List<KeyValuePair<int, double>> { new KeyValuePair<int, double>(10, 1) },
                DiscreteDistribution.DefaultWeighing);
            var traveling = DiscreteDistribution.FromEntries(
                new List<KeyValuePair<int, double>> { new KeyValuePair<int, double>(10, 1) },
                DiscreteDistribution.DefaultTraveling);

            double oneScalerMinutes = FleetSurrogateCostModel.EstimateTotalTimeMinutes(
                500, 10, loading, weighing, traveling, 6, 2, 1);
            double twoScalerMinutes = FleetSurrogateCostModel.EstimateTotalTimeMinutes(
                500, 10, loading, weighing, traveling, 6, 2, 2);

            Assert.AreEqual(520.0, oneScalerMinutes, 1e-9, "1-scaler pipelined schedule should be 520 min.");
            Assert.AreEqual(275.0, twoScalerMinutes, 1e-9, "2-scaler pipelined schedule should be 275 min.");
            Assert.IsTrue(twoScalerMinutes < oneScalerMinutes, "Extra scalers should shorten the pipelined surrogate schedule.");
        }

        [TestMethod]
        public void SurrogateFormula_RankingOrder_MoreResourcesReducesCostUpToSaturation()
        {
            var loading = DiscreteDistribution.FromEntries(
                new List<KeyValuePair<int, double>> { new KeyValuePair<int, double>(10, 1) },
                DiscreteDistribution.DefaultLoading);
            var weighing = DiscreteDistribution.FromEntries(
                new List<KeyValuePair<int, double>> { new KeyValuePair<int, double>(10, 1) },
                DiscreteDistribution.DefaultWeighing);
            var traveling = DiscreteDistribution.FromEntries(
                new List<KeyValuePair<int, double>> { new KeyValuePair<int, double>(10, 1) },
                DiscreteDistribution.DefaultTraveling);

            // Cost should decrease when adding a scaler relieves the bottleneck.
            double costOneScaler = FleetSurrogateCostModel.EstimateTotalCost(
                500, 10, 100f, 200f, 300f, 30f, 5000f,
                loading, weighing, traveling, 6, 2, 1);
            double costTwoScalers = FleetSurrogateCostModel.EstimateTotalCost(
                500, 10, 100f, 200f, 300f, 30f, 5000f,
                loading, weighing, traveling, 6, 2, 2);

            Assert.IsTrue(costTwoScalers < costOneScaler,
                "Adding a scaler that relieves the bottleneck should reduce total cost.");
        }

        /// <summary>
        /// Phase 3 with small bounds (6×2×2 = 24 combos) always uses expected-time ranking, not surrogate.
        /// This test verifies the expected-time ranking path produces the same optimal as Phase 1 brute-force.
        /// </summary>
        [TestMethod]
        public void Phase3_ExpectedTimeRanking_MatchesPhase1BruteForce_OnSmallBounds()
        {
            var request = DemoOptimizationRequest();
            var service = new PhaseOptimizationService();
            OptimizationRunResult exhaustive = service.RunPhase1(request);
            OptimizationRunResult phase3 = service.RunPhase3(request);

            // Confirm Phase 3 used the expected-time path (not the surrogate) for these bounds.
            StringAssert.Contains(phase3.MethodSummary, "Expected-time ranking",
                "6×2×2 = 24 combos is below the surrogate threshold; expected-time ranking must be used.");

            // Both phases rank with the same expected-time model; their optima must agree.
            Assert.AreEqual(exhaustive.BestChromosome.Genes[0], phase3.BestChromosome.Genes[0], "Trucks must match.");
            Assert.AreEqual(exhaustive.BestChromosome.Genes[1], phase3.BestChromosome.Genes[1], "Loaders must match.");
            Assert.AreEqual(exhaustive.BestChromosome.Genes[2], phase3.BestChromosome.Genes[2], "Scalers must match.");
        }

        private static GeneticOptimizationRequest DemoOptimizationRequest() =>
            new GeneticOptimizationRequest
            {
                PopulationSize = 20,
                MaxTrucks = 6,
                MaxLoaders = 2,
                MaxScalers = 2,
                Generations = 100,
                Simulation = DemoSimulation()
            };

        private static SimulationRequest DemoSimulation() =>
            new SimulationRequest
            {
                CoalVolume = 10000,
                TruckLoadVolume = 20,
                TruckCostPerDay = 1000,
                LoaderCostPerDay = 2000,
                ScalerCostPerDay = 3000,
                ProjectDurationDays = 120,
                DelayCostPerDay = 10000,
                LoadingDistribution = new List<KeyValuePair<int, double>>
                {
                    new KeyValuePair<int, double>(5, 0.3),
                    new KeyValuePair<int, double>(10, 0.5),
                    new KeyValuePair<int, double>(15, 0.2)
                },
                WeighingDistribution = new List<KeyValuePair<int, double>>
                {
                    new KeyValuePair<int, double>(12, 0.7),
                    new KeyValuePair<int, double>(16, 0.3)
                },
                TravelingDistribution = new List<KeyValuePair<int, double>>
                {
                    new KeyValuePair<int, double>(40, 0.4),
                    new KeyValuePair<int, double>(60, 0.3),
                    new KeyValuePair<int, double>(80, 0.2),
                    new KeyValuePair<int, double>(100, 0.1)
                }
            };
    }
}
