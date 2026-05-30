using System;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Immutable configuration for a genetic-algorithm run (resource bounds, population, fitness scaling).
    /// </summary>
    internal sealed class GeneticAlgorithmConfig
    {
        /// <summary>Individuals per generation.</summary>
        public int PopulationSize { get; }

        /// <summary>Maximum trucks per chromosome gene.</summary>
        public int MaxTrucks { get; }

        /// <summary>Maximum loaders per chromosome gene.</summary>
        public int MaxLoaders { get; }

        /// <summary>Maximum scales per chromosome gene.</summary>
        public int MaxScalers { get; }

        /// <summary>Material volume used in the fitness numerator.</summary>
        public float NumCoal { get; }

        /// <summary>Per-gene mutation probability.</summary>
        public double MutationRate { get; }

        /// <summary>Multiplier used by <see cref="FleetFitnessCalculator"/> (cost first, fewer days on ties).</summary>
        public double FitnessScale { get; }

        private GeneticAlgorithmConfig(
            int populationSize,
            int maxTrucks,
            int maxLoaders,
            int maxScalers,
            float numCoal,
            double mutationRate,
            double fitnessScale)
        {
            PopulationSize = populationSize;
            MaxTrucks = maxTrucks;
            MaxLoaders = maxLoaders;
            MaxScalers = maxScalers;
            NumCoal = numCoal;
            MutationRate = mutationRate;
            FitnessScale = fitnessScale;
        }

        /// <summary>
        /// Creates and validates a run configuration.
        /// </summary>
        public static GeneticAlgorithmConfig Create(
            int populationSize,
            int maxTrucks,
            int maxLoaders,
            int maxScalers,
            float numCoal,
            double mutationRate = 0.01,
            double fitnessScale = 1000.0)
        {
            if (populationSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(populationSize), "Population size must be positive.");
            if (maxTrucks <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxTrucks), "Max trucks must be positive.");
            if (maxLoaders <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxLoaders), "Max loaders must be positive.");
            if (maxScalers <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxScalers), "Max scalers must be positive.");
            if (maxTrucks > SimulationParameters.MaxResourceCount)
                throw new ArgumentOutOfRangeException(nameof(maxTrucks), $"Max trucks cannot exceed {SimulationParameters.MaxResourceCount}.");
            if (maxLoaders > SimulationParameters.MaxResourceCount)
                throw new ArgumentOutOfRangeException(nameof(maxLoaders), $"Max loaders cannot exceed {SimulationParameters.MaxResourceCount}.");
            if (maxScalers > SimulationParameters.MaxResourceCount)
                throw new ArgumentOutOfRangeException(nameof(maxScalers), $"Max scalers cannot exceed {SimulationParameters.MaxResourceCount}.");
            if (populationSize > SimulationParameters.MaxParameterValue)
                throw new ArgumentOutOfRangeException(nameof(populationSize), $"Population size cannot exceed {SimulationParameters.MaxParameterValue:N0}.");
            if (numCoal < 0)
                throw new ArgumentOutOfRangeException(nameof(numCoal), "Coal volume cannot be negative.");
            if (mutationRate < 0 || mutationRate > 1)
                throw new ArgumentOutOfRangeException(nameof(mutationRate), "Mutation rate must be between 0 and 1.");

            return new GeneticAlgorithmConfig(
                populationSize,
                maxTrucks,
                maxLoaders,
                maxScalers,
                numCoal,
                mutationRate,
                fitnessScale);
        }
    }
}
