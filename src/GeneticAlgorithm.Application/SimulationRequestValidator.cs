using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Application
{
    /// <summary>
    /// Validates simulation inputs before API, CLI, or optimizer runs.
    /// Mirrors <see cref="SimulationParameters"/> rules so WinForms, CLI, and API stay aligned.
    /// </summary>
    public enum SimulationValidationKind
    {
        /// <summary>Single simulation run — fleet counts are required (Simulation tab / POST /api/simulation).</summary>
        SingleRun,

        /// <summary>Search context — fleet sizes come from the optimizer; only economics and load-per-truck are required.</summary>
        SearchContext
    }

    public static class SimulationRequestValidator
    {
        private const int MaxDistributionEntries = 100;

        public static string TryValidate(SimulationRequest request) =>
            TryValidate(request, SimulationValidationKind.SingleRun);

        public static string TryValidate(SimulationRequest request, SimulationValidationKind kind)
        {
            if (request == null)
                return "simulation is required.";

            if (request.CoalVolume < 0)
                return "coalVolume cannot be negative.";
            if (request.TruckCostPerDay < 0 || request.LoaderCostPerDay < 0
                || request.ScalerCostPerDay < 0 || request.DelayCostPerDay < 0)
                return "Daily costs cannot be negative.";
            if (request.ProjectDurationDays < 0)
                return "projectDurationDays cannot be negative.";
            if (request.TruckLoadVolume <= 0)
                return "truckLoadVolume must be positive.";

            if (kind == SimulationValidationKind.SingleRun)
            {
                if (request.TruckCount <= 0)
                    return "truckCount must be positive.";
                if (request.LoaderCount < 1)
                    return "loaderCount must be at least 1.";
                if (request.ScalerCount < 1)
                    return "scalerCount must be at least 1.";
                if (request.TruckCount > SimulationParameters.MaxResourceCount
                    || request.LoaderCount > SimulationParameters.MaxResourceCount
                    || request.ScalerCount > SimulationParameters.MaxResourceCount)
                    return $"Resource counts exceed the allowed maximum ({SimulationParameters.MaxResourceCount}).";
            }

            string distributionError = ValidateDistributionCount(
                request.LoadingDistribution, "loadingDistribution", MaxDistributionEntries);
            if (distributionError != null)
                return distributionError;

            distributionError = ValidateDistributionCount(
                request.WeighingDistribution, "weighingDistribution", MaxDistributionEntries);
            if (distributionError != null)
                return distributionError;

            distributionError = ValidateDistributionCount(
                request.TravelingDistribution, "travelingDistribution", MaxDistributionEntries);
            if (distributionError != null)
                return distributionError;

            return null;
        }

        private static string ValidateDistributionCount(
            System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, double>> distribution,
            string name,
            int maxEntries)
        {
            if (distribution == null)
                return null;

            if (distribution.Count > maxEntries)
                return $"{name} cannot exceed {maxEntries} entries.";

            return null;
        }
    }
}
