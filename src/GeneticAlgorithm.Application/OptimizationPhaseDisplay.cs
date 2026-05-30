using GeneticAlgorithm.Application.Models;

namespace GeneticAlgorithm.Application
{
    /// <summary>
    /// User-facing names for the truck fleet optimization UI, CLI, and API.
    /// </summary>
    public static class OptimizationPhaseDisplay
    {
        public const string ApplicationTitle = "Truck Fleet Problem";

        public const string ApiTitle = "Truck Fleet Problem API";
        public const string ApiDescription =
            "Fleet optimization service — same Simulation and search tabs as the desktop app.";

        public const string CliPrompt = "truck-fleet> ";
        public const string CliExecutableName = "truck-fleet-problem";

        public const string SimulationTab = "Simulation";
        public const string GeneticAlgorithmTab = "Genetic Algorithm";

        public const string ExhaustiveSearch = "Exhaustive Search";
        public const string GeneticSearch = "Genetic Search";
        public const string SurrogateSearch = "Surrogate Search";
        public const string DynamicProgrammingSearch = "Dynamic Programming Search";

        public const string RunDemoLabel = "Run Demo Using Default Values";

        public static string GetStrategyName(OptimizationPhase phase)
        {
            switch (phase)
            {
                case OptimizationPhase.Phase1:
                    return ExhaustiveSearch;
                case OptimizationPhase.Phase2:
                    return GeneticSearch;
                case OptimizationPhase.Phase3:
                    return SurrogateSearch;
                case OptimizationPhase.Phase4:
                    return DynamicProgrammingSearch;
                default:
                    return phase.ToString();
            }
        }
    }
}
