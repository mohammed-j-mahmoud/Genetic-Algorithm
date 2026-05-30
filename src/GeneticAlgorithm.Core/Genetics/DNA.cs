using System;
using System.Collections.Generic;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Chromosome with three integer genes (trucks, loaders, scalers) and cached simulation metrics.
    /// </summary>
    public sealed class DNA
    {
        private readonly int[] _genes = new int[3];

        /// <summary>Chromosome genes: [trucks, loaders, scalers] (read-only view).</summary>
        public IReadOnlyList<int> Genes => Array.AsReadOnly(_genes);

        /// <summary>Total project cost from last evaluation.</summary>
        public double TotalCost { get; private set; }

        /// <summary>Fitness score (higher is better).</summary>
        public double Fitness { get; private set; }

        /// <summary>Coal volume used in fitness calculation.</summary>
        public float numCoal { get; private set; }

        /// <summary>Total project days from last evaluation.</summary>
        public double TotalDays { get; private set; }

        /// <summary>Days of delay from last evaluation.</summary>
        public double DaysofDelay { get; private set; }

        /// <summary>Delay cost from last evaluation.</summary>
        public double CostofDelay { get; private set; }

        /// <summary>Truck utilization from last evaluation.</summary>
        public float utilTruck { get; private set; }

        /// <summary>Loader utilization from last evaluation.</summary>
        public float utilLoader { get; private set; }

        /// <summary>Scale utilization from last evaluation.</summary>
        public float utilScaler { get; private set; }

        internal DNA()
        {
        }

        internal void CopyFrom(FleetChromosome source, float coalVolume)
        {
            _genes[0] = source.Trucks;
            _genes[1] = source.Loaders;
            _genes[2] = source.Scalers;
            numCoal = coalVolume;
            ApplySnapshot(source.LastSnapshot);
        }

        internal void CopyFrom(int trucks, int loaders, int scalers, FitnessSnapshot snapshot, float coalVolume)
        {
            _genes[0] = trucks;
            _genes[1] = loaders;
            _genes[2] = scalers;
            numCoal = coalVolume;
            ApplySnapshot(snapshot);
        }

        private void ApplySnapshot(FitnessSnapshot snapshot)
        {
            TotalCost = snapshot.TotalCost;
            Fitness = snapshot.Fitness;
            TotalDays = snapshot.TotalDays;
            DaysofDelay = snapshot.DaysOfDelay;
            CostofDelay = snapshot.CostOfDelay;
            utilTruck = snapshot.UtilTruck;
            utilLoader = snapshot.UtilLoader;
            utilScaler = snapshot.UtilScaler;
        }

        /// <inheritdoc />
        public override string ToString() =>
            $"Number of Trucks: {_genes[0]}, Number of Loaders: {_genes[1]}, Number of Scalers: {_genes[2]}, Fitness: {Fitness}";
    }
}
