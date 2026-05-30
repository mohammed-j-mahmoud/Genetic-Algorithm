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
            GeneticOptimizationRequest searchOpt = DemoRequests.CreateOptimization(50);
            GeneticOptimizationRequest gaOpt = DemoRequests.CreateOptimization(50);

            CliOutput.WriteTitle("COPY-PASTE TEMPLATES (edit values, then run)");
            CliOutput.WriteLine("Use at the truck-fleet> prompt — type commands as shown (no executable prefix).");
            CliOutput.WriteLine($"From PowerShell/cmd one-shot: prefix with {CliCatalog.ExecutableName}.");
            CliOutput.WriteLine("Always include --load-per-truck (demo default 20). Omitting it causes validation errors.");
            CliOutput.WriteLine("Same simulation flags work for simulation and all search commands.");
            CliOutput.WriteLine("For other search tabs, change the command name only:");
            CliOutput.WriteLine("  exhaustive-search | genetic-search | surrogate-search | dynamic-programming-search");
            CliOutput.WriteLine("  genetic-algorithm = main GA tab (stochastic fitness). genetic-search = comparison tab.");
            CliOutput.WriteLine("Wrapped templates use PowerShell backtick (`) line continuation.");
            CliOutput.WriteLine("Delete # comment lines before running, or use the single-line version.");
            CliOutput.WriteBlankLine();

            PrintSimulationTemplate(sim);
            CliOutput.WriteBlankLine();
            PrintSearchTemplate("exhaustive-search", OptimizationPhaseDisplay.ExhaustiveSearch, sim, searchOpt, includeGaFlags: false);
            CliOutput.WriteBlankLine();
            PrintGeneticAlgorithmTemplate(sim, gaOpt);
            CliOutput.WriteBlankLine();
            PrintSearchTemplate("genetic-search", OptimizationPhaseDisplay.GeneticSearch, sim, searchOpt, includeGaFlags: true);
            CliOutput.WriteBlankLine();
            PrintSearchTemplate("surrogate-search", OptimizationPhaseDisplay.SurrogateSearch, sim, searchOpt, includeGaFlags: false);
            CliOutput.WriteBlankLine();
            PrintSearchTemplate("dynamic-programming-search", OptimizationPhaseDisplay.DynamicProgrammingSearch, sim, searchOpt, includeGaFlags: false);
        }

        private static void PrintGeneticAlgorithmTemplate(
            SimulationRequest sim,
            GeneticOptimizationRequest opt)
        {
            CliOutput.WriteSubTitle($"TEMPLATE: genetic-algorithm ({OptimizationPhaseDisplay.GeneticAlgorithmTab} tab)");
            CliOutput.WriteLine("# API equivalent: POST /api/genetic-algorithm");
            CliOutput.WriteLine("# defaults: desktop demo + 50 generations");
            WriteWrappedCommand("genetic-algorithm", BuildOptimizationEntries(sim, opt, includeGaFlags: true));
            CliOutput.WriteLine("# single line:");
            CliOutput.WriteLine(BuildSingleLine("genetic-algorithm", sim, opt, includeGaFlags: true));
        }

        private static void PrintSimulationTemplate(SimulationRequest sim)
        {
            CliOutput.WriteSubTitle($"TEMPLATE: simulation ({OptimizationPhaseDisplay.SimulationTab} tab)");
            CliOutput.WriteLine("# API equivalent: POST /api/simulation");
            CliOutput.WriteLine("# defaults: desktop demo (Distribution tab timings applied automatically)");
            WriteWrappedCommand("simulation", BuildSimulationEntries(sim));
            CliOutput.WriteLine("# single line:");
            CliOutput.WriteLine(BuildSingleLine("simulation", sim));
        }

        private static void PrintSearchTemplate(
            string command,
            string tabName,
            SimulationRequest sim,
            GeneticOptimizationRequest opt,
            bool includeGaFlags)
        {
            CliOutput.WriteSubTitle($"TEMPLATE: {command} ({tabName} tab)");
            CliOutput.WriteLine($"# API equivalent: POST /api/{command}");
            WriteWrappedCommand(command, BuildOptimizationEntries(sim, opt, includeGaFlags));
            CliOutput.WriteLine("# single line:");
            CliOutput.WriteLine(BuildSingleLine(command, sim, opt, includeGaFlags));
        }

        private static TemplateEntry[] BuildSimulationEntries(SimulationRequest sim) =>
            new[]
            {
                Entry("simulation.coalVolume — material volume", "--coal", sim.CoalVolume),
                Entry("simulation.truckCount — number of trucks", "--trucks", sim.TruckCount),
                Entry("simulation.loaderCount — number of loaders", "--loaders", sim.LoaderCount),
                Entry("simulation.scalerCount — number of scalers", "--scalers", sim.ScalerCount),
                Entry("simulation.truckLoadVolume — load per truck (required)", "--load-per-truck", sim.TruckLoadVolume),
                Entry("simulation.truckCostPerDay — cost per truck / day", "--truck-cost", sim.TruckCostPerDay),
                Entry("simulation.loaderCostPerDay — cost per loader / day", "--loader-cost", sim.LoaderCostPerDay),
                Entry("simulation.scalerCostPerDay — cost per scaler / day", "--scaler-cost", sim.ScalerCostPerDay),
                Entry("simulation.projectDurationDays — project duration (days)", "--project-days", sim.ProjectDurationDays),
                Entry("simulation.delayCostPerDay — delay cost / day", "--delay-cost", sim.DelayCostPerDay)
            };

        private static TemplateEntry[] BuildOptimizationEntries(
            SimulationRequest sim,
            GeneticOptimizationRequest opt,
            bool includeGaFlags)
        {
            var entries = new System.Collections.Generic.List<TemplateEntry>(BuildSimulationEntries(sim));
            entries.Add(Entry("search.maxTrucks — max trucks gene bound", "--max-trucks", opt.MaxTrucks));
            entries.Add(Entry("search.maxLoaders — max loaders gene bound", "--max-loaders", opt.MaxLoaders));
            entries.Add(Entry("search.maxScalers — max scalers gene bound", "--max-scalers", opt.MaxScalers));

            if (includeGaFlags)
            {
                entries.Add(Entry("search.generations — GA generations", "--generations", opt.Generations));
                entries.Add(Entry("search.populationSize — GA population", "--population", opt.PopulationSize));
                entries.Add(Entry("search.mutationRate — GA mutation rate (0-1)", "--mutation-rate", opt.MutationRate));
            }

            return entries.ToArray();
        }

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

        private static string BuildSingleLine(
            string command,
            SimulationRequest sim,
            GeneticOptimizationRequest opt = null,
            bool includeGaFlags = false)
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

                if (includeGaFlags)
                {
                    Append(parts, "--generations", opt.Generations);
                    Append(parts, "--population", opt.PopulationSize);
                    Append(parts, "--mutation-rate", opt.MutationRate);
                }
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
