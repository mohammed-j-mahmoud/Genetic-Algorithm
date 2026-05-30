using GeneticAlgorithm.Api;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddSingleton<SimulationService>();
builder.Services.AddSingleton<PhaseOptimizationService>();

var app = builder.Build();
app.UseFriendlyJsonErrors();

app.MapGet("/", () => Results.Content(ApiLandingPage.BuildHtml(), "text/html"));

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    application = OptimizationPhaseDisplay.ApplicationTitle
}));

app.MapGet("/api/sample/simulation-request", () =>
    Results.Ok(DemoRequests.CreateSimulation()));

app.MapGet("/api/sample/optimization-request", () =>
    Results.Ok(DemoRequests.CreateOptimization(20)));

app.MapPost("/api/simulation", (SimulationRequest request, SimulationService service) =>
{
    var output = service.Run(request);
    return Results.Ok(output);
});

app.MapPost("/api/exhaustive-search", (GeneticOptimizationRequest request, PhaseOptimizationService service) =>
    RunOptimization(request, service, s => s.RunPhase1(request)));

app.MapPost("/api/genetic-search", (GeneticOptimizationRequest request, PhaseOptimizationService service) =>
    RunOptimization(request, service, s => s.RunPhase2(request)));

app.MapPost("/api/surrogate-search", (GeneticOptimizationRequest request, PhaseOptimizationService service) =>
    RunOptimization(request, service, s => s.RunPhase3(request)));

app.MapPost("/api/dynamic-programming-search", (GeneticOptimizationRequest request, PhaseOptimizationService service) =>
    RunOptimization(request, service, s => s.RunPhase4(request)));

app.MapPost("/api/genetic-algorithm", (GeneticOptimizationRequest request, PhaseOptimizationService service) =>
    RunOptimization(request, service, s => s.RunPhase2(request)));

app.Run();

static IResult RunOptimization(
    GeneticOptimizationRequest request,
    PhaseOptimizationService service,
    Func<PhaseOptimizationService, OptimizationRunResult> run)
{
    string error = PhaseOptimizationService.TryValidateRequest(request);
    if (error != null)
        return Results.BadRequest(new { error });

    return Results.Ok(OptimizationRunResponse.FromResult(run(service)));
}
