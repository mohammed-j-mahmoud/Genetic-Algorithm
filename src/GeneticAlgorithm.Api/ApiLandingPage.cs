using System.Net;
using System.Text.Json;
using GeneticAlgorithm.Application;

namespace GeneticAlgorithm.Api;

internal static class ApiLandingPage
{
    private static readonly JsonSerializerOptions PrettyJson = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string BuildHtml()
    {
        string simulationJson = JsonSerializer.Serialize(DemoRequests.CreateSimulation(), PrettyJson);
        string optimizationJson = JsonSerializer.Serialize(DemoRequests.CreateOptimization(20), PrettyJson);
        string simulationCurl = BuildCurlExample("/api/simulation", simulationJson);
        string optimizationCurl = BuildCurlExample("/api/exhaustive-search", optimizationJson);

        return $@"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""utf-8"" />
  <title>{OptimizationPhaseDisplay.ApiTitle}</title>
  <style>
    body {{ font-family: system-ui, sans-serif; max-width: 52rem; margin: 2rem auto; padding: 0 1rem; line-height: 1.5; }}
    code {{ background: #f4f4f4; padding: 0.1rem 0.35rem; border-radius: 4px; }}
    pre {{ background: #f4f4f4; padding: 1rem; overflow-x: auto; border-radius: 6px; font-size: 0.9rem; }}
    h3 {{ margin-top: 1.5rem; }}
  </style>
</head>
<body>
  <h1>{OptimizationPhaseDisplay.ApiTitle}</h1>
  <p>{OptimizationPhaseDisplay.ApiDescription}</p>

  <h2>Endpoints</h2>
  <ul>
    <li><code>GET /health</code> — health check</li>
    <li><code>GET /api/sample/simulation-request</code> — sample JSON for Simulation</li>
    <li><code>GET /api/sample/optimization-request</code> — sample JSON for search tabs</li>
    <li><code>POST /api/simulation</code> — {OptimizationPhaseDisplay.SimulationTab} tab</li>
    <li><code>POST /api/genetic-algorithm</code> — {OptimizationPhaseDisplay.GeneticAlgorithmTab} tab</li>
    <li><code>POST /api/exhaustive-search</code> — {OptimizationPhaseDisplay.ExhaustiveSearch} tab</li>
    <li><code>POST /api/genetic-search</code> — {OptimizationPhaseDisplay.GeneticSearch} tab</li>
    <li><code>POST /api/surrogate-search</code> — {OptimizationPhaseDisplay.SurrogateSearch} tab</li>
    <li><code>POST /api/dynamic-programming-search</code> — {OptimizationPhaseDisplay.DynamicProgrammingSearch} tab</li>
  </ul>

  <p><strong>Postman:</strong> set Body to <em>raw</em> → <em>JSON</em>. Property names must use double quotes.</p>
  <p>Invalid: <code>{{maxTrucks: 6}}</code> &nbsp;|&nbsp; Valid: <code>{{""maxTrucks"": 6}}</code></p>

  <h2>Example: Simulation</h2>
  <p>POST <code>/api/simulation</code></p>
  <h3>JSON body (copy into Postman)</h3>
  <pre>{HtmlEncode(simulationJson)}</pre>
  <h3>curl (Windows)</h3>
  <pre>{HtmlEncode(simulationCurl)}</pre>

  <h2>Example: Exhaustive Search</h2>
  <p>POST <code>/api/exhaustive-search</code> — same JSON works for genetic-search, surrogate-search, and dynamic-programming-search.</p>
  <h3>JSON body (copy into Postman)</h3>
  <pre>{HtmlEncode(optimizationJson)}</pre>
  <h3>curl (Windows)</h3>
  <pre>{HtmlEncode(optimizationCurl)}</pre>
</body>
</html>";
    }

    private static string BuildCurlExample(string path, string jsonBody)
    {
        string compactJson = jsonBody
            .Replace("\r\n", string.Empty)
            .Replace("\n", string.Empty)
            .Replace("  ", string.Empty);
        string escapedJson = compactJson.Replace("\"", "\\\"");

        return $@"curl -k -X POST https://localhost:7190{path} ^
  -H ""Content-Type: application/json"" ^
  -d ""{escapedJson}""";
    }

    private static string HtmlEncode(string value) => WebUtility.HtmlEncode(value);
}
