using GeneticAlgorithm.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.IntegrationTests
{
    [TestClass]
    public class CliIntegrationTests
    {
        [TestMethod]
        public void Help_ListsAllDesktopTabs()
        {
            CliRunResult result = CliProcessRunner.Run("help");

            Assert.AreEqual(0, result.ExitCode);
            StringAssert.Contains(result.StandardOutput, "genetic-algorithm");
            StringAssert.Contains(result.StandardOutput, OptimizationPhaseDisplay.GeneticAlgorithmTab);
            StringAssert.Contains(result.StandardOutput, "simulation");
            StringAssert.Contains(result.StandardOutput, "exhaustive-search");
            StringAssert.Contains(result.StandardOutput, "genetic-search");
            StringAssert.Contains(result.StandardOutput, "surrogate-search");
            StringAssert.Contains(result.StandardOutput, "dynamic-programming-search");
        }

        [TestMethod]
        public void Simulation_PrintsSimulationOutputs()
        {
            CliRunResult result = CliProcessRunner.RunCommand("simulation", IntegrationTestFixtures.CliQuickSimulationFlags);

            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);
            StringAssert.Contains(result.StandardOutput, OptimizationPhaseDisplay.SimulationTab);
            StringAssert.Contains(result.StandardOutput, "Total Cost:");
            StringAssert.Contains(result.StandardOutput, "Total Days:");
            StringAssert.Contains(result.StandardOutput, "Utilization of Trucks:");
        }

        [TestMethod]
        public void GeneticAlgorithm_PrintsGeneticAlgorithmTab()
        {
            CliRunResult result = CliProcessRunner.RunCommand("genetic-algorithm", IntegrationTestFixtures.CliQuickOptimizationFlags);

            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);
            StringAssert.Contains(result.StandardOutput, OptimizationPhaseDisplay.GeneticAlgorithmTab);
            StringAssert.Contains(result.StandardOutput, "Generation 1/3");
            StringAssert.Contains(result.StandardOutput, "Number of Trucks:");
            StringAssert.Contains(result.StandardOutput, "stochastic simulation fitness");
        }

        [TestMethod]
        public void GeneticSearch_PrintsGeneticSearchTab()
        {
            CliRunResult result = CliProcessRunner.RunCommand("genetic-search", IntegrationTestFixtures.CliQuickOptimizationFlags);

            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);
            StringAssert.Contains(result.StandardOutput, OptimizationPhaseDisplay.GeneticSearch);
            StringAssert.Contains(result.StandardOutput, "expected-time");
        }

        [TestMethod]
        public void ExhaustiveSearch_PrintsExhaustiveSearchTab()
        {
            CliRunResult result = CliProcessRunner.RunCommand("exhaustive-search", IntegrationTestFixtures.CliQuickOptimizationFlags);

            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);
            StringAssert.Contains(result.StandardOutput, OptimizationPhaseDisplay.ExhaustiveSearch);
        }

        [TestMethod]
        public void SurrogateSearch_PrintsSurrogateSearchTab()
        {
            CliRunResult result = CliProcessRunner.RunCommand("surrogate-search", IntegrationTestFixtures.CliQuickOptimizationFlags);

            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);
            StringAssert.Contains(result.StandardOutput, OptimizationPhaseDisplay.SurrogateSearch);
        }

        [TestMethod]
        public void DynamicProgrammingSearch_PrintsDynamicProgrammingSearchTab()
        {
            CliRunResult result = CliProcessRunner.RunCommand("dynamic-programming-search", IntegrationTestFixtures.CliQuickOptimizationFlags);

            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);
            StringAssert.Contains(result.StandardOutput, OptimizationPhaseDisplay.DynamicProgrammingSearch);
        }

        [TestMethod]
        public void Demo_RunsGeneticSearchWithTwentyGenerations()
        {
            CliRunResult result = CliProcessRunner.RunCommand(
                "demo",
                "--max-trucks 2 --max-loaders 2 --max-scalers 2 --generations 5");

            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);
            StringAssert.Contains(result.StandardOutput, OptimizationPhaseDisplay.GeneticSearch);
            StringAssert.Contains(result.StandardOutput, "generations=5");
        }

        [TestMethod]
        public void Aliases_MapToDesktopTabs()
        {
            Assert.AreEqual(0, CliProcessRunner.RunCommand("sim", IntegrationTestFixtures.CliQuickSimulationFlags).ExitCode);
            Assert.AreEqual(0, CliProcessRunner.RunCommand("ga", IntegrationTestFixtures.CliQuickOptimizationFlags).ExitCode);
            Assert.AreEqual(0, CliProcessRunner.RunCommand("phase1", IntegrationTestFixtures.CliQuickOptimizationFlags).ExitCode);
            Assert.AreEqual(0, CliProcessRunner.RunCommand("optimize", IntegrationTestFixtures.CliQuickOptimizationFlags).ExitCode);
            Assert.AreEqual(0, CliProcessRunner.RunCommand("dp", IntegrationTestFixtures.CliQuickOptimizationFlags).ExitCode);
        }

        [TestMethod]
        public void UnknownCommand_ReturnsError()
        {
            CliRunResult result = CliProcessRunner.Run("not-a-command");

            Assert.AreEqual(1, result.ExitCode);
            StringAssert.Contains(result.StandardError, "Unknown command");
        }

        [TestMethod]
        public void GeneticSearch_ReturnsError_ForInvalidGenerations()
        {
            CliRunResult result = CliProcessRunner.RunCommand("genetic-search", "--generations 0");

            Assert.AreEqual(1, result.ExitCode);
            StringAssert.Contains(result.StandardError, "generations must be between");
        }

        [TestMethod]
        public void Simulation_ReturnsError_ForNonPositiveTrucks()
        {
            CliRunResult result = CliProcessRunner.RunCommand("simulation", "--trucks 0");

            Assert.AreEqual(1, result.ExitCode);
            StringAssert.Contains(result.StandardError, "trucks, loaders, and scalers must be positive");
        }
    }
}
