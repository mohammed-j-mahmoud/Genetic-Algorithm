using System;
using System.Globalization;
using System.Text;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;

namespace GeneticAlgorithm.Cli
{
    internal static class CliHelpTemplates
    {
        public static void Print()
        {
            SimulationRequest sim = DemoRequests.CreateSimulation();
            GeneticOptimizationRequest opt = DemoRequests.CreateOptimization(20);
            GeneticOptimizationRequest gaOpt = DemoRequests.CreateOptimization(50);

            CliOutput.WriteTitle("COPY-PASTE TEMPLATES (edit values, then run)");
            CliOutput.WriteLine("Copy a template below. Edit the numbers on each --flag line before running.");
            CliOutput.WriteLine("Delete comment lines (# ...) before running.");
            CliOutput.WriteLine("At the truck-fleet> prompt, run commands as shown (no executable prefix).");
            CliOutput.WriteLine("From PowerShell or cmd, prefix one-shot runs with truck-fleet-problem.");
            CliOutput.WriteLine("Wrapped templates use PowerShell backtick (`) line continuation.");
            CliOutput.WriteLine("Or use the single-line version under each template.");
            CliOutput.WriteLine("For other search tabs, replace genetic-search with exhaustive-search,");
            CliOutput.WriteLine("surrogate-search, or dynamic-programming-search (omit GA-only flags).");
            CliOutput.WriteLine("Use genetic-algorithm for the main Genetic Algorithm tab (stochastic fitness).");
            CliOutput.WriteBlankLine();

            PrintGeneticAlgorithmTemplate(sim, gaOpt);
            CliOutput.WriteBlankLine();
            PrintSimulationTemplate(sim);
            CliOutput.WriteBlankLine();
            PrintGeneticSearchTemplate(sim, opt);
        }

        private static void PrintGeneticAlgorithmTemplate(
            SimulationRequest sim,
            GeneticOptimizationRequest opt)
        {
            CliOutput.WriteSubTitle($"TEMPLATE: genetic-algorithm ({OptimizationPhaseDisplay.GeneticAlgorithmTab} tab)");
            CliOutput.WriteLine("# kind: GeneticOptimizationRequest");
            CliOutput.WriteLine("# defaults: desktop demo + 50 generations");
            WriteWrappedCommand("genetic-algorithm", BuildOptimizationEntries(sim, opt));
            CliOutput.WriteLine("# single line:");
            CliOutput.WriteLine(BuildSingleLine("genetic-algorithm", sim, opt));
        }

        private static void PrintSimulationTemplate(SimulationRequest sim)
        {
            CliOutput.WriteSubTitle($"TEMPLATE: simulation ({OptimizationPhaseDisplay.SimulationTab} tab)");
            CliOutput.WriteLine("# kind: Simulation");
            CliOutput.WriteLine("# defaults: desktop demo (Distribution tab timings applied automatically)");
            WriteWrappedCommand("simulation", new[]
            {
                Entry("simulation.coalVolume — material volume", "--coal", sim.CoalVolume),
                Entry("simulation.truckCount — number of trucks", "--trucks", sim.TruckCount),
                Entry("simulation.loaderCount — number of loaders", "--loaders", sim.LoaderCount),
                Entry("simulation.scalerCount — number of scalers", "--scalers", sim.ScalerCount),
                Entry("simulation.truckLoadVolume — load per truck", "--load-per-truck", sim.TruckLoadVolume),
                Entry("simulation.truckCostPerDay — cost per truck / day", "--truck-cost", sim.TruckCostPerDay),
                Entry("simulation.loaderCostPerDay — cost per loader / day", "--loader-cost", sim.LoaderCostPerDay),
                Entry("simulation.scalerCostPerDay — cost per scaler / day", "--scaler-cost", sim.ScalerCostPerDay),
                Entry("simulation.projectDurationDays — project duration (days)", "--project-days", sim.ProjectDurationDays),
                Entry("simulation.delayCostPerDay — delay cost / day", "--delay-cost", sim.DelayCostPerDay)
            });
            CliOutput.WriteLine("# single line:");
            CliOutput.WriteLine(BuildSingleLine("simulation", sim));
        }

        private static void PrintGeneticSearchTemplate(
            SimulationRequest sim,
            GeneticOptimizationRequest opt)
        {
            CliOutput.WriteSubTitle($"TEMPLATE: genetic-search ({OptimizationPhaseDisplay.GeneticSearch} tab)");
            CliOutput.WriteLine("# kind: GeneticOptimizationRequest");
            CliOutput.WriteLine("# defaults: desktop demo + 20 generations");
            WriteWrappedCommand("genetic-search", BuildOptimizationEntries(sim, opt));
            CliOutput.WriteLine("# single line:");
            CliOutput.WriteLine(BuildSingleLine("genetic-search", sim, opt));
        }

        private static TemplateEntry[] BuildOptimizationEntries(
            SimulationRequest sim,
            GeneticOptimizationRequest opt) =>
            new[]
            {
                Entry("simulation.coalVolume — material volume", "--coal", sim.CoalVolume),
                Entry("simulation.truckCount — trucks for verify simulation", "--trucks", sim.TruckCount),
                Entry("simulation.loaderCount — loaders for verify simulation", "--loaders", sim.LoaderCount),
                Entry("simulation.scalerCount — scalers for verify simulation", "--scalers", sim.ScalerCount),
                Entry("simulation.truckLoadVolume — load per truck", "--load-per-truck", sim.TruckLoadVolume),
                Entry("simulation.truckCostPerDay — truck cost / day", "--truck-cost", sim.TruckCostPerDay),
                Entry("simulation.loaderCostPerDay — loader cost / day", "--loader-cost", sim.LoaderCostPerDay),
                Entry("simulation.scalerCostPerDay — scaler cost / day", "--scaler-cost", sim.ScalerCostPerDay),
                Entry("simulation.projectDurationDays — project duration (days)", "--project-days", sim.ProjectDurationDays),
                Entry("simulation.delayCostPerDay — delay cost / day", "--delay-cost", sim.DelayCostPerDay),
                Entry("search.maxTrucks — max trucks gene bound", "--max-trucks", opt.MaxTrucks),
                Entry("search.maxLoaders — max loaders gene bound", "--max-loaders", opt.MaxLoaders),
                Entry("search.maxScalers — max scalers gene bound", "--max-scalers", opt.MaxScalers),
                Entry("search.generations — GA generations", "--generations", opt.Generations),
                Entry("search.populationSize — GA population", "--population", opt.PopulationSize),
                Entry("search.mutationRate — GA mutation rate (0-1)", "--mutation-rate", opt.MutationRate)
            };

        /// <summary>
        /// Single generic overload replaces the previous float/double/int trio.
        /// All numeric types that implement IFormattable use invariant culture automatically.
        /// </summary>
        private static TemplateEntry Entry<T>(string comment, string flag, T value) where T : IFormattable =>
            new TemplateEntry(comment, flag, value.ToString(null, CultureInfo.InvariantCulture));

        private static void WriteWrappedCommand(string command, TemplateEntry[] entries)
        {
            CliOutput.WriteLine(command + " `");
            for (int i = 0; i < entries.Length; i++)
            {
                TemplateEntry entry = entries[i];
                CliOutput.WriteLine("# " + entry.Comment);
                bool isLast = i == entries.Length - 1;
                string continuation = isLast ? string.Empty : " `";
                CliOutput.WriteLine($"  {entry.Flag} {entry.Value}{continuation}");
            }
        }

        /// <summary>
        /// Passing <paramref name="opt"/> includes the GA-specific flags in the single-line output.
        /// Removing the redundant boolean flag: opt != null is the condition.
        /// </summary>
        private static string BuildSingleLine(
            string command,
            SimulationRequest sim,
            GeneticOptimizationRequest opt = null)
        {
            var parts = new StringBuilder();
            parts.Append(command);
            Append(parts, "--coal", sim.CoalVolume);
            Append(parts, "--trucks", sim.TruckCount);
            Append(parts, "--loaders", sim.LoaderCount);
            Append(parts, "--scalers", sim.ScalerCount);
            Append(parts, "--load-per-truck", sim.TruckLoadVolume);
            Append(parts, "--truck-cost", sim.TruckCostPerDay);
            Append(parts, "--loader-cost", sim.LoaderCostPerDay);
            Append(parts, "--scaler-cost", sim.ScalerCostPerDay);
            Append(parts, "--project-days", sim.ProjectDurationDays);
            Append(parts, "--delay-cost", sim.DelayCostPerDay);

            if (opt != null)
            {
                Append(parts, "--max-trucks", opt.MaxTrucks);
                Append(parts, "--max-loaders", opt.MaxLoaders);
                Append(parts, "--max-scalers", opt.MaxScalers);
                Append(parts, "--generations", opt.Generations);
                Append(parts, "--population", opt.PopulationSize);
                Append(parts, "--mutation-rate", opt.MutationRate);
            }

            return parts.ToString();
        }

        private static void Append<T>(StringBuilder builder, string flag, T value) where T : IFormattable =>
            builder.Append(' ').Append(flag).Append(' ').Append(value.ToString(null, CultureInfo.InvariantCulture));

        private sealed class TemplateEntry
        {
            public TemplateEntry(string comment, string flag, string value)
            {
                Comment = comment;
                Flag = flag;
                Value = value;
            }

            public string Comment { get; }
            public string Flag { get; }
            public string Value { get; }
        }
    }
}
