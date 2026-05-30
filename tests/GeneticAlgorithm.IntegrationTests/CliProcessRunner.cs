using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GeneticAlgorithm.IntegrationTests
{
    internal sealed class CliRunResult
    {
        public CliRunResult(int exitCode, string standardOutput, string standardError)
        {
            ExitCode = exitCode;
            StandardOutput = standardOutput;
            StandardError = standardError;
        }

        public int ExitCode { get; }
        public string StandardOutput { get; }
        public string StandardError { get; }
        public string CombinedOutput => StandardOutput + StandardError;
    }

    internal static class CliProcessRunner
    {
        private static readonly string CliDllPath =
            Path.Combine(AppContext.BaseDirectory, "truck-fleet-problem.dll");

        /// <summary>Runs the CLI with the given arguments, each passed as a discrete token.</summary>
        public static CliRunResult Run(params string[] args)
        {
            if (!File.Exists(CliDllPath))
                throw new FileNotFoundException(
                    "CLI assembly was not copied to the test output directory.", CliDllPath);

            return Execute(args);
        }

        /// <summary>
        /// Runs the CLI with <paramref name="command"/> followed by flags split from
        /// <paramref name="optionFlags"/> on whitespace.  Flag values must not contain spaces;
        /// pass arguments via <see cref="Run"/> directly when values may contain spaces.
        /// </summary>
        public static CliRunResult RunCommand(string command, string optionFlags)
        {
            if (string.IsNullOrWhiteSpace(optionFlags))
                return Run(command);

            string[] flagTokens = optionFlags.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var args = new string[flagTokens.Length + 1];
            args[0] = command;
            Array.Copy(flagTokens, 0, args, 1, flagTokens.Length);
            return Run(args);
        }

        private static CliRunResult Execute(IReadOnlyList<string> args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            // ArgumentList lets the runtime handle OS-level quoting/escaping correctly,
            // avoiding the CommandLineToArgvW backslash-before-quote edge cases.
            startInfo.ArgumentList.Add("exec");
            startInfo.ArgumentList.Add(CliDllPath);
            foreach (string arg in args)
                startInfo.ArgumentList.Add(arg);

            using var process = Process.Start(startInfo)
                ?? throw new InvalidOperationException("Failed to start truck-fleet-problem CLI process.");

            string stdout = process.StandardOutput.ReadToEnd();
            string stderr = process.StandardError.ReadToEnd();

            if (!process.WaitForExit(120_000))
            {
                try { process.Kill(entireProcessTree: true); } catch { /* best effort */ }
                throw new TimeoutException(
                    $"CLI command timed out after 120 s. First arg: {(args.Count > 0 ? args[0] : "<none>")}");
            }

            return new CliRunResult(process.ExitCode, stdout, stderr);
        }
    }
}
