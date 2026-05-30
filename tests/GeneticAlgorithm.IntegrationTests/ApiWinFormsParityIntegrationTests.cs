using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.IntegrationTests
{
    [TestClass]
    public class ApiWinFormsParityIntegrationTests
    {
        [TestMethod]
        public async Task Simulation_MatchesWinFormsReferenceRunner()
        {
            SimulationRequest request = IntegrationTestFixtures.CreateSmallSimulation();
            DumpTruckSimulation.SimulationOutput expected = WinFormsReferenceRunner.RunSimulation(request);

            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/simulation", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            DumpTruckSimulation.SimulationOutput actual =
                await response.Content.ReadFromJsonAsync<DumpTruckSimulation.SimulationOutput>();
            IntegrationTestFixtures.AssertSimulationMatches(expected, actual);
        }

        [TestMethod]
        public async Task ExhaustiveSearch_MatchesWinFormsPhase1OptimalFleet()
        {
            GeneticOptimizationRequest request = IntegrationTestFixtures.CreatePhaseGridRequest();
            OptimizationRunResult expected = WinFormsReferenceRunner.RunPhase(OptimizationPhase.Phase1, request);

            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/exhaustive-search", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            OptimizationRunResponse body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(body, OptimizationPhaseDisplay.ExhaustiveSearch, "Phase1");
            IntegrationTestFixtures.AssertFleetMatches(expected, body);
        }

        [TestMethod]
        public async Task DynamicProgrammingSearch_MatchesWinFormsPhase4AndPhase1()
        {
            GeneticOptimizationRequest request = IntegrationTestFixtures.CreatePhaseGridRequest();
            OptimizationRunResult phase1 = WinFormsReferenceRunner.RunPhase(OptimizationPhase.Phase1, request);
            OptimizationRunResult phase4 = WinFormsReferenceRunner.RunPhase(OptimizationPhase.Phase4, request);
            IntegrationTestFixtures.AssertFleetMatches(phase1, phase4);

            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/dynamic-programming-search", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            OptimizationRunResponse body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(
                body,
                OptimizationPhaseDisplay.DynamicProgrammingSearch,
                "Phase4");
            IntegrationTestFixtures.AssertFleetMatches(phase4, body);
        }

        [TestMethod]
        public async Task GeneticAlgorithm_UsesGeneticAlgorithmTab_NotGeneticSearch()
        {
            GeneticOptimizationRequest request = IntegrationTestFixtures.CreateSmallOptimizationRequest();

            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/genetic-algorithm", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            OptimizationRunResponse body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(
                body,
                OptimizationPhaseDisplay.GeneticAlgorithmTab,
                "GeneticAlgorithm");
            StringAssert.Contains(body.MethodSummary, "stochastic simulation fitness");
        }

        [TestMethod]
        public async Task GeneticSearch_UsesExpectedTimePipeline_NotGeneticAlgorithmTab()
        {
            GeneticOptimizationRequest request = IntegrationTestFixtures.CreateSmallOptimizationRequest();

            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/genetic-search", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            OptimizationRunResponse body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(body, OptimizationPhaseDisplay.GeneticSearch, "Phase2");
            StringAssert.Contains(body.MethodSummary, "expected-time");
        }

        [TestMethod]
        public async Task SurrogateSearch_MatchesWinFormsPhase3OptimalFleet()
        {
            GeneticOptimizationRequest request = IntegrationTestFixtures.CreatePhaseGridRequest();
            OptimizationRunResult expected = WinFormsReferenceRunner.RunPhase(OptimizationPhase.Phase3, request);

            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/surrogate-search", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            OptimizationRunResponse body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(body, OptimizationPhaseDisplay.SurrogateSearch, "Phase3");
            IntegrationTestFixtures.AssertFleetMatches(expected, body);
        }

        [TestMethod]
        public async Task SampleSimulationRequest_MatchesDemoRequests()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            SimulationRequest sample = await client.GetFromJsonAsync<SimulationRequest>("/api/sample/simulation-request");
            SimulationRequest expected = DemoRequests.CreateSimulation();

            Assert.AreEqual(expected.CoalVolume, sample.CoalVolume);
            Assert.AreEqual(expected.TruckCount, sample.TruckCount);
            Assert.AreEqual(expected.LoaderCount, sample.LoaderCount);
            Assert.AreEqual(expected.ProjectDurationDays, sample.ProjectDurationDays);
            Assert.AreEqual(expected.LoadingDistribution.Count, sample.LoadingDistribution.Count);
        }

        [TestMethod]
        public async Task SampleOptimizationRequest_MatchesDemoRequests()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            GeneticOptimizationRequest sample =
                await client.GetFromJsonAsync<GeneticOptimizationRequest>("/api/sample/optimization-request");
            GeneticOptimizationRequest expected = DemoRequests.CreateOptimization(20);

            Assert.AreEqual(expected.Generations, sample.Generations);
            Assert.AreEqual(expected.MaxTrucks, sample.MaxTrucks);
            Assert.AreEqual(expected.Simulation.CoalVolume, sample.Simulation.CoalVolume);
        }
    }
}
