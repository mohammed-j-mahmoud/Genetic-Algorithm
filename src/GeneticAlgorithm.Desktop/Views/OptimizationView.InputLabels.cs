using System.Windows.Forms;
using GeneticAlgorithm.Desktop.Views;

namespace GeneticAlgorithm.Desktop
{
    public partial class OptimizationView
    {
        private void ApplyInputFieldLabels()
        {
            lblGaMaterialVolume.Text = InputFieldLabels.MaterialVolume;
            lblGaLoadPerTruck.Text = InputFieldLabels.LoadPerTruck;
            lblGaTruckCostPerDay.Text = InputFieldLabels.CostPerTruck;
            lblGaLoaderCostPerDay.Text = InputFieldLabels.CostPerLoader;
            lblGaScalerCostPerDay.Text = InputFieldLabels.CostPerScaler;
            lblGaProjectDuration.Text = InputFieldLabels.ProjectDuration;
            lblGaDelayCostPerDay.Text = InputFieldLabels.CostOfDelay;
            lblGaMaxTrucks.Text = InputFieldLabels.MaxTrucks;
            lblGaMaxLoaders.Text = InputFieldLabels.MaxLoaders;
            lblGaMaxScalers.Text = InputFieldLabels.MaxScalers;
            lblGaPopulation.Text = InputFieldLabels.Population;
            lblGaGenerations.Text = InputFieldLabels.LastGeneration;
            lblGaMutationRate.Text = InputFieldLabels.MutationRate;

            lblSimMaterialVolume.Text = InputFieldLabels.MaterialVolume;
            lblSimLoadPerTruck.Text = InputFieldLabels.LoadPerTruck;
            lblSimTruckCostPerDay.Text = InputFieldLabels.CostPerTruck;
            lblSimLoaderCostPerDay.Text = InputFieldLabels.CostPerLoader;
            lblSimScalerCostPerDay.Text = InputFieldLabels.CostPerScaler;
            lblSimProjectDuration.Text = InputFieldLabels.ProjectDuration;
            lblSimDelayCostPerDay.Text = InputFieldLabels.CostOfDelay;
            lblSimTruckCount.Text = InputFieldLabels.TruckCount;
            lblSimLoaderCount.Text = InputFieldLabels.LoaderCount;
            lblSimScalerCount.Text = InputFieldLabels.ScalerCount;

            lblSimProjectDurationCaption.Text = InputFieldLabels.ProjectDurationOutput;
            lblSimDaysDelayedCaption.Text = InputFieldLabels.DaysDelayed;
            lblSimTruckCostCaption.Text = InputFieldLabels.TruckCostOutput;
            lblSimLoaderCostCaption.Text = InputFieldLabels.LoaderCostOutput;
            lblSimScalerCostCaption.Text = InputFieldLabels.ScalerCostOutput;
            lblSimDelayCostCaption.Text = InputFieldLabels.DelayCostOutput;
            lblSimTotalCostCaption.Text = InputFieldLabels.TotalCostOutput;

            lblSimUtilTrucksCaption.Text = InputFieldLabels.UtilizationTrucks;
            lblSimUtilLoadersCaption.Text = InputFieldLabels.UtilizationLoaders;
            lblSimUtilScalersCaption.Text = InputFieldLabels.UtilizationScalers;
        }

        private void ApplyDistributionColumnHeaders()
        {
            ApplyDistributionColumnHeaders(gridLoadingDistribution);
            ApplyDistributionColumnHeaders(gridWeighingDistribution);
            ApplyDistributionColumnHeaders(gridTravelingDistribution);
        }

        private static void ApplyDistributionColumnHeaders(DataGridView grid)
        {
            if (grid == null || grid.Columns.Count < 2)
                return;

            grid.Columns[0].HeaderText = UiCopy.DistributionTimeColumn;
            grid.Columns[1].HeaderText = UiCopy.DistributionProbabilityColumn;
        }
    }
}
