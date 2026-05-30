using System;
using GeneticSharp;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Three-gene chromosome: trucks, loaders, and scalers within configured bounds.
    /// </summary>
    internal sealed class FleetChromosome : ChromosomeBase
    {
        private readonly int _maxTrucks;
        private readonly int _maxLoaders;
        private readonly int _maxScalers;

        internal FleetChromosome(int maxTrucks, int maxLoaders, int maxScalers)
            : base(3)
        {
            if (maxTrucks <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxTrucks));
            if (maxLoaders <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxLoaders));
            if (maxScalers <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxScalers));

            _maxTrucks = maxTrucks;
            _maxLoaders = maxLoaders;
            _maxScalers = maxScalers;
            CreateGenes();
        }

        internal int MaxTrucks => _maxTrucks;

        internal int MaxLoaders => _maxLoaders;

        internal int MaxScalers => _maxScalers;

        internal int Trucks => ReadGene(0, _maxTrucks);

        internal int Loaders => ReadGene(1, _maxLoaders);

        internal int Scalers => ReadGene(2, _maxScalers);

        internal FitnessSnapshot LastSnapshot { get; private set; }

        internal void SetLastSnapshot(FitnessSnapshot snapshot) => LastSnapshot = snapshot;

        public override Gene GenerateGene(int geneIndex)
        {
            switch (geneIndex)
            {
                case 0:
                    return new Gene(Random.Shared.Next(1, _maxTrucks + 1));
                case 1:
                    return new Gene(Random.Shared.Next(1, _maxLoaders + 1));
                case 2:
                    return new Gene(Random.Shared.Next(1, _maxScalers + 1));
                default:
                    throw new ArgumentOutOfRangeException(nameof(geneIndex));
            }
        }

        public override IChromosome CreateNew() =>
            new FleetChromosome(_maxTrucks, _maxLoaders, _maxScalers);

        internal void MutateGene(int geneIndex, int maxValue)
        {
            ReplaceGene(geneIndex, new Gene(Random.Shared.Next(1, maxValue + 1)));
        }

        private int ReadGene(int geneIndex, int maxValue)
        {
            int value = Convert.ToInt32(GetGene(geneIndex).Value);
            if (value < 1)
                return 1;
            if (value > maxValue)
                return maxValue;
            return value;
        }
    }
}
