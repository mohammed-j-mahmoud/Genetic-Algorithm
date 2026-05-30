using GeneticSharp;

namespace GeneticAlgorithm.Core.Genetics
{
    /// <summary>
    /// Evaluates fleet chromosomes via the simulation fitness cache.
    /// </summary>
    internal sealed class FleetSimulationFitness : IFitness
    {
        private readonly SimulationFitnessCache _cache;

        internal FleetSimulationFitness(SimulationFitnessCache cache)
        {
            _cache = cache;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var fleet = (FleetChromosome)chromosome;
            FitnessSnapshot snapshot = _cache.GetOrEvaluate(fleet.Trucks, fleet.Loaders, fleet.Scalers);
            fleet.SetLastSnapshot(snapshot);
            return snapshot.Fitness;
        }
    }
}
