using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<SimulationService>();
builder.Services.AddSingleton<GeneticOptimizationService>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.MapPost("/api/simulation", (SimulationRequest request, SimulationService service) =>
{
    var output = service.Run(request);
    return Results.Ok(output);
});

app.MapPost("/api/genetic-algorithm", (GeneticOptimizationRequest request, GeneticOptimizationService service) =>
{
    var result = service.Run(request);
    return Results.Ok(new
    {
        result.BestGeneration,
        trucks = result.BestChromosome.Genes[0],
        loaders = result.BestChromosome.Genes[1],
        scalers = result.BestChromosome.Genes[2],
        fitness = result.BestChromosome.Fitness,
        totalCost = result.BestChromosome.TotalCost
    });
});

app.Run();
