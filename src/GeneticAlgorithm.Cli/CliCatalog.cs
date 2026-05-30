using System;
using GeneticAlgorithm.Application;

namespace GeneticAlgorithm.Cli
{
    internal sealed class CliCommandInfo
    {
        public CliCommandInfo(
            string command,
            string tab,
            string description,
            string aliases = null,
            bool interactiveOnly = false)
        {
            Command = command;
            Tab = tab;
            Description = description;
            Aliases = aliases;
            InteractiveOnly = interactiveOnly;
        }

        public string Command { get; }
        public string Tab { get; }
        public string Description { get; }
        public string Aliases { get; }
        public bool InteractiveOnly { get; }
    }

    internal static class CliCatalog
    {
        public const string ExecutableName = "truck-fleet-problem";

        public static readonly CliCommandInfo[] Commands =
        {
            new CliCommandInfo(
                "simulation",
                OptimizationPhaseDisplay.SimulationTab,
                "Run one fleet simulation (same as POST /api/simulation).",
                "sim, simulate"),
            new CliCommandInfo(
                "genetic-algorithm",
                OptimizationPhaseDisplay.GeneticAlgorithmTab,
                "Classic GA with stochastic simulation fitness (POST /api/genetic-algorithm).",
                "ga, genetic, genetic-algo"),
            new CliCommandInfo(
                "exhaustive-search",
                OptimizationPhaseDisplay.ExhaustiveSearch,
                "Try every combo within max bounds (POST /api/exhaustive-search).",
                "exhaustive, phase1"),
            new CliCommandInfo(
                "genetic-search",
                OptimizationPhaseDisplay.GeneticSearch,
                "GA on expected-time fitness + verify (POST /api/genetic-search).",
                "optimize, phase2"),
            new CliCommandInfo(
                "surrogate-search",
                OptimizationPhaseDisplay.SurrogateSearch,
                "Surrogate ranking + verify (POST /api/surrogate-search).",
                "surrogate, phase3"),
            new CliCommandInfo(
                "dynamic-programming-search",
                OptimizationPhaseDisplay.DynamicProgrammingSearch,
                "3D DP on expected-time sim (POST /api/dynamic-programming-search).",
                "dp, phase4, dynamic-programming"),
            new CliCommandInfo(
                "demo",
                OptimizationPhaseDisplay.GeneticSearch,
                "Run Genetic Search with demo defaults (20 generations)."),
            new CliCommandInfo("help", "Help", "Show this help.", "-h, --help, ?"),
            new CliCommandInfo("clear", "Shell", "Clear the screen.", "cls", interactiveOnly: true),
            new CliCommandInfo("exit", "Shell", "Quit interactive shell (or Ctrl+C).", "quit, q", interactiveOnly: true)
        };

        public static bool TryResolve(string token, out string canonicalCommand, out CliCommandInfo commandInfo)
        {
            canonicalCommand = null;
            commandInfo = null;

            if (string.IsNullOrWhiteSpace(token))
                return false;

            string lower = token.ToLowerInvariant();

            foreach (CliCommandInfo command in Commands)
            {
                if (string.Equals(command.Command, lower, StringComparison.Ordinal))
                {
                    canonicalCommand = command.Command;
                    commandInfo = command;
                    return true;
                }

                if (string.IsNullOrWhiteSpace(command.Aliases))
                    continue;

                foreach (string alias in command.Aliases.Split(',', StringSplitOptions.TrimEntries))
                {
                    if (string.Equals(alias, lower, StringComparison.Ordinal))
                    {
                        canonicalCommand = command.Command;
                        commandInfo = command;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
