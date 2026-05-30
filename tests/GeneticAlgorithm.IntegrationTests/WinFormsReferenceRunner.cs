using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.IntegrationTests
{
    /// <summary>
    /// Runs the same application services the WinForms desktop app uses (gold standard).
    /// </summary>
    internal static class WinFormsReferenceRunner
    {
        public static DumpTruckSimulation.SimulationOutput RunSimulation(SimulationRequest request) =>
            new SimulationService().Run(request);

        public static OptimizationRunResult RunGeneticAlgorithm(GeneticOptimizationRequest request) =>
            new GeneticOptimizationService().Run(request);

        public static OptimizationRunResult RunPhase(OptimizationPhase phase, GeneticOptimizationRequest request)
        {
            var service = new PhaseOptimizationService();
            switch (phase)
            {
                case OptimizationPhase.Phase1:
                    return service.RunPhase1(request);
                case OptimizationPhase.Phase2:
                    return service.RunPhase2(request);
                case OptimizationPhase.Phase3:
                    return service.RunPhase3(request);
                case OptimizationPhase.Phase4:
                    return service.RunPhase4(request);
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(phase), phase, null);
            }
        }
    }
}
