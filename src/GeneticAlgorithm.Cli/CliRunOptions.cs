using System;
using System.Text;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Cli
{
    internal sealed class CliRunOptions
    {
        public GeneticOptimizationRequest BuildOptimizationRequest(int defaultGenerations)
        {
            var request = DemoRequests.CreateOptimization(defaultGenerations);
            ApplySimulationOverrides(request.Simulation);
            ApplyOptimizationOverrides(request, defaultGenerations);
            ValidateOptimization(request);
            return request;
        }

        public SimulationRequest BuildSimulationRequest()
        {
            var request = DemoRequests.CreateSimulation();
            ApplySimulationOverrides(request);
            ValidateSimulation(request);
            return request;
        }

        public void Apply(ParsedCommand parsed)
        {
            CopyOption(parsed, "coal", v => _coal = v);
            CopyOption(parsed, "material", v => _coal = v);
            CopyOption(parsed, "trucks", v => _trucks = v);
            CopyOption(parsed, "loaders", v => _loaders = v);
            CopyOption(parsed, "scalers", v => _scalers = v);
            CopyOption(parsed, "load-per-truck", v => _loadPerTruck = v);
            CopyOption(parsed, "truck-cost", v => _truckCost = v);
            CopyOption(parsed, "loader-cost", v => _loaderCost = v);
            CopyOption(parsed, "scaler-cost", v => _scalerCost = v);
            CopyOption(parsed, "project-days", v => _projectDays = v);
            CopyOption(parsed, "delay-cost", v => _delayCost = v);
            CopyOption(parsed, "max-trucks", v => _maxTrucks = v);
            CopyOption(parsed, "max-loaders", v => _maxLoaders = v);
            CopyOption(parsed, "max-scalers", v => _maxScalers = v);
            CopyOption(parsed, "generations", v => _generations = v);
            CopyOption(parsed, "population", v => _population = v);
            CopyOption(parsed, "mutation-rate", v => _mutationRate = v);
        }

        public string DescribeInputs()
        {
            var parts = new StringBuilder("inputs: demo defaults");
            Append(parts, "coal", _coal);
            Append(parts, "trucks", _trucks);
            Append(parts, "loaders", _loaders);
            Append(parts, "scalers", _scalers);
            Append(parts, "max-trucks", _maxTrucks);
            Append(parts, "max-loaders", _maxLoaders);
            Append(parts, "max-scalers", _maxScalers);
            Append(parts, "generations", _generations);
            Append(parts, "population", _population);
            Append(parts, "mutation-rate", _mutationRate);
            return parts.ToString();
        }

        private string _coal;
        private string _trucks;
        private string _loaders;
        private string _scalers;
        private string _loadPerTruck;
        private string _truckCost;
        private string _loaderCost;
        private string _scalerCost;
        private string _projectDays;
        private string _delayCost;
        private string _maxTrucks;
        private string _maxLoaders;
        private string _maxScalers;
        private string _generations;
        private string _population;
        private string _mutationRate;

        private static void CopyOption(ParsedCommand parsed, string name, Action<string> assign)
        {
            if (parsed.TryGetOption(name, out string value))
                assign(value);
        }

        private static void Append(StringBuilder builder, string name, string value)
        {
            if (value == null)
                return;

            builder.Append(", ").Append(name).Append('=').Append(value);
        }

        private void ApplySimulationOverrides(SimulationRequest simulation)
        {
            if (TryParseFloat(_coal, out float coal))
                simulation.CoalVolume = coal;
            if (TryParseFloat(_trucks, out float trucks))
                simulation.TruckCount = trucks;
            if (TryParseFloat(_loaders, out float loaders))
                simulation.LoaderCount = loaders;
            if (TryParseFloat(_scalers, out float scalers))
                simulation.ScalerCount = scalers;
            if (TryParseFloat(_loadPerTruck, out float loadPerTruck))
                simulation.TruckLoadVolume = loadPerTruck;
            if (TryParseFloat(_truckCost, out float truckCost))
                simulation.TruckCostPerDay = truckCost;
            if (TryParseFloat(_loaderCost, out float loaderCost))
                simulation.LoaderCostPerDay = loaderCost;
            if (TryParseFloat(_scalerCost, out float scalerCost))
                simulation.ScalerCostPerDay = scalerCost;
            if (TryParseFloat(_projectDays, out float projectDays))
                simulation.ProjectDurationDays = projectDays;
            if (TryParseFloat(_delayCost, out float delayCost))
                simulation.DelayCostPerDay = delayCost;
        }

        private void ApplyOptimizationOverrides(GeneticOptimizationRequest request, int defaultGenerations)
        {
            if (TryParseInt(_maxTrucks, out int maxTrucks))
                request.MaxTrucks = maxTrucks;
            if (TryParseInt(_maxLoaders, out int maxLoaders))
                request.MaxLoaders = maxLoaders;
            if (TryParseInt(_maxScalers, out int maxScalers))
                request.MaxScalers = maxScalers;
            if (TryParseInt(_generations, out int generations))
                request.Generations = generations;
            else
                request.Generations = defaultGenerations;
            if (TryParseInt(_population, out int population))
                request.PopulationSize = population;
            if (TryParseDouble(_mutationRate, out double mutationRate))
                request.MutationRate = mutationRate;
        }

        private static void ValidateSimulation(SimulationRequest request)
        {
            string error = SimulationRequestValidator.TryValidate(request);
            if (error != null)
                throw new ArgumentException(error);
        }

        private static void ValidateOptimization(GeneticOptimizationRequest request)
        {
            string error = PhaseOptimizationService.TryValidateRequest(request);
            if (error != null)
                throw new ArgumentException(error);
        }

        private static bool TryParseInt(string text, out int value)
        {
            value = 0;
            return text != null && int.TryParse(text, out value);
        }

        private static bool TryParseFloat(string text, out float value)
        {
            value = 0;
            return text != null && float.TryParse(text, out value);
        }

        private static bool TryParseDouble(string text, out double value)
        {
            value = 0;
            return text != null && double.TryParse(text, out value);
        }
    }
}
