using System;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;

namespace GeneticAlgorithm.Cli
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length > 0)
                return ExecuteCommand(args, interactiveShell: false);

            return RunInteractiveShell();
        }

        private static int RunInteractiveShell()
        {
            Console.WriteLine($"{OptimizationPhaseDisplay.ApplicationTitle} (interactive CLI)");
            Console.WriteLine("Same tabs as the desktop app and HTTP API. Type 'help' for all commands.");
            Console.WriteLine($"At {OptimizationPhaseDisplay.CliPrompt}type commands directly — no \"{CliCatalog.ExecutableName}\" prefix.");
            Console.WriteLine("Type 'exit' or press Ctrl+C to quit.");
            Console.WriteLine();

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                Console.WriteLine();
                Environment.Exit(0);
            };

            while (true)
            {
                Console.Write(OptimizationPhaseDisplay.CliPrompt);
                string line = Console.ReadLine();
                if (line == null)
                    return 0;

                line = line.Trim();
                if (line.Length == 0)
                    continue;

                if (IsExitCommand(line))
                    return 0;

                string[] commandArgs = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                ExecuteCommand(commandArgs, interactiveShell: true);
                Console.WriteLine();
            }
        }

        private static bool IsExitCommand(string line)
        {
            switch (line.ToLowerInvariant())
            {
                case "exit":
                case "quit":
                case "q":
                    return true;
                default:
                    return false;
            }
        }

        private static int ExecuteCommand(string[] args, bool interactiveShell)
        {
            try
            {
                if (!CliCatalog.TryResolve(args[0], out string command, out CliCommandInfo commandInfo))
                {
                    Console.Error.WriteLine($"Unknown command '{args[0]}'. Type 'help' for available commands.");
                    return 1;
                }

                if (commandInfo.InteractiveOnly && !interactiveShell)
                {
                    Console.Error.WriteLine($"'{command}' is only available in the interactive shell.");
                    return 1;
                }

                switch (command)
                {
                    case "help":
                        CliHelp.Print();
                        return 0;
                    case "simulation":
                        return RunSimulation(ParseOptions(args));
                    case "genetic-algorithm":
                        return RunGeneticAlgorithm(ParseOptions(args));
                    case "exhaustive-search":
                        return RunOptimizationPhase(OptimizationPhase.Phase1, ParseOptions(args), defaultGenerations: 1);
                    case "genetic-search":
                        return RunOptimizationPhase(OptimizationPhase.Phase2, ParseOptions(args), defaultGenerations: 50);
                    case "surrogate-search":
                        return RunOptimizationPhase(OptimizationPhase.Phase3, ParseOptions(args), defaultGenerations: 1);
                    case "dynamic-programming-search":
                        return RunOptimizationPhase(OptimizationPhase.Phase4, ParseOptions(args), defaultGenerations: 1);
                    case "demo":
                        return RunOptimizationPhase(OptimizationPhase.Phase2, ParseOptions(args), defaultGenerations: 20);
                    case "clear":
                        Console.Clear();
                        return 0;
                    default:
                        Console.Error.WriteLine($"Unknown command '{args[0]}'. Type 'help' for available commands.");
                        return 1;
                }
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine("Type 'help' for command-line options.");
                return 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 2;
            }
        }

        private static CliRunOptions ParseOptions(string[] args)
        {
            var options = new CliRunOptions();
            if (args.Length > 1)
                options.Apply(CliArgumentParser.Parse(args));
            return options;
        }

        private static int RunSimulation(CliRunOptions options)
        {
            Console.WriteLine($"{OptimizationPhaseDisplay.SimulationTab} — {options.DescribeInputs()}");
            SimulationRequest request = options.BuildSimulationRequest();
            var result = new SimulationService().Run(request);
            Console.WriteLine($"Total Cost: {result.TotalCost:F2}");
            Console.WriteLine($"Total Days: {result.TotalDays:F2}");
            Console.WriteLine($"Days Delayed: {result.DelayDays:F2}");
            Console.WriteLine($"Utilization of Trucks: {result.TruckUtilization:P1}");
            Console.WriteLine($"Utilization of Loaders: {result.LoaderUtilization:P1}");
            Console.WriteLine($"Utilization of Scalers: {result.ScalerUtilization:P1}");
            return 0;
        }

        private static int RunGeneticAlgorithm(CliRunOptions options)
        {
            GeneticOptimizationRequest request = options.BuildOptimizationRequest(defaultGenerations: 50);
            string error = PhaseOptimizationService.TryValidateRequest(request);
            if (error != null)
                throw new ArgumentException(error);

            Console.WriteLine($"{OptimizationPhaseDisplay.GeneticAlgorithmTab} — {options.DescribeInputs()}");

            int generations = request.Generations;
            int lastReportedGeneration = 0;
            var progress = new Progress<GenerationProgress>(report =>
            {
                if (report.Generation <= lastReportedGeneration)
                    return;

                lastReportedGeneration = report.Generation;
                Console.WriteLine($"Generation {report.Generation}/{generations}");
            });

            OptimizationRunResult result = new GeneticOptimizationService().Run(request, progress);
            PrintOptimizationResult(result, geneticAlgorithmTab: true);
            return 0;
        }

        private static int RunOptimizationPhase(
            OptimizationPhase phase,
            CliRunOptions options,
            int defaultGenerations)
        {
            GeneticOptimizationRequest request = options.BuildOptimizationRequest(defaultGenerations);
            var service = new PhaseOptimizationService();
            string strategy = OptimizationPhaseDisplay.GetStrategyName(phase);
            Console.WriteLine($"{strategy} — {options.DescribeInputs()}");

            OptimizationRunResult result = phase switch
            {
                OptimizationPhase.Phase1 => service.RunPhase1(request, CreatePercentProgress()),
                OptimizationPhase.Phase2 => service.RunPhase2(request, CreateGeneticProgress(request.Generations)),
                OptimizationPhase.Phase3 => service.RunPhase3(request, CreatePercentProgress()),
                OptimizationPhase.Phase4 => service.RunPhase4(request, CreatePercentProgress()),
                _ => throw new InvalidOperationException($"Unsupported phase: {phase}")
            };

            PrintOptimizationResult(result);
            return 0;
        }

        private static IProgress<int> CreatePercentProgress()
        {
            int lastReportedPercent = -1;
            return new Progress<int>(percent =>
            {
                if (percent / 10 <= lastReportedPercent / 10)
                    return;

                lastReportedPercent = percent;
                Console.WriteLine($"Progress: {percent}%");
            });
        }

        private static IProgress<int> CreateGeneticProgress(int generations)
        {
            int lastReportedGeneration = 0;
            return new Progress<int>(percent =>
            {
                int generation = Math.Max(1, percent * generations / 100);
                if (generation <= lastReportedGeneration)
                    return;

                lastReportedGeneration = generation;
                Console.WriteLine($"Generation {generation}/{generations}");
            });
        }

        private static void PrintOptimizationResult(OptimizationRunResult result, bool geneticAlgorithmTab = false)
        {
            OptimizationRunResponse response = geneticAlgorithmTab
                ? OptimizationRunResponse.FromGeneticAlgorithm(result)
                : OptimizationRunResponse.FromResult(result);
            Console.WriteLine();
            Console.WriteLine($"Tab: {response.Strategy}");
            Console.WriteLine($"Summary: {response.MethodSummary}");
            if (response.StoppedEarly)
                Console.WriteLine("Stopped early: yes");

            Console.WriteLine($"Best generation: {response.BestGeneration}");
            Console.WriteLine($"Number of Trucks: {response.Trucks}");
            Console.WriteLine($"Number of Loaders: {response.Loaders}");
            Console.WriteLine($"Number of Scalers: {response.Scalers}");
            Console.WriteLine($"Fitness: {response.Fitness:F4}");
            Console.WriteLine($"Total Cost: {response.TotalCost:F2}");
            Console.WriteLine($"Total Days: {response.TotalDays:F0}");
            Console.WriteLine($"Combinations evaluated: {response.CombinationsEvaluated}");
            Console.WriteLine($"Simulation calls: {response.SimulationCalls}");
            Console.WriteLine($"Cache hits: {response.CacheHits}");
        }
    }
}
