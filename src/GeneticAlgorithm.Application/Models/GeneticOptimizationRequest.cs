using System.Collections.Generic;

namespace GeneticAlgorithm.Application.Models
{
    public sealed class GeneticOptimizationRequest
    {
        public int PopulationSize { get; set; } = 20;
        public int MaxTrucks { get; set; } = 6;
        public int MaxLoaders { get; set; } = 2;
        public int MaxScalers { get; set; } = 2;
        public int Generations { get; set; } = 100;
        public double MutationRate { get; set; } = 0.01;
        public SimulationRequest Simulation { get; set; } = new SimulationRequest();
    }
}
