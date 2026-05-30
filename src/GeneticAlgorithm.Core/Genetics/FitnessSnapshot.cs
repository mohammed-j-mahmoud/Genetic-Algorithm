using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Genetics
{
    internal readonly struct FitnessSnapshot
    {
        public double TotalCost { get; }
        public double Fitness { get; }
        public double TotalDays { get; }
        public double DaysOfDelay { get; }
        public double CostOfDelay { get; }
        public float UtilTruck { get; }
        public float UtilLoader { get; }
        public float UtilScaler { get; }

        private FitnessSnapshot(
            double totalCost,
            double fitness,
            double totalDays,
            double daysOfDelay,
            double costOfDelay,
            float utilTruck,
            float utilLoader,
            float utilScaler)
        {
            TotalCost = totalCost;
            Fitness = fitness;
            TotalDays = totalDays;
            DaysOfDelay = daysOfDelay;
            CostOfDelay = costOfDelay;
            UtilTruck = utilTruck;
            UtilLoader = utilLoader;
            UtilScaler = utilScaler;
        }

        public static FitnessSnapshot FromSimulation(
            DumpTruckSimulation.SimulationOutput simulation,
            float coalVolume,
            double fitnessScale)
        {
            double totalCost = simulation.TotalCost;
            return new FitnessSnapshot(
                totalCost,
                FleetFitnessCalculator.Compute(coalVolume, fitnessScale, totalCost, simulation.TotalDays),
                simulation.TotalDays,
                simulation.DelayDays,
                simulation.DelayCost,
                simulation.TruckUtilization,
                simulation.LoaderUtilization,
                simulation.ScalerUtilization);
        }

        /// <summary>Placeholder snapshot when only surrogate fitness is known before verification.</summary>
        internal static FitnessSnapshot FromRank(double fitness, float coalVolume, double fitnessScale) =>
            new FitnessSnapshot(0, fitness, 0, 0, 0, 0, 0, 0);
    }
}
