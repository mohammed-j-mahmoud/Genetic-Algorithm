using System;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Roulette-wheel parent selection using a precomputed cumulative fitness array (Strategy pattern).
    /// </summary>
    internal static class RouletteWheelSelector
    {
        /// <summary>
        /// Selects one individual index using binary search on prefix sums — O(log n).
        /// </summary>
        /// <param name="cumulativeFitness">Prefix sums of fitness (must match population length).</param>
        /// <param name="fitnessSum">Total fitness of the population.</param>
        /// <param name="populationSize">Number of individuals.</param>
        /// <param name="random">Random source.</param>
        /// <returns>Index of the selected individual.</returns>
        public static int SelectIndex(double[] cumulativeFitness, double fitnessSum, int populationSize, Random random)
        {
            if (fitnessSum <= 0)
                return random.Next(populationSize);

            double diceRoll = random.NextDouble() * fitnessSum;

            int low = 0;
            int high = populationSize - 1;
            while (low < high)
            {
                int mid = (low + high) >> 1;
                if (cumulativeFitness[mid] < diceRoll)
                    low = mid + 1;
                else
                    high = mid;
            }

            return low;
        }
    }
}
