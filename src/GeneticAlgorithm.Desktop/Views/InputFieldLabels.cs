namespace GeneticAlgorithm.Desktop.Views
{
    /// <summary>
    /// Shared input and output captions with physical units.
    /// </summary>
    internal static class InputFieldLabels
    {
        public const string MaterialVolume = "Amount of Material (m³)";
        public const string LoadPerTruck = "Load Per Truck (m³)";
        public const string CostPerTruck = "Cost per Truck ($/day)";
        public const string CostPerLoader = "Cost per Loader ($/day)";
        public const string CostPerScaler = "Cost per Scaler ($/day)";
        public const string ProjectDuration = "Project Duration (days)";
        public const string CostOfDelay = "Cost of Delay ($/day)";

        public const string TruckCount = "Number of Trucks (count)";
        public const string LoaderCount = "Number of Loaders (count)";
        public const string ScalerCount = "Number of Scalers (count)";

        public const string MaxTrucks = "Max Trucks (count)";
        public const string MaxLoaders = "Max Loaders (count)";
        public const string MaxScalers = "Max Scalers (count)";
        public const string Population = "Population (individuals)";
        public const string LastGeneration = "Last Generation (generations)";
        public const string MutationRate = "Mutation Rate (0–1)";

        public const string ProjectDurationOutput = "Project Duration (days):";
        public const string DaysDelayed = "Days Delayed (days):";
        public const string TruckCostOutput = "Trucks Cost ($):";
        public const string LoaderCostOutput = "Loaders Cost ($):";
        public const string ScalerCostOutput = "Scalers Cost ($):";
        public const string DelayCostOutput = "Delay Cost ($):";
        public const string TotalCostOutput = "Total Cost ($):";

        public const string UtilizationTrucks = "Utilization of Trucks (ratio):";
        public const string UtilizationLoaders = "Utilization of Loaders (ratio):";
        public const string UtilizationScalers = "Utilization of Scalers (ratio):";
    }
}
