using System;
using GeneticSharp;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Per-gene uniform mutation matching the original optimizer behavior.
    /// </summary>
    internal sealed class FleetGeneMutation : MutationBase
    {
        private readonly double _mutationRate;

        internal FleetGeneMutation(double mutationRate)
        {
            _mutationRate = mutationRate;
        }

        protected override void PerformMutate(IChromosome chromosome, float probability)
        {
            var fleet = (FleetChromosome)chromosome;

            if (Random.Shared.NextDouble() < _mutationRate)
                fleet.MutateGene(0, fleet.MaxTrucks);

            if (Random.Shared.NextDouble() < _mutationRate)
                fleet.MutateGene(1, fleet.MaxLoaders);

            if (Random.Shared.NextDouble() < _mutationRate)
                fleet.MutateGene(2, fleet.MaxScalers);
        }
    }
}
