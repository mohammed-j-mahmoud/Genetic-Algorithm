using System;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Genetics;
using GeneticAlgorithm.Core.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.Core.Tests
{
    [TestClass]
    public class PhaseOptimizationTests
    {
        [TestMethod]
        public void Phase1_BruteForce_FindsBestCombo()
        {
            var request = DemoRequest();
            request.MaxTrucks = 2;
            request.MaxLoaders = 2;
            request.MaxScalers = 2;

            OptimizationRunResult result = new PhaseOptimizationService().RunPhase1(request);
            Assert.IsNotNull(result.BestChromosome);
            Assert.AreEqual(OptimizationPhase.Phase1, result.Phase);
            Assert.AreEqual(8, result.CombinationsEvaluated);
            Assert.IsTrue(result.BestChromosome.Fitness > 0);
        }

        [TestMethod]
        public void SeededStochastic_IsReproducibleForSameFleet()
        {
            var request = DemoRequest();
            var evaluator = new SimulationEvaluator();
            int runSeed = 42;

            var first = evaluator.Evaluate(request.Simulation, 2, 1, 1, SimulationEvaluationMode.Stochastic, runSeed);
            var second = evaluator.Evaluate(request.Simulation, 2, 1, 1, SimulationEvaluationMode.Stochastic, runSeed);

            Assert.AreEqual(first.TotalCost, second.TotalCost);
            Assert.AreEqual(first.TotalDays, second.TotalDays);
        }

        [TestMethod]
        public void ExpectedTime_IsDeterministicForSameFleet()
        {
            var request = DemoRequest();
            var evaluator = new SimulationEvaluator();

            var first = evaluator.Evaluate(request.Simulation, 3, 2, 2, SimulationEvaluationMode.ExpectedTimes, 1);
            var second = evaluator.Evaluate(request.Simulation, 3, 2, 2, SimulationEvaluationMode.ExpectedTimes, 99);

            Assert.AreEqual(first.TotalCost, second.TotalCost);
        }

        [TestMethod]
        public void Phase4_DynamicProgramming_FindsBestCombo()
        {
            var request = DemoRequest();
            request.MaxTrucks = 2;
            request.MaxLoaders = 2;
            request.MaxScalers = 2;

            OptimizationRunResult result = new PhaseOptimizationService().RunPhase4(request);
            Assert.IsNotNull(result.BestChromosome);
            Assert.AreEqual(OptimizationPhase.Phase4, result.Phase);
            Assert.AreEqual(8, result.CombinationsEvaluated);
            Assert.IsTrue(result.BestChromosome.Fitness > 0);
        }

        [TestMethod]
        public void Phase4_MatchesPhase1OptimalFleet()
        {
            var request = DemoRequest();
            request.MaxTrucks = 2;
            request.MaxLoaders = 2;
            request.MaxScalers = 2;

            var service = new PhaseOptimizationService();
            OptimizationRunResult exhaustive = service.RunPhase1(request);
            OptimizationRunResult dynamicProgramming = service.RunPhase4(request);

            Assert.AreEqual(exhaustive.BestChromosome.Genes[0], dynamicProgramming.BestChromosome.Genes[0]);
            Assert.AreEqual(exhaustive.BestChromosome.Genes[1], dynamicProgramming.BestChromosome.Genes[1]);
            Assert.AreEqual(exhaustive.BestChromosome.Genes[2], dynamicProgramming.BestChromosome.Genes[2]);
        }

        private static GeneticOptimizationRequest DemoRequest() =>
            new GeneticOptimizationRequest
            {
                PopulationSize = 4,
                MaxTrucks = 2,
                MaxLoaders = 2,
                MaxScalers = 2,
                Generations = 3,
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
    }
}
