using System.Collections.Generic;

namespace GeneticAlgorithm.Application.Models
{
    public sealed class SimulationRequest
    {
        public float CoalVolume { get; set; }
        public float TruckCount { get; set; }
        public float TruckLoadVolume { get; set; }
        public float TruckCostPerDay { get; set; }
        public float LoaderCount { get; set; }
        public float LoaderCostPerDay { get; set; }
        public float ScalerCount { get; set; }
        public float ScalerCostPerDay { get; set; }
        public float ProjectDurationDays { get; set; }
        public float DelayCostPerDay { get; set; }
        public List<KeyValuePair<int, double>> LoadingDistribution { get; set; } = new List<KeyValuePair<int, double>>();
        public List<KeyValuePair<int, double>> WeighingDistribution { get; set; } = new List<KeyValuePair<int, double>>();
        public List<KeyValuePair<int, double>> TravelingDistribution { get; set; } = new List<KeyValuePair<int, double>>();
    }
}
