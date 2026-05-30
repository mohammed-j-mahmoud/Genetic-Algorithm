using GeneticAlgorithm.Application;

namespace GeneticAlgorithm.Api;

internal static class ApiCatalog
{
    internal sealed class EndpointInfo
    {
        public string Method { get; init; }
        public string Path { get; init; }
        public string Tab { get; init; }
        public string Description { get; init; }
        public bool BrowserGetOk { get; init; }
    }

    internal static readonly EndpointInfo[] Endpoints =
    {
        new EndpointInfo
        {
            Method = "GET",
            Path = "/",
            Tab = "Docs",
            Description = "This help page (HTML).",
            BrowserGetOk = true
        },
        new EndpointInfo
        {
            Method = "GET",
            Path = "/health",
            Tab = "Health",
            Description = "Health check JSON.",
            BrowserGetOk = true
        },
        new EndpointInfo
        {
            Method = "GET",
            Path = "/api/endpoints",
            Tab = "Catalog",
            Description = "JSON list of all routes (method, path, tab).",
            BrowserGetOk = true
        },
        new EndpointInfo
        {
            Method = "GET",
            Path = "/api/sample/simulation-request",
            Tab = OptimizationPhaseDisplay.SimulationTab,
            Description = "Sample JSON body for POST /api/simulation.",
            BrowserGetOk = true
        },
        new EndpointInfo
        {
            Method = "GET",
            Path = "/api/sample/optimization-request",
            Tab = "Search tabs",
            Description = "Sample JSON for search POST routes (max bounds + economics — no fixed fleet counts).",
            BrowserGetOk = true
        },
        new EndpointInfo
        {
            Method = "POST",
            Path = "/api/simulation",
            Tab = OptimizationPhaseDisplay.SimulationTab,
            Description = "Run simulation (flat JSON — same shape as sample).",
            BrowserGetOk = false
        },
        new EndpointInfo
        {
            Method = "POST",
            Path = "/api/genetic-algorithm",
            Tab = OptimizationPhaseDisplay.GeneticAlgorithmTab,
            Description = "Genetic Algorithm tab (stochastic GA fitness).",
            BrowserGetOk = false
        },
        new EndpointInfo
        {
            Method = "POST",
            Path = "/api/exhaustive-search",
            Tab = OptimizationPhaseDisplay.ExhaustiveSearch,
            Description = "Exhaustive Search tab.",
            BrowserGetOk = false
        },
        new EndpointInfo
        {
            Method = "POST",
            Path = "/api/genetic-search",
            Tab = OptimizationPhaseDisplay.GeneticSearch,
            Description = "Genetic Search tab (expected-time GA + verify).",
            BrowserGetOk = false
        },
        new EndpointInfo
        {
            Method = "POST",
            Path = "/api/surrogate-search",
            Tab = OptimizationPhaseDisplay.SurrogateSearch,
            Description = "Surrogate Search tab.",
            BrowserGetOk = false
        },
        new EndpointInfo
        {
            Method = "POST",
            Path = "/api/dynamic-programming-search",
            Tab = OptimizationPhaseDisplay.DynamicProgrammingSearch,
            Description = "Dynamic Programming Search tab.",
            BrowserGetOk = false
        }
    };

    internal static object BuildJsonResponse() => new
    {
        application = OptimizationPhaseDisplay.ApplicationTitle,
        baseUrls = new
        {
            visualStudioHttps = "https://localhost:7190",
            visualStudioHttp = "http://localhost:5296",
            docker = "http://localhost:8080"
        },
        note = "POST routes require Postman or curl. Opening a POST path in the browser address bar will not work.",
        endpoints = Endpoints
    };
}
