using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Desktop.ViewModels
{
    /// <summary>
    /// MVVM view model: orchestrates simulation and genetic optimization (no UI types).
    /// </summary>
    public sealed class OptimizationViewModel : ViewModelBase
    {
        private readonly SimulationService _simulationService = new SimulationService();
        private readonly GeneticOptimizationService _geneticService = new GeneticOptimizationService();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public List<KeyValuePair<int, double>> LoadingDistribution { get; } = new List<KeyValuePair<int, double>>();
        public List<KeyValuePair<int, double>> WeighingDistribution { get; } = new List<KeyValuePair<int, double>>();
        public List<KeyValuePair<int, double>> TravelingDistribution { get; } = new List<KeyValuePair<int, double>>();

        public Task<DumpTruckSimulation.SimulationOutput> RunSimulationAsync(SimulationRequest request) =>
            Task.Run(() => _simulationService.Run(request));

        public Task<OptimizationRunResult> RunGeneticAlgorithmAsync(
            GeneticOptimizationRequest request,
            IProgress<GenerationProgress> progress) =>
            Task.Run(() => _geneticService.Run(request, progress));
    }
}
