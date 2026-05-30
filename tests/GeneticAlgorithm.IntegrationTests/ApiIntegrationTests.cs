using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.IntegrationTests
{
    [TestClass]
    public class ApiIntegrationTests
    {
        [TestMethod]
        public async Task Root_ReturnsOk()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.GetAsync("/");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Health_ReturnsHealthyApplication()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.GetAsync("/health");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            HealthResponse body = await response.Content.ReadFromJsonAsync<HealthResponse>();
            Assert.AreEqual("healthy", body.Status);
            Assert.AreEqual(OptimizationPhaseDisplay.ApplicationTitle, body.Application);
        }

        [TestMethod]
        public async Task Simulation_ReturnsPositiveMetrics()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            SimulationRequest request = IntegrationTestFixtures.CreateSmallSimulation();

            var response = await client.PostAsJsonAsync("/api/simulation", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            DumpTruckSimulation.SimulationOutput body =
                await response.Content.ReadFromJsonAsync<DumpTruckSimulation.SimulationOutput>();
            Assert.IsTrue(body.TotalCost > 0);
            Assert.IsTrue(body.TotalDays > 0);
        }

        [TestMethod]
        public async Task GeneticAlgorithm_ReturnsBestGenes()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var request = IntegrationTestFixtures.CreateSmallOptimizationRequest();

            var response = await client.PostAsJsonAsync("/api/genetic-algorithm", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(
                body,
                OptimizationPhaseDisplay.GeneticAlgorithmTab,
                "GeneticAlgorithm");
        }

        [TestMethod]
        public async Task ExhaustiveSearch_ReturnsOk()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/exhaustive-search", IntegrationTestFixtures.CreateSmallOptimizationRequest());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(body, OptimizationPhaseDisplay.ExhaustiveSearch, "Phase1");
        }

        [TestMethod]
        public async Task GeneticSearch_ReturnsOk()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/genetic-search", IntegrationTestFixtures.CreateSmallOptimizationRequest());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(body, OptimizationPhaseDisplay.GeneticSearch, "Phase2");
        }

        [TestMethod]
        public async Task SurrogateSearch_ReturnsOk()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/surrogate-search", IntegrationTestFixtures.CreateSmallOptimizationRequest());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(body, OptimizationPhaseDisplay.SurrogateSearch, "Phase3");
        }

        [TestMethod]
        public async Task DynamicProgrammingSearch_ReturnsOk()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/dynamic-programming-search", IntegrationTestFixtures.CreateSmallOptimizationRequest());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(body, OptimizationPhaseDisplay.DynamicProgrammingSearch, "Phase4");
        }

        [TestMethod]
        public async Task Root_ContainsValidJsonExamples()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            string html = await client.GetStringAsync("/");
            StringAssert.Contains(html, "maxTrucks");
            StringAssert.Contains(html, "truckLoadVolume");
            StringAssert.Contains(html, "simulation");
            StringAssert.Contains(html, "do not take a fixed");
            StringAssert.Contains(html, "/api/genetic-algorithm");
            StringAssert.Contains(html, "/api/exhaustive-search");
            StringAssert.Contains(html, "/api/endpoints");
            StringAssert.Contains(html, "Postman / curl only");
        }

        [TestMethod]
        public async Task EndpointsCatalog_ReturnsAllRoutes()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.GetAsync("/api/endpoints");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            string json = await response.Content.ReadAsStringAsync();
            StringAssert.Contains(json, "/api/simulation");
            StringAssert.Contains(json, "/api/genetic-algorithm");
            StringAssert.Contains(json, "/api/exhaustive-search");
            StringAssert.Contains(json, "/api/dynamic-programming-search");
            StringAssert.Contains(json, "https://localhost:7190");
        }

        [TestMethod]
        public async Task SampleSimulationRequest_ReturnsOk()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.GetAsync("/api/sample/simulation-request");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task SampleOptimizationRequest_ReturnsOk()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var response = await client.GetAsync("/api/sample/optimization-request");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task SampleOptimizationRequest_DoesNotIncludeFixedFleetCounts()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            GeneticOptimizationRequest sample =
                await client.GetFromJsonAsync<GeneticOptimizationRequest>("/api/sample/optimization-request");

            Assert.IsNotNull(sample.Simulation);
            Assert.AreEqual(0f, sample.Simulation.TruckCount);
            Assert.AreEqual(0f, sample.Simulation.LoaderCount);
            Assert.AreEqual(0f, sample.Simulation.ScalerCount);
            Assert.IsTrue(sample.MaxTrucks > 0);
        }

        [TestMethod]
        public async Task Optimization_ReturnsBadRequest_ForMalformedJson()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            using var content = new StringContent("{maxTrucks: 6}", Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/genetic-search", content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Optimization_ReturnsBadRequest_ForInvalidGenerations()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var request = IntegrationTestFixtures.CreateSmallOptimizationRequest();
            request.Generations = 0;
            var response = await client.PostAsJsonAsync("/api/genetic-search", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
