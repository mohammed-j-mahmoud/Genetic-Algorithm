using System.Net.Http.Json;
using System.Threading.Tasks;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.IntegrationTests
{
    [TestClass]
    public class CliWinFormsParityIntegrationTests
    {
        [TestMethod]
        public void Simulation_MatchesWinFormsReferenceRunner()
        {
            SimulationRequest request = IntegrationTestFixtures.CreateSmallSimulation();
            DumpTruckSimulation.SimulationOutput expected = WinFormsReferenceRunner.RunSimulation(request);

            CliRunResult result = CliProcessRunner.RunCommand("simulation", IntegrationTestFixtures.CliQuickSimulationFlags);
            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);

            double totalCost = IntegrationTestFixtures.ParseCliDouble(result.StandardOutput, "Total Cost:");
            double totalDays = IntegrationTestFixtures.ParseCliDouble(result.StandardOutput, "Total Days:");
            Assert.AreEqual(expected.TotalCost, totalCost, 0.01);
            Assert.AreEqual(expected.TotalDays, totalDays, 0.01);
        }

        [TestMethod]
        public void ExhaustiveSearch_MatchesWinFormsPhase1OptimalFleet()
        {
            GeneticOptimizationRequest request = IntegrationTestFixtures.CreatePhaseGridRequest();
            OptimizationRunResult expected = WinFormsReferenceRunner.RunPhase(OptimizationPhase.Phase1, request);

            CliRunResult result = CliProcessRunner.RunCommand("exhaustive-search", IntegrationTestFixtures.CliPhaseGridFlags);
            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);

            Assert.AreEqual(
                expected.BestChromosome.Genes[IntegrationTestFixtures.TrucksGeneIndex],
                IntegrationTestFixtures.ParseCliInt(result.StandardOutput, "Number of Trucks:"));
            Assert.AreEqual(
                expected.BestChromosome.Genes[IntegrationTestFixtures.LoadersGeneIndex],
                IntegrationTestFixtures.ParseCliInt(result.StandardOutput, "Number of Loaders:"));
            Assert.AreEqual(
                expected.BestChromosome.Genes[IntegrationTestFixtures.ScalersGeneIndex],
                IntegrationTestFixtures.ParseCliInt(result.StandardOutput, "Number of Scalers:"));
        }

        [TestMethod]
        public void DynamicProgrammingSearch_MatchesWinFormsPhase4OptimalFleet()
        {
            GeneticOptimizationRequest request = IntegrationTestFixtures.CreatePhaseGridRequest();
            OptimizationRunResult expected = WinFormsReferenceRunner.RunPhase(OptimizationPhase.Phase4, request);

            CliRunResult result = CliProcessRunner.RunCommand("dynamic-programming-search", IntegrationTestFixtures.CliPhaseGridFlags);
            Assert.AreEqual(0, result.ExitCode, result.CombinedOutput);

            Assert.AreEqual(
                expected.BestChromosome.Genes[IntegrationTestFixtures.TrucksGeneIndex],
                IntegrationTestFixtures.ParseCliInt(result.StandardOutput, "Number of Trucks:"));
            Assert.AreEqual(
                expected.BestChromosome.Genes[IntegrationTestFixtures.LoadersGeneIndex],
                IntegrationTestFixtures.ParseCliInt(result.StandardOutput, "Number of Loaders:"));
            Assert.AreEqual(
                expected.BestChromosome.Genes[IntegrationTestFixtures.ScalersGeneIndex],
                IntegrationTestFixtures.ParseCliInt(result.StandardOutput, "Number of Scalers:"));
        }

        [TestMethod]
        public async Task ApiAndCli_Simulation_MatchWinFormsReferenceRunner()
        {
            SimulationRequest request = IntegrationTestFixtures.CreateSmallSimulation();
            DumpTruckSimulation.SimulationOutput expected = WinFormsReferenceRunner.RunSimulation(request);

            CliRunResult cli = CliProcessRunner.RunCommand("simulation", IntegrationTestFixtures.CliQuickSimulationFlags);
            Assert.AreEqual(0, cli.ExitCode, cli.CombinedOutput);

            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/simulation", request);
            DumpTruckSimulation.SimulationOutput api =
                await response.Content.ReadFromJsonAsync<DumpTruckSimulation.SimulationOutput>();

            double cliCost = IntegrationTestFixtures.ParseCliDouble(cli.StandardOutput, "Total Cost:");
            Assert.AreEqual(expected.TotalCost, cliCost, 0.01);
            IntegrationTestFixtures.AssertSimulationMatches(expected, api);
        }

        [TestMethod]
        public async Task ApiAndCli_ExhaustiveSearch_MatchWinFormsPhase1Fleet()
        {
            GeneticOptimizationRequest request = IntegrationTestFixtures.CreatePhaseGridRequest();
            OptimizationRunResult expected = WinFormsReferenceRunner.RunPhase(OptimizationPhase.Phase1, request);

            CliRunResult cli = CliProcessRunner.RunCommand("exhaustive-search", IntegrationTestFixtures.CliPhaseGridFlags);
            Assert.AreEqual(0, cli.ExitCode, cli.CombinedOutput);

            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/exhaustive-search", request);
            OptimizationRunResponse api = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();

            int cliTrucks = IntegrationTestFixtures.ParseCliInt(cli.StandardOutput, "Number of Trucks:");
            Assert.AreEqual(expected.BestChromosome.Genes[IntegrationTestFixtures.TrucksGeneIndex], cliTrucks);
            IntegrationTestFixtures.AssertFleetMatches(expected, api);
        }
    }
}
