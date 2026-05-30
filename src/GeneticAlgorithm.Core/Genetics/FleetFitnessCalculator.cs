using System;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Converts simulation cost and duration into a single fitness score (higher is better).
    /// Minimizes total cost first; when costs tie, favors fewer project days.
    /// </summary>
    internal static class FleetFitnessCalculator
    {
        private const double CostOffset = 1.0;
        private const double DaysOffset = 1.0;
        // +2 keeps the days tie-break smaller than a one-dollar primary fitness step at any cost >= 0.
        private const double TieBreakCostOffset = 2.0;

        /// <summary>
        /// Higher is better. Primary term minimizes cost; secondary term breaks ties by minimizing days
        /// without ever outweighing a one-dollar cost difference at any realistic project cost.
        /// </summary>
        internal static double Compute(float coalVolume, double fitnessScale, double totalCost, double totalDays)
        {
            if (totalCost < 0 || totalDays < 0)
                return 0;

            double primary = fitnessScale * coalVolume / (totalCost + CostOffset);
            return primary * (1.0 + 1.0 / ((totalDays + DaysOffset) * (totalCost + TieBreakCostOffset)));
        }
    }
}
