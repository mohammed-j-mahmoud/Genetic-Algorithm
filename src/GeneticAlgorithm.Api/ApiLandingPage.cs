using System.Net;
using System.Text;
using System.Text.Json;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;

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
        string minimumOptimizationJson = BuildMinimumOptimizationExample();
        string flatOptimizationJson = simulationJson;
        string simulationCurlHttps = BuildCurlExample("https://localhost:7190", "/api/simulation", simulationJson, useInsecureFlag: true);
        string simulationCurlHttp = BuildCurlExample("http://localhost:5296", "/api/simulation", simulationJson, useInsecureFlag: false);
        string optimizationCurlHttps = BuildCurlExample("https://localhost:7190", "/api/genetic-algorithm", minimumOptimizationJson, useInsecureFlag: true);
        string optimizationCurlHttp = BuildCurlExample("http://localhost:5296", "/api/exhaustive-search", flatOptimizationJson, useInsecureFlag: false);

        return $@"<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""utf-8"" />
  <title>{OptimizationPhaseDisplay.ApiTitle}</title>
  <style>
    body {{ font-family: system-ui, sans-serif; max-width: 56rem; margin: 2rem auto; padding: 0 1rem; line-height: 1.5; }}
    code {{ background: #f4f4f4; padding: 0.1rem 0.35rem; border-radius: 4px; }}
    pre {{ background: #f4f4f4; padding: 1rem; overflow-x: auto; border-radius: 6px; font-size: 0.9rem; }}
    h3 {{ margin-top: 1.5rem; }}
    table {{ border-collapse: collapse; width: 100%; font-size: 0.95rem; }}
    th, td {{ border: 1px solid #ddd; padding: 0.5rem 0.65rem; text-align: left; vertical-align: top; }}
    th {{ background: #f4f4f4; }}
    .method-get {{ color: #0b6; font-weight: 600; }}
    .method-post {{ color: #06c; font-weight: 600; }}
    .note {{ background: #fff8e6; border: 1px solid #f0d080; padding: 0.75rem 1rem; border-radius: 6px; }}
  </style>
</head>
<body>
  <h1>{OptimizationPhaseDisplay.ApiTitle}</h1>
  <p>{OptimizationPhaseDisplay.ApiDescription}</p>

  <div class=""note"">
    <strong>Base URLs</strong> (pick the one your API is running on):<br />
    Visual Studio HTTPS profile: <code>https://localhost:7190</code><br />
    Visual Studio HTTP profile: <code>http://localhost:5296</code><br />
    Docker Compose: <code>http://localhost:8080</code><br /><br />
    <strong>Browser vs Postman:</strong> GET routes below work in the browser. <em>POST</em> routes need Postman or curl — typing a POST path in the address bar will fail (404/405).
  </div>

  <h2>All endpoints</h2>
  <p>Machine-readable catalog: <code>GET /api/endpoints</code></p>
  {BuildEndpointTableHtml()}

  <h2>Quick start in Postman</h2>
  <ol>
    <li>Method <strong>GET</strong> → open <code>/api/sample/simulation-request</code> or <code>/api/sample/optimization-request</code> in the browser and copy the JSON.</li>
    <li>Method <strong>POST</strong> → paste into Postman body (raw JSON) and post to the matching route below.</li>
    <li>Include <code>truckLoadVolume</code> (demo default <code>20</code>) and other simulation fields.</li>
    <li>For search tabs, send simulation fields <strong>nested under</strong> <code>simulation</code> <em>or</em> <strong>flat at the root</strong> (same JSON as <code>/api/simulation</code>).</li>
  </ol>

  <h2>Example: Simulation (POST /api/simulation)</h2>
  <p>Flat JSON — copy into Postman:</p>
  <pre>{HtmlEncode(simulationJson)}</pre>
  <h3>curl — HTTPS profile</h3>
  <pre>{HtmlEncode(simulationCurlHttps)}</pre>
  <h3>curl — HTTP profile</h3>
  <pre>{HtmlEncode(simulationCurlHttp)}</pre>

  <h2>Example: Search tabs (POST)</h2>
  <p>Works for <code>/api/genetic-algorithm</code>, <code>/api/exhaustive-search</code>, <code>/api/genetic-search</code>, <code>/api/surrogate-search</code>, <code>/api/dynamic-programming-search</code></p>

  <h3>Option A — flat JSON (same as simulation sample)</h3>
  <p>Paste the simulation JSON above into Postman and POST to e.g. <code>/api/exhaustive-search</code>.</p>
  <pre>{HtmlEncode(flatOptimizationJson)}</pre>
  <h3>curl — flat body to exhaustive-search (HTTP)</h3>
  <pre>{HtmlEncode(optimizationCurlHttp)}</pre>

  <h3>Option B — nested JSON with search options</h3>
  <pre>{HtmlEncode(minimumOptimizationJson)}</pre>
  <h3>curl — nested body to genetic-algorithm (HTTPS)</h3>
  <pre>{HtmlEncode(optimizationCurlHttps)}</pre>

  <h3>Option C — full demo with distributions</h3>
  <pre>{HtmlEncode(optimizationJson)}</pre>
</body>
</html>";
    }

    private static string BuildEndpointTableHtml()
    {
        var builder = new StringBuilder();
        builder.AppendLine("<table>");
        builder.AppendLine("<tr><th>Method</th><th>Path</th><th>Desktop tab</th><th>Notes</th></tr>");

        foreach (ApiCatalog.EndpointInfo endpoint in ApiCatalog.Endpoints)
        {
            string methodClass = endpoint.Method == "GET" ? "method-get" : "method-post";
            string browserNote = endpoint.BrowserGetOk ? "Works in browser" : "Postman / curl only";
            builder.AppendLine(
                $"<tr><td class=\"{methodClass}\">{endpoint.Method}</td>" +
                $"<td><code>{HtmlEncode(endpoint.Path)}</code></td>" +
                $"<td>{HtmlEncode(endpoint.Tab)}</td>" +
                $"<td>{HtmlEncode(endpoint.Description)} — {browserNote}</td></tr>");
        }

        builder.AppendLine("</table>");
        return builder.ToString();
    }

    private static string BuildMinimumOptimizationExample()
    {
        SimulationRequest sim = DemoRequests.CreateSimulation();
        var request = new
        {
            generations = 20,
            populationSize = 20,
            maxTrucks = 6,
            maxLoaders = 2,
            maxScalers = 2,
            mutationRate = 0.01,
            simulation = new
            {
                coalVolume = sim.CoalVolume,
                truckLoadVolume = sim.TruckLoadVolume,
                truckCount = sim.TruckCount,
                loaderCount = sim.LoaderCount,
                scalerCount = sim.ScalerCount,
                truckCostPerDay = sim.TruckCostPerDay,
                loaderCostPerDay = sim.LoaderCostPerDay,
                scalerCostPerDay = sim.ScalerCostPerDay,
                projectDurationDays = sim.ProjectDurationDays,
                delayCostPerDay = sim.DelayCostPerDay
            }
        };

        return JsonSerializer.Serialize(request, PrettyJson);
    }

    private static string BuildCurlExample(string baseUrl, string path, string jsonBody, bool useInsecureFlag)
    {
        string compactJson = jsonBody
            .Replace("\r\n", string.Empty)
            .Replace("\n", string.Empty)
            .Replace("  ", string.Empty);
        string escapedJson = compactJson.Replace("\"", "\\\"");
        string insecure = useInsecureFlag ? "-k " : string.Empty;

        return $@"curl {insecure}-X POST {baseUrl}{path} ^
  -H ""Content-Type: application/json"" ^
  -d ""{escapedJson}""";
    }

    private static string HtmlEncode(string value) => WebUtility.HtmlEncode(value);
}
