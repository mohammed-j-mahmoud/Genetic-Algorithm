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
            string exe = CliCatalog.ExecutableName;

            SimulationRequest sim = DemoRequests.CreateSimulation();
            GeneticOptimizationRequest opt = DemoRequests.CreateOptimization(20);

            CliOutput.WriteTitle($"{OptimizationPhaseDisplay.ApplicationTitle} — command-line help");
            CliOutput.WriteLine("Same desktop tabs as the WinForms app and the HTTP API.");
            CliOutput.WriteBlankLine();

            CliOutput.WriteLine("HOW TO RUN");
            CliOutput.WriteLine("  Interactive shell   dotnet run --project src/GeneticAlgorithm.Cli");
            CliOutput.WriteLine("                      (or run truck-fleet-problem with no arguments)");
            CliOutput.WriteLine($"                      Prompt: {OptimizationPhaseDisplay.CliPrompt}type commands below — no \"{exe}\" prefix.");
            CliOutput.WriteLine("  One-shot command    truck-fleet-problem <command> [options]");
            CliOutput.WriteLine("                      Good for scripts and PowerShell/cmd.");
            CliOutput.WriteBlankLine();

            CliOutput.WriteLine("QUICK START (interactive — at the truck-fleet> prompt)");
            CliOutput.WriteLine("  help");
            CliOutput.WriteLine("  simulation");
            CliOutput.WriteLine($"  simulation --coal {Fmt(sim.CoalVolume)} --trucks {Fmt(sim.TruckCount)} --loaders {Fmt(sim.LoaderCount)} --scalers {Fmt(sim.ScalerCount)} --load-per-truck {Fmt(sim.TruckLoadVolume)}");
            CliOutput.WriteLine("  exhaustive-search --max-trucks 6 --max-loaders 2 --max-scalers 2");
            CliOutput.WriteLine("  genetic-algorithm --generations 20 --max-trucks 6");
            CliOutput.WriteLine("  genetic-search --generations 20 --max-trucks 6");
            CliOutput.WriteLine("  exit");
            CliOutput.WriteBlankLine();

            CliOutput.WriteLine("ALL COMMANDS (matches API routes)");
            foreach (CliCommandInfo command in CliCatalog.Commands)
            {
                string aliasSuffix = string.IsNullOrWhiteSpace(command.Aliases)
                    ? string.Empty
                    : $"  (aliases: {command.Aliases})";
                string modeSuffix = command.InteractiveOnly ? "  [interactive shell only]" : string.Empty;
                CliOutput.WriteLine($"  {command.Command,-30} {command.Tab,-28} {command.Description}{aliasSuffix}{modeSuffix}");
            }
            CliOutput.WriteBlankLine();

            CliOutput.WriteLine("REQUIRED OPTIONS");
            CliOutput.WriteLine("  simulation — needs --trucks, --loaders, --scalers, and --load-per-truck.");
            CliOutput.WriteLine("  search commands — need --max-trucks, --max-loaders, --max-scalers, and --load-per-truck.");
            CliOutput.WriteLine("    (Search does not use --trucks/--loaders/--scalers — same as WinForms GA inputs.)");
            CliOutput.WriteLine($"  Demo defaults when omitted: coal={Fmt(sim.CoalVolume)}, load-per-truck={Fmt(sim.TruckLoadVolume)}.");
            CliOutput.WriteLine("  Distributions use desktop demo values unless changed in the API or Distribution tab.");
            CliOutput.WriteBlankLine();

            CliOutput.WriteLine("SIMULATION-ONLY OPTIONS (simulation command — fixed fleet size)");
            CliOutput.WriteLine($"  --coal, -c <float>           Amount of material (default demo: {Fmt(sim.CoalVolume)})");
            CliOutput.WriteLine("  --trucks, -t <float>         Number of trucks for this run");
            CliOutput.WriteLine("  --loaders, -l <float>        Number of loaders");
            CliOutput.WriteLine("  --scalers, -s <float>        Number of scalers");
            CliOutput.WriteLine($"  --load-per-truck <float>     Load per truck — required (default demo: {Fmt(sim.TruckLoadVolume)})");
            CliOutput.WriteLine($"  --truck-cost <float>         Cost per truck per day (default demo: {Fmt(sim.TruckCostPerDay)})");
            CliOutput.WriteLine($"  --loader-cost <float>        Cost per loader per day (default demo: {Fmt(sim.LoaderCostPerDay)})");
            CliOutput.WriteLine($"  --scaler-cost <float>        Cost per scaler per day (default demo: {Fmt(sim.ScalerCostPerDay)})");
            CliOutput.WriteLine($"  --project-days <float>       Project duration in days (default demo: {Fmt(sim.ProjectDurationDays)})");
            CliOutput.WriteLine($"  --delay-cost <float>         Cost of delay per day (default demo: {Fmt(sim.DelayCostPerDay)})");
            CliOutput.WriteBlankLine();

            CliOutput.WriteLine("SEARCH CONTEXT (search commands — economics only, same as WinForms GA tab)");
            CliOutput.WriteLine($"  --coal, -c <float>           Amount of material (default demo: {Fmt(sim.CoalVolume)})");
            CliOutput.WriteLine($"  --load-per-truck <float>     Load per truck — required (default demo: {Fmt(sim.TruckLoadVolume)})");
            CliOutput.WriteLine($"  --truck-cost <float>         Cost per truck per day (default demo: {Fmt(sim.TruckCostPerDay)})");
            CliOutput.WriteLine($"  --loader-cost <float>        Cost per loader per day (default demo: {Fmt(sim.LoaderCostPerDay)})");
            CliOutput.WriteLine($"  --scaler-cost <float>        Cost per scaler per day (default demo: {Fmt(sim.ScalerCostPerDay)})");
            CliOutput.WriteLine($"  --project-days <float>       Project duration in days (default demo: {Fmt(sim.ProjectDurationDays)})");
            CliOutput.WriteLine($"  --delay-cost <float>         Cost of delay per day (default demo: {Fmt(sim.DelayCostPerDay)})");
            CliOutput.WriteBlankLine();

            CliOutput.WriteLine("SEARCH OPTIONS (genetic-algorithm / exhaustive / genetic / surrogate / dp)");
            CliOutput.WriteLine($"  --max-trucks <int>           Max trucks gene bound (default demo: {opt.MaxTrucks})");
            CliOutput.WriteLine($"  --max-loaders <int>          Max loaders gene bound (default demo: {opt.MaxLoaders})");
            CliOutput.WriteLine($"  --max-scalers <int>          Max scalers gene bound (default demo: {opt.MaxScalers})");
            CliOutput.WriteLine("  --generations, -g <int>      GA generations (genetic-algorithm / genetic-search default: 50; demo: 20)");
            CliOutput.WriteLine($"  --population, -p <int>       GA population (default demo: {opt.PopulationSize})");
            CliOutput.WriteLine($"  --mutation-rate, -m <0-1>   GA mutation rate (default demo: {Fmt(opt.MutationRate)})");
            CliOutput.WriteLine($"  Limits: generations/population/fleet bounds ≤ {max}; fleet counts ≤ {maxFleet}.");
            CliOutput.WriteBlankLine();

            CliOutput.WriteLine("EXAMPLES — ONE-SHOT (PowerShell / cmd — include executable prefix)");
            CliOutput.WriteLine($"  {exe} help");
            CliOutput.WriteLine($"  {exe} simulation --coal 5000 --trucks 4 --loaders 2 --scalers 1 --load-per-truck 20");
            CliOutput.WriteLine($"  {exe} exhaustive-search --max-trucks 6 --max-loaders 2 --max-scalers 2");
            CliOutput.WriteLine($"  {exe} genetic-algorithm --generations 30 --max-trucks 8 --coal 10000 --load-per-truck 20");
            CliOutput.WriteLine($"  {exe} genetic-search --generations 30 --max-trucks 8 --coal 10000 --load-per-truck 20");
            CliOutput.WriteLine($"  {exe} ga -g 20 -p 40 --max-trucks 10 --mutation-rate 0.02 --load-per-truck 20");
            CliOutput.WriteLine($"  {exe} surrogate-search --max-trucks 6 --max-loaders 2 --max-scalers 2");
            CliOutput.WriteLine($"  {exe} dp --max-trucks 6 --max-loaders 2 --max-scalers 2");
            CliOutput.WriteLine($"  {exe} demo");
            CliOutput.WriteBlankLine();

            CliHelpTemplates.Print();
        }

        private static string Fmt(float value) => value.ToString(CultureInfo.InvariantCulture);

        private static string Fmt(double value) => value.ToString(CultureInfo.InvariantCulture);
    }
}
