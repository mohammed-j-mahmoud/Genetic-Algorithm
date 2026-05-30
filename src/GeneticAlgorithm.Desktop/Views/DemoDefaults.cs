namespace GeneticAlgorithm.Desktop.Views
{
    /// <summary>
    /// Shared demo values for GA and Simulation "run with default values" actions.
    /// </summary>
    internal static class DemoDefaults
    {
        public const float MaterialVolume = 10000f;
        public const float TruckLoadVolume = 20f;
        public const float TruckCostPerDay = 1000f;
        public const float LoaderCostPerDay = 2000f;
        public const float ScalerCostPerDay = 3000f;
        public const float ProjectDurationDays = 120f;
        public const float DelayCostPerDay = 10000f;

        public const int DemoTrucks = 6;
        public const int DemoLoaders = 2;
        public const int DemoScalers = 2;

        public const int PopulationSize = 20;
        public const int Generations = 100;
        public const double MutationRate = 0.01;
    }
}
