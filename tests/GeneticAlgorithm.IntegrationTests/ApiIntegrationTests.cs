using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.IntegrationTests
{
    [TestClass]
    public class ApiIntegrationTests
    {
        private static WebApplicationFactory<GeneticAlgorithm.Api.Program> CreateFactory() =>
            new WebApplicationFactory<GeneticAlgorithm.Api.Program>();

        [TestMethod]
        public async Task Root_ReturnsOk()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var response = await client.GetAsync("/");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Health_ReturnsOk()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var response = await client.GetAsync("/health");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Simulation_ReturnsCost()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var request = new SimulationRequest
            {
                CoalVolume = 40,
                TruckCount = 2,
                TruckLoadVolume = 20,
                TruckCostPerDay = 100,
                LoaderCount = 1,
                LoaderCostPerDay = 200,
                ScalerCount = 1,
                ScalerCostPerDay = 300,
                ProjectDurationDays = 30,
                DelayCostPerDay = 50
            };

            var response = await client.PostAsJsonAsync("/api/simulation", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GeneticAlgorithm_ReturnsBestGenes()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var request = CreateSmallOptimizationRequest();

            var response = await client.PostAsJsonAsync("/api/genetic-algorithm", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            Assert.IsNotNull(body);
            Assert.AreEqual("Genetic Search", body.Strategy);
            Assert.IsTrue(body.Trucks >= 1);
        }

        [TestMethod]
        public async Task ExhaustiveSearch_ReturnsOk()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/exhaustive-search", CreateSmallOptimizationRequest());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            Assert.IsNotNull(body);
            Assert.AreEqual("Exhaustive Search", body.Strategy);
        }

        [TestMethod]
        public async Task GeneticSearch_ReturnsOk()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/genetic-search", CreateSmallOptimizationRequest());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            Assert.IsNotNull(body);
            Assert.AreEqual("Genetic Search", body.Strategy);
        }

        [TestMethod]
        public async Task SurrogateSearch_ReturnsOk()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/surrogate-search", CreateSmallOptimizationRequest());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            Assert.IsNotNull(body);
            Assert.AreEqual("Surrogate Search", body.Strategy);
        }

        [TestMethod]
        public async Task DynamicProgrammingSearch_ReturnsOk()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/dynamic-programming-search", CreateSmallOptimizationRequest());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            Assert.IsNotNull(body);
            Assert.AreEqual("Dynamic Programming Search", body.Strategy);
        }

        [TestMethod]
        public async Task Root_ContainsValidJsonExamples()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            string html = await client.GetStringAsync("/");
            StringAssert.Contains(html, "maxTrucks");
            StringAssert.Contains(html, "simulation");
            StringAssert.Contains(html, "/api/exhaustive-search");
        }

        [TestMethod]
        public async Task SampleOptimizationRequest_ReturnsOk()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var response = await client.GetAsync("/api/sample/optimization-request");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Optimization_ReturnsBadRequest_ForMalformedJson()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            using var content = new StringContent("{maxTrucks: 6}", Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/genetic-search", content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Optimization_ReturnsBadRequest_ForInvalidGenerations()
        {
            using var factory = CreateFactory();
            using var client = factory.CreateClient();
            var request = CreateSmallOptimizationRequest();
            request.Generations = 0;
            var response = await client.PostAsJsonAsync("/api/genetic-search", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static GeneticOptimizationRequest CreateSmallOptimizationRequest() =>
            new GeneticOptimizationRequest
            {
                Generations = 3,
                PopulationSize = 4,
                Simulation = new SimulationRequest
                {
                    CoalVolume = 40,
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
