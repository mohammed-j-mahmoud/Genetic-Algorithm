using System;
using System.Collections.Generic;

namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Immutable inputs for a dump-truck simulation run.
    /// </summary>
    public sealed class SimulationParameters
    {
        /// <summary>Maximum trucks, loaders, or scales allowed per simulation run.</summary>
        public const int MaxResourceCount = 10_000;

        /// <summary>Total coal volume to move (m³).</summary>
        public float CoalVolume { get; }

        /// <summary>Number of trucks in the fleet.</summary>
        public float TruckCount { get; }

        /// <summary>Coal volume per truck load (m³).</summary>
        public float TruckLoadVolume { get; }

        /// <summary>Daily cost per truck.</summary>
        public float TruckCostPerDay { get; }

        /// <summary>Number of loaders.</summary>
        public float LoaderCount { get; }

        /// <summary>Daily cost per loader.</summary>
        public float LoaderCostPerDay { get; }

        /// <summary>Number of scales.</summary>
        public float ScalerCount { get; }

        /// <summary>Daily cost per scale.</summary>
        public float ScalerCostPerDay { get; }

        /// <summary>Planned project duration in days.</summary>
        public float ProjectDurationDays { get; }

        /// <summary>Penalty cost per day of delay.</summary>
        public float DelayCostPerDay { get; }

        /// <summary>Precomputed loading-time distribution.</summary>
        internal DiscreteDistribution LoadingDistribution { get; }

        /// <summary>Precomputed weighing-time distribution.</summary>
        internal DiscreteDistribution WeighingDistribution { get; }

        /// <summary>Precomputed travel-time distribution.</summary>
        internal DiscreteDistribution TravelingDistribution { get; }

        /// <summary>
        /// Creates simulation parameters from UI or optimizer inputs.
        /// </summary>
        public SimulationParameters(
            float coalVolume,
            float truckCount,
            float truckLoadVolume,
            float truckCostPerDay,
            float loaderCount,
            float loaderCostPerDay,
            float scalerCount,
            float scalerCostPerDay,
            float projectDurationDays,
            float delayCostPerDay,
            IReadOnlyList<KeyValuePair<int, double>> loadingDistribution,
            IReadOnlyList<KeyValuePair<int, double>> weighingDistribution,
            IReadOnlyList<KeyValuePair<int, double>> travelingDistribution)
        {
            CoalVolume = coalVolume;
            TruckCount = truckCount;
            TruckLoadVolume = truckLoadVolume;
            TruckCostPerDay = truckCostPerDay;
            LoaderCount = loaderCount;
            LoaderCostPerDay = loaderCostPerDay;
            ScalerCount = scalerCount;
            ScalerCostPerDay = scalerCostPerDay;
            ProjectDurationDays = projectDurationDays;
            DelayCostPerDay = delayCostPerDay;

            LoadingDistribution = DiscreteDistribution.FromEntries(loadingDistribution, DiscreteDistribution.DefaultLoading);
            WeighingDistribution = DiscreteDistribution.FromEntries(weighingDistribution, DiscreteDistribution.DefaultWeighing);
            TravelingDistribution = DiscreteDistribution.FromEntries(travelingDistribution, DiscreteDistribution.DefaultTraveling);

            Validate();
        }

        /// <summary>Number of trucks as a positive integer fleet size.</summary>
        public int TruckFleetSize => Math.Max(1, (int)TruckCount);

        private void Validate()
        {
            if (CoalVolume < 0)
                throw new ArgumentOutOfRangeException(nameof(CoalVolume), "Coal volume cannot be negative.");
            if (TruckCostPerDay < 0 || LoaderCostPerDay < 0 || ScalerCostPerDay < 0 || DelayCostPerDay < 0)
                throw new ArgumentOutOfRangeException("Daily costs cannot be negative.");
            if (ProjectDurationDays < 0)
                throw new ArgumentOutOfRangeException(nameof(ProjectDurationDays), "Project duration cannot be negative.");
            if (TruckCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(TruckCount), "Truck count must be positive.");
            if (TruckLoadVolume <= 0)
                throw new ArgumentOutOfRangeException(nameof(TruckLoadVolume), "Truck load volume must be positive.");
            if (LoaderCount < 1)
                throw new ArgumentOutOfRangeException(nameof(LoaderCount), "Loader count must be at least 1.");
            if (ScalerCount < 1)
                throw new ArgumentOutOfRangeException(nameof(ScalerCount), "Scaler count must be at least 1.");
            if (TruckFleetSize > MaxResourceCount || LoaderCount > MaxResourceCount || ScalerCount > MaxResourceCount)
                throw new ArgumentOutOfRangeException($"Resource counts exceed the allowed maximum ({MaxResourceCount}).");
        }
    }
}
