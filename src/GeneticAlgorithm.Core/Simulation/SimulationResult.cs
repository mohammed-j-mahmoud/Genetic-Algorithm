namespace GeneticAlgorithm.Core.Simulation
{
    /// <summary>
    /// Aggregated output of a dump-truck operation simulation run.
    /// </summary>
    public sealed class SimulationResult
    {
        /// <summary>Truck fleet utilization ratio (0–1).</summary>
        public float UtilTruck { get; set; }

        /// <summary>Loader utilization ratio (0–1).</summary>
        public float UtilLoader { get; set; }

        /// <summary>Scale utilization ratio (0–1).</summary>
        public float UtilScaler { get; set; }

        /// <summary>Total project duration in days.</summary>
        public double TotalDays { get; set; }

        /// <summary>Total cost including delay penalties.</summary>
        public double TotalCost { get; set; }

        /// <summary>Cost attributed to trucks.</summary>
        public double TotalCostTruck { get; set; }

        /// <summary>Cost attributed to loaders.</summary>
        public double TotalCostLoader { get; set; }

        /// <summary>Cost attributed to scales.</summary>
        public double TotalCostScaler { get; set; }

        /// <summary>Monetary cost of schedule delay.</summary>
        public double CostOfDelay { get; set; }

        /// <summary>Number of days the project exceeded the planned duration.</summary>
        public double DelayDuration { get; set; }
    }
}
