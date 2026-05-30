using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.IntegrationTests
{
    [TestClass]
    public class ApiValidationIntegrationTests
    {
        [TestMethod]
        public async Task GeneticAlgorithm_ReturnsBadRequest_WhenSimulationMissing()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var request = IntegrationTestFixtures.CreateSmallOptimizationRequest();
            request.Simulation = null;

            var response = await client.PostAsJsonAsync("/api/genetic-algorithm", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            Assert.IsNotNull(body);
            StringAssert.Contains(body.Error, "simulation is required");
        }

        [TestMethod]
        public async Task GeneticSearch_ReturnsBadRequest_WhenMaxTrucksZero()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var request = IntegrationTestFixtures.CreateSmallOptimizationRequest();
            request.MaxTrucks = 0;

            var response = await client.PostAsJsonAsync("/api/genetic-search", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GeneticSearch_ReturnsBadRequest_WhenPopulationZero()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var request = IntegrationTestFixtures.CreateSmallOptimizationRequest();
            request.PopulationSize = 0;

            var response = await client.PostAsJsonAsync("/api/genetic-search", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GeneticSearch_ReturnsBadRequest_WhenMutationRateInvalid()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var request = IntegrationTestFixtures.CreateSmallOptimizationRequest();
            request.MutationRate = 1.5;

            var response = await client.PostAsJsonAsync("/api/genetic-search", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GeneticAlgorithm_ReturnsBadRequest_WhenGenerationsTooLarge()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var request = IntegrationTestFixtures.CreateSmallOptimizationRequest();
            request.Generations = SimulationParameters.MaxParameterValue + 1;

            var response = await client.PostAsJsonAsync("/api/genetic-algorithm", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            Assert.IsNotNull(body);
            StringAssert.Contains(body.Error, "generations must be between");
        }

        [TestMethod]
        public async Task ExhaustiveSearch_ReturnsBadRequest_WhenDistributionTooLarge()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var request = IntegrationTestFixtures.CreateSmallOptimizationRequest();
            request.Simulation.LoadingDistribution = IntegrationTestFixtures.CreateOversizedDistribution(101);

            var response = await client.PostAsJsonAsync("/api/exhaustive-search", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            Assert.IsNotNull(body);
            StringAssert.Contains(body.Error, "loadingDistribution");
        }

        [TestMethod]
        public async Task GeneticAlgorithm_ReturnsBadRequest_WhenTruckLoadVolumeMissing()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            var request = IntegrationTestFixtures.CreateSmallOptimizationRequest();
            request.Simulation.TruckLoadVolume = 0;

            var response = await client.PostAsJsonAsync("/api/genetic-algorithm", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            Assert.IsNotNull(body);
            Assert.AreEqual("truckLoadVolume must be positive.", body.Error);
        }

        [TestMethod]
        public async Task Simulation_ReturnsBadRequest_WhenTruckLoadVolumeMissing()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            SimulationRequest request = IntegrationTestFixtures.CreateSmallSimulation();
            request.TruckLoadVolume = 0;

            var response = await client.PostAsJsonAsync("/api/simulation", request);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            Assert.IsNotNull(body);
            Assert.AreEqual("truckLoadVolume must be positive.", body.Error);
        }

        [TestMethod]
        public async Task Simulation_ReturnsBadRequest_ForMalformedJson()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            using var content = new StringContent("{coalVolume: 40}", Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/simulation", content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GeneticAlgorithm_ReturnsBadRequest_ForMalformedJson()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            using var content = new StringContent("{maxTrucks: 6}", Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/genetic-algorithm", content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GeneticSearch_AcceptsCamelCasePropertyNames()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            const string json = "{\"generations\":3,\"populationSize\":4,\"maxTrucks\":2,\"maxLoaders\":2,\"maxScalers\":2,\"mutationRate\":0.01,\"simulation\":{\"coalVolume\":40,\"truckCount\":2,\"truckLoadVolume\":20,\"truckCostPerDay\":100,\"loaderCount\":1,\"loaderCostPerDay\":200,\"scalerCount\":1,\"scalerCostPerDay\":300,\"projectDurationDays\":30,\"delayCostPerDay\":50}}";
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/genetic-search", content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task ExhaustiveSearch_AcceptsFlatSimulationFieldsAtRoot()
        {
            using var factory = IntegrationTestFixtures.CreateApiFactory();
            using var client = factory.CreateClient();
            SimulationRequest simulation = DemoRequests.CreateSimulation();
            using var content = new StringContent(
                JsonSerializer.Serialize(simulation, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/exhaustive-search", content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            OptimizationRunResponse body = await response.Content.ReadFromJsonAsync<OptimizationRunResponse>();
            IntegrationTestFixtures.AssertOptimizationResponse(body, OptimizationPhaseDisplay.ExhaustiveSearch, "Phase1");
        }
    }
}
