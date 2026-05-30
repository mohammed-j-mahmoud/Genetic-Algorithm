using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.IntegrationTests
{
    internal static class IntegrationTestFixtures
    {
        /// <summary>Gene index for the truck count in a <see cref="DNA"/> chromosome.</summary>
        internal const int TrucksGeneIndex = 0;

        /// <summary>Gene index for the loader count in a <see cref="DNA"/> chromosome.</summary>
        internal const int LoadersGeneIndex = 1;

        /// <summary>Gene index for the scaler count in a <see cref="DNA"/> chromosome.</summary>
        internal const int ScalersGeneIndex = 2;

        // ── CLI flag strings ──────────────────────────────────────────────────────────

        internal const string CliQuickSimulationFlags =
            "--coal 40 --trucks 2 --loaders 1 --scalers 1 --load-per-truck 20 --truck-cost 100 " +
            "--loader-cost 200 --scaler-cost 300 --project-days 30 --delay-cost 50";

        internal const string CliQuickOptimizationFlags =
            "--generations 3 --population 4 --max-trucks 2 --max-loaders 2 --max-scalers 2 " +
            "--coal 40 --trucks 2 --loaders 1 --scalers 1 --load-per-truck 20 --truck-cost 100 " +
            "--loader-cost 200 --scaler-cost 300 --project-days 30 --delay-cost 50";

        internal const string CliPhaseGridFlags =
            "--generations 3 --population 4 --max-trucks 2 --max-loaders 2 --max-scalers 2 " +
            "--coal 80 --trucks 2 --loaders 1 --scalers 1 --load-per-truck 20 --truck-cost 100 " +
            "--loader-cost 200 --scaler-cost 300 --project-days 30 --delay-cost 50";

        // ── WebApplicationFactory ─────────────────────────────────────────────────────

        /// <summary>Creates a fresh in-process API factory for each test.</summary>
        internal static WebApplicationFactory<GeneticAlgorithm.Api.Program> CreateApiFactory() =>
            new WebApplicationFactory<GeneticAlgorithm.Api.Program>();

        // ── Request builders ──────────────────────────────────────────────────────────

        public static SimulationRequest CreateSmallSimulation() =>
            new SimulationRequest
            {
                CoalVolume = 40,
                TruckCount = 2,
                TruckLoadVolume = 20,
                TruckCostPerDay = 100,
                LoaderCount = 1,
                LoaderCostPerDay = 200,
                ScalerCount = 1,
                ScalerCostPerDay = 300,
                ProjectDurationDays = 30,
                DelayCostPerDay = 50
            };

        public static GeneticOptimizationRequest CreateSmallOptimizationRequest() =>
            new GeneticOptimizationRequest
            {
                Generations = 3,
                PopulationSize = 4,
                MaxTrucks = 2,
                MaxLoaders = 2,
                MaxScalers = 2,
                MutationRate = 0.01,
                Simulation = CreateSmallSimulation()
            };

        public static GeneticOptimizationRequest CreatePhaseGridRequest() =>
            new GeneticOptimizationRequest
            {
                PopulationSize = 4,
                MaxTrucks = 2,
                MaxLoaders = 2,
                MaxScalers = 2,
                Generations = 3,
                MutationRate = 0.01,
                Simulation = new SimulationRequest
                {
                    CoalVolume = 80,
                    TruckCount = 2,
                    TruckLoadVolume = 20,
                    TruckCostPerDay = 100,
                    LoaderCount = 1,
                    LoaderCostPerDay = 200,
                    ScalerCount = 1,
                    ScalerCostPerDay = 300,
                    ProjectDurationDays = 30,
                    DelayCostPerDay = 50
                }
            };

        // ── Assertion helpers ─────────────────────────────────────────────────────────

        public static void AssertSimulationMatches(
            DumpTruckSimulation.SimulationOutput expected,
            DumpTruckSimulation.SimulationOutput actual)
        {
            Assert.AreEqual(expected.TotalCost, actual.TotalCost, 0.01);
            Assert.AreEqual(expected.TotalDays, actual.TotalDays, 0.01);
            Assert.AreEqual(expected.DelayDays, actual.DelayDays, 0.01);
            Assert.AreEqual(expected.TruckUtilization, actual.TruckUtilization, 0.0001f);
            Assert.AreEqual(expected.LoaderUtilization, actual.LoaderUtilization, 0.0001f);
            Assert.AreEqual(expected.ScalerUtilization, actual.ScalerUtilization, 0.0001f);
        }

        public static void AssertOptimizationResponse(
            OptimizationRunResponse body,
            string expectedStrategy,
            string expectedPhase = null)
        {
            Assert.IsNotNull(body);
            Assert.AreEqual(OptimizationPhaseDisplay.ApplicationTitle, body.Application);
            Assert.AreEqual(expectedStrategy, body.Strategy);
            Assert.AreEqual(expectedStrategy, body.Tab);
            Assert.IsFalse(string.IsNullOrWhiteSpace(body.MethodSummary));
            Assert.IsTrue(body.Trucks >= 1);
            Assert.IsTrue(body.Loaders >= 1);
            Assert.IsTrue(body.Scalers >= 1);
            Assert.IsTrue(body.Fitness > 0);
            Assert.IsTrue(body.TotalCost > 0);
            Assert.IsTrue(body.TotalDays > 0);
            Assert.IsTrue(body.SimulationCalls > 0);

            if (expectedPhase != null)
                Assert.AreEqual(expectedPhase, body.Phase);
        }

        public static void AssertFleetMatches(OptimizationRunResult expected, OptimizationRunResponse actual)
        {
            Assert.AreEqual(expected.BestChromosome.Genes[TrucksGeneIndex], actual.Trucks);
            Assert.AreEqual(expected.BestChromosome.Genes[LoadersGeneIndex], actual.Loaders);
            Assert.AreEqual(expected.BestChromosome.Genes[ScalersGeneIndex], actual.Scalers);
        }

        public static void AssertFleetMatches(OptimizationRunResult expected, OptimizationRunResult actual)
        {
            Assert.AreEqual(
                expected.BestChromosome.Genes[TrucksGeneIndex],
                actual.BestChromosome.Genes[TrucksGeneIndex]);
            Assert.AreEqual(
                expected.BestChromosome.Genes[LoadersGeneIndex],
                actual.BestChromosome.Genes[LoadersGeneIndex]);
            Assert.AreEqual(
                expected.BestChromosome.Genes[ScalersGeneIndex],
                actual.BestChromosome.Genes[ScalersGeneIndex]);
        }

        // ── CLI output parsers ────────────────────────────────────────────────────────

        public static double ParseCliDouble(string output, string label)
        {
            Match match = Regex.Match(output, Regex.Escape(label) + @"\s*([\d\.]+)");
            Assert.IsTrue(match.Success, $"Expected '{label}' in CLI output:{Environment.NewLine}{output}");
            return double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        }

        public static int ParseCliInt(string output, string label)
        {
            Match match = Regex.Match(output, Regex.Escape(label) + @"\s*(\d+)");
            Assert.IsTrue(match.Success, $"Expected '{label}' in CLI output:{Environment.NewLine}{output}");
            return int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        }

        // ── Validation helpers ────────────────────────────────────────────────────────

        public static List<KeyValuePair<int, double>> CreateOversizedDistribution(int count)
        {
            var distribution = new List<KeyValuePair<int, double>>();
            for (int i = 0; i < count; i++)
                distribution.Add(new KeyValuePair<int, double>(i + 1, 1.0 / count));
            return distribution;
        }
    }

    internal sealed class ApiErrorResponse
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    internal sealed class HealthResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("application")]
        public string Application { get; set; }
    }
}
