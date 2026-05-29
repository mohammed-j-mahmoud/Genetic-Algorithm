using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Json;
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
            var request = new GeneticOptimizationRequest
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

            var response = await client.PostAsJsonAsync("/api/genetic-algorithm", request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
