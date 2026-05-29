namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Discrete states in the truck loading / weighing / traveling cycle.
    /// </summary>
    public enum TruckState
    {
        /// <summary>Truck is being loaded.</summary>
        Loading = 0,

        /// <summary>Truck is on the scale.</summary>
        Weighing = 1,

        /// <summary>Truck is traveling to the dump site.</summary>
        Traveling = 2
    }
}
