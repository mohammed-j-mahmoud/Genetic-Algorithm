using System;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;

namespace GeneticAlgorithm.Cli
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                return 1;
            }

            try
            {
                switch (args[0].ToLowerInvariant())
                {
                    case "sim":
                    case "simulate":
                        return RunSimulation();
                    case "ga":
                    case "optimize":
                        return RunGeneticAlgorithm(args);
                    case "demo":
                        return RunDemo();
                    default:
                        PrintHelp();
                        return 1;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 2;
            }
        }

        private static int RunSimulation()
        {
            var request = CreateDemoSimulationRequest();
            var result = new SimulationService().Run(request);
            Console.WriteLine($"Total cost: {result.TotalCost:F2}");
            Console.WriteLine($"Total days: {result.TotalDays:F2}");
            Console.WriteLine($"Delay days: {result.DelayDays:F2}");
            Console.WriteLine($"Truck utilization: {result.TruckUtilization:P1}");
            return 0;
        }

        private static int RunGeneticAlgorithm(string[] args)
        {
            int generations = 50;
            if (args.Length > 1 && !int.TryParse(args[1], out generations))
                generations = 50;

            var request = new GeneticOptimizationRequest
            {
                Generations = generations,
                Simulation = CreateDemoSimulationRequest()
            };

            var service = new GeneticOptimizationService();
            var result = service.Run(request, new Progress<GenerationProgress>(p =>
                Console.WriteLine($"Generation {p.Generation}: fitness {p.BestFitness:F4}")));

            Console.WriteLine();
            Console.WriteLine($"Best generation: {result.BestGeneration}");
            Console.WriteLine($"Best trucks: {result.BestChromosome.Genes[0]}");
            Console.WriteLine($"Best loaders: {result.BestChromosome.Genes[1]}");
            Console.WriteLine($"Best scalers: {result.BestChromosome.Genes[2]}");
            Console.WriteLine($"Best fitness: {result.BestChromosome.Fitness:F4}");
            Console.WriteLine($"Total cost: {result.BestChromosome.TotalCost:F2}");
            return 0;
        }

        private static int RunDemo() => RunGeneticAlgorithm(new[] { "ga", "20" });

        private static SimulationRequest CreateDemoSimulationRequest() =>
            new SimulationRequest
            {
                CoalVolume = 10000,
                TruckCount = 6,
                TruckLoadVolume = 20,
                TruckCostPerDay = 1000,
                LoaderCount = 2,
                LoaderCostPerDay = 2000,
                ScalerCount = 2,
                ScalerCostPerDay = 3000,
                ProjectDurationDays = 120,
                DelayCostPerDay = 10000
            };

        private static void PrintHelp()
        {
            Console.WriteLine("Genetic Algorithm — dump truck optimization (cross-platform CLI)");
            Console.WriteLine();
            Console.WriteLine("  genetic-algorithm sim              Run one simulation with demo inputs");
            Console.WriteLine("  genetic-algorithm ga [generations] Run genetic algorithm (default 50)");
            Console.WriteLine("  genetic-algorithm demo             Shortcut for ga 20");
        }
    }
}
