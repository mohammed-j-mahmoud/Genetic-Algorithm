using System;
using System.Collections.Generic;

namespace GeneticAlgorithm.Cli
{
    internal sealed class ParsedCommand
    {
        public string Command { get; set; }
        public Dictionary<string, string> Options { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> Flags { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public bool HasFlag(string name) => Flags.Contains(Normalize(name));

        public bool TryGetOption(string name, out string value) =>
            Options.TryGetValue(Normalize(name), out value);

        public bool TryGetInt(string name, out int value)
        {
            value = 0;
            return TryGetOption(name, out string text) && int.TryParse(text, out value);
        }

        public bool TryGetFloat(string name, out float value)
        {
            value = 0;
            return TryGetOption(name, out string text) && float.TryParse(text, out value);
        }

        public bool TryGetDouble(string name, out double value)
        {
            value = 0;
            return TryGetOption(name, out string text) && double.TryParse(text, out value);
        }

        private static string Normalize(string name) => name.Trim().TrimStart('-').ToLowerInvariant();
    }

    internal static class CliArgumentParser
    {
        private static readonly Dictionary<string, string> ShortToLong = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["g"] = "generations",
            ["p"] = "population",
            ["m"] = "mutation-rate",
            ["c"] = "coal",
            ["t"] = "trucks",
            ["l"] = "loaders",
            ["s"] = "scalers"
        };

        public static ParsedCommand Parse(string[] args)
        {
            if (args == null || args.Length == 0)
                throw new ArgumentException("Command is required.");

            var parsed = new ParsedCommand { Command = args[0] };

            for (int i = 1; i < args.Length; i++)
            {
                string arg = args[i];
                if (IsOptionToken(arg))
                {
                    string key = NormalizeOptionKey(arg);
                    if (i + 1 < args.Length && !IsOptionToken(args[i + 1]))
                    {
                        parsed.Options[key] = args[++i];
                    }
                    else
                    {
                        parsed.Flags.Add(key);
                    }

                    continue;
                }

                if (IsGeneticSearchCommand(parsed.Command) && !parsed.Options.ContainsKey("generations"))
                {
                    parsed.Options["generations"] = arg;
                    continue;
                }

                throw new ArgumentException($"Unexpected argument '{arg}'. Options must start with '-' or '--'. Type 'help' for usage.");
            }

            return parsed;
        }

        private static bool IsOptionToken(string arg) =>
            arg.StartsWith("--", StringComparison.Ordinal) ||
            (arg.StartsWith("-", StringComparison.Ordinal) && arg.Length > 1 && !double.TryParse(arg, out _));

        private static string NormalizeOptionKey(string arg)
        {
            string key = arg.TrimStart('-');
            if (arg.StartsWith("-", StringComparison.Ordinal) &&
                !arg.StartsWith("--", StringComparison.Ordinal) &&
                ShortToLong.TryGetValue(key, out string mapped))
            {
                return mapped;
            }

            return key.Replace('_', '-').ToLowerInvariant();
        }

        private static bool IsGeneticSearchCommand(string command)
        {
            switch (command.ToLowerInvariant())
            {
                case "ga":
                case "genetic":
                case "genetic-search":
                case "optimize":
                case "phase2":
                case "demo":
                    return true;
                default:
                    return false;
            }
        }
    }
}
