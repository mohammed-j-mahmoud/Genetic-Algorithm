using System.Globalization;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Cli
{
    internal static class CliHelp
    {
        public static void Print()
        {
            string max = SimulationParameters.MaxParameterValue.ToString("N0");
            string maxFleet = SimulationParameters.MaxResourceCount.ToString("N0");

            // Pull live defaults so help text never silently drifts from DemoRequests.
            SimulationRequest sim = DemoRequests.CreateSimulation();
            GeneticOptimizationRequest opt = DemoRequests.CreateOptimization(20);

            CliOutput.WriteTitle($"{OptimizationPhaseDisplay.ApplicationTitle} — command-line help");
            CliOutput.WriteLine("Matches the desktop app tabs: Genetic Algorithm, Simulation, Exhaustive Search,");
            CliOutput.WriteLine("Genetic Search, Surrogate Search, and Dynamic Programming Search.");
            CliOutput.WriteBlankLine();
            CliOutput.WriteLine("MODES");
            CliOutput.WriteLine("  Interactive shell   Run with no arguments; type commands at the prompt.");
            CliOutput.WriteLine("  One-shot command    Run one command and exit (good for scripts).");
            CliOutput.WriteBlankLine();
            CliOutput.WriteLine("COMMANDS (desktop tab names)");
            CliOutput.WriteLine($"  genetic-algorithm              {OptimizationPhaseDisplay.GeneticAlgorithmTab}");
            CliOutput.WriteLine($"  simulation                     {OptimizationPhaseDisplay.SimulationTab}");
            CliOutput.WriteLine($"  exhaustive-search              {OptimizationPhaseDisplay.ExhaustiveSearch}");
            CliOutput.WriteLine($"  genetic-search                 {OptimizationPhaseDisplay.GeneticSearch}");
            CliOutput.WriteLine($"  surrogate-search               {OptimizationPhaseDisplay.SurrogateSearch}");
            CliOutput.WriteLine($"  dynamic-programming-search     {OptimizationPhaseDisplay.DynamicProgrammingSearch}");
            CliOutput.WriteLine($"  demo                           {OptimizationPhaseDisplay.RunDemoLabel} (Genetic Search, 20 generations)");
            CliOutput.WriteLine("  help                           Show this help");
            CliOutput.WriteLine("  clear                          Clear the screen (interactive mode only)");
            CliOutput.WriteLine("  exit                           Quit interactive shell (or Ctrl+C)");
            CliOutput.WriteBlankLine();
            CliOutput.WriteLine("SHORT ALIASES");
            CliOutput.WriteLine("  ga, genetic, genetic-algo      → genetic-algorithm");
            CliOutput.WriteLine("  sim, simulate                → simulation");
            CliOutput.WriteLine("  exhaustive, phase1           → exhaustive-search");
            CliOutput.WriteLine("  optimize, phase2             → genetic-search");
            CliOutput.WriteLine("  surrogate, phase3            → surrogate-search");
            CliOutput.WriteLine("  dp, phase4                   → dynamic-programming-search");
            CliOutput.WriteBlankLine();
            CliOutput.WriteLine("SIMULATION OPTIONS (simulation command, or nested under search commands)");
            CliOutput.WriteLine($"  --coal, -c <float>           Amount of material (default demo: {Fmt(sim.CoalVolume)})");
            CliOutput.WriteLine("  --trucks, -t <float>         Number of trucks for the simulation run");
            CliOutput.WriteLine("  --loaders, -l <float>        Number of loaders");
            CliOutput.WriteLine("  --scalers, -s <float>        Number of scalers");
            CliOutput.WriteLine($"  --load-per-truck <float>     Load per truck (default demo: {Fmt(sim.TruckLoadVolume)})");
            CliOutput.WriteLine($"  --truck-cost <float>         Cost per truck per day (default demo: {Fmt(sim.TruckCostPerDay)})");
            CliOutput.WriteLine($"  --loader-cost <float>        Cost per loader per day (default demo: {Fmt(sim.LoaderCostPerDay)})");
            CliOutput.WriteLine($"  --scaler-cost <float>        Cost per scaler per day (default demo: {Fmt(sim.ScalerCostPerDay)})");
            CliOutput.WriteLine($"  --project-days <float>       Project duration in days (default demo: {Fmt(sim.ProjectDurationDays)})");
            CliOutput.WriteLine($"  --delay-cost <float>         Cost of delay per day (default demo: {Fmt(sim.DelayCostPerDay)})");
            CliOutput.WriteLine("  Distributions (loading / weighing / traveling) use desktop demo values unless");
            CliOutput.WriteLine("  you change them through the API or desktop Distribution tab.");
            CliOutput.WriteBlankLine();
            CliOutput.WriteLine("SEARCH OPTIONS (genetic-algorithm / exhaustive / genetic / surrogate / dp commands)");
            CliOutput.WriteLine($"  --max-trucks <int>           Max trucks gene bound (default demo: {opt.MaxTrucks})");
            CliOutput.WriteLine($"  --max-loaders <int>          Max loaders gene bound (default demo: {opt.MaxLoaders})");
            CliOutput.WriteLine($"  --max-scalers <int>          Max scalers gene bound (default demo: {opt.MaxScalers})");
            CliOutput.WriteLine($"  --generations, -g <int>      Generations for Genetic Search (default: 50, demo: {opt.Generations})");
            CliOutput.WriteLine($"  --population, -p <int>       GA population size (default demo: {opt.PopulationSize})");
            CliOutput.WriteLine($"  --mutation-rate, -m <0-1>   GA mutation rate (default demo: {Fmt(opt.MutationRate)})");
            CliOutput.WriteLine($"  Limits: generations/population/fleet bounds ≤ {max}; fleet counts ≤ {maxFleet}.");
            CliOutput.WriteBlankLine();
            CliOutput.WriteLine("EXAMPLES (at the truck-fleet> prompt — type as shown)");
            CliOutput.WriteLine("  help");
            CliOutput.WriteLine("  simulation");
            CliOutput.WriteLine("  simulation --coal 5000 --trucks 4 --loaders 2 --scalers 1");
            CliOutput.WriteLine("  exhaustive-search --max-trucks 6 --max-loaders 2 --max-scalers 2");
            CliOutput.WriteLine("  genetic-algorithm --generations 30 --max-trucks 8 --coal 10000");
            CliOutput.WriteLine("  genetic-search --generations 30 --max-trucks 8 --coal 10000");
            CliOutput.WriteLine("  ga -g 20 -p 40 --max-trucks 10 --mutation-rate 0.02");
            CliOutput.WriteLine("  surrogate-search --max-trucks 6 --max-loaders 2 --max-scalers 2");
            CliOutput.WriteLine("  dp --max-trucks 6 --max-loaders 2 --max-scalers 2");
            CliOutput.WriteLine("  demo");
            CliOutput.WriteLine("  exit");
            CliOutput.WriteLine("From PowerShell or cmd, prefix one-shot runs with truck-fleet-problem.");
            CliOutput.WriteBlankLine();
            CliHelpTemplates.Print();
        }

        private static string Fmt(float value) => value.ToString(CultureInfo.InvariantCulture);

        private static string Fmt(double value) => value.ToString(CultureInfo.InvariantCulture);
    }
}
