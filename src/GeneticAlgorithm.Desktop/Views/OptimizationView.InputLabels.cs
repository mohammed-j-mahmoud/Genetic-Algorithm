using GeneticAlgorithm.Desktop.Views;

namespace GeneticAlgorithm.Desktop
{
    public partial class OptimizationView
    {
        private void ApplyInputFieldLabels()
        {
            label16.Text = InputFieldLabels.MaterialVolume;
            label19.Text = InputFieldLabels.LoadPerTruck;
            label20.Text = InputFieldLabels.CostPerTruck;
            label22.Text = InputFieldLabels.CostPerLoader;
            label21.Text = InputFieldLabels.CostPerScaler;
            label18.Text = InputFieldLabels.ProjectDuration;
            label17.Text = InputFieldLabels.CostOfDelay;
            label15.Text = InputFieldLabels.MaxTrucks;
            label7.Text = InputFieldLabels.MaxLoaders;
            label6.Text = InputFieldLabels.MaxScalers;
            label2.Text = InputFieldLabels.Population;
            label1.Text = InputFieldLabels.LastGeneration;
            label5.Text = InputFieldLabels.MutationRate;

            label12.Text = InputFieldLabels.MaterialVolume;
            label23.Text = InputFieldLabels.LoadPerTruck;
            label24.Text = InputFieldLabels.CostPerTruck;
            label26.Text = InputFieldLabels.CostPerLoader;
            label25.Text = InputFieldLabels.CostPerScaler;
            label14.Text = InputFieldLabels.ProjectDuration;
            label13.Text = InputFieldLabels.CostOfDelay;
            label11.Text = InputFieldLabels.TruckCount;
            label10.Text = InputFieldLabels.LoaderCount;
            label9.Text = InputFieldLabels.ScalerCount;

            label30.Text = InputFieldLabels.ProjectDurationOutput;
            label44.Text = InputFieldLabels.DaysDelayed;
            label27.Text = InputFieldLabels.TruckCostOutput;
            label34.Text = InputFieldLabels.LoaderCostOutput;
            label33.Text = InputFieldLabels.ScalerCostOutput;
            label29.Text = InputFieldLabels.DelayCostOutput;
            label45.Text = InputFieldLabels.TotalCostOutput;

            label28.Text = InputFieldLabels.UtilizationTrucks;
            label31.Text = InputFieldLabels.UtilizationLoaders;
            label32.Text = InputFieldLabels.UtilizationScalers;
        }

        private void ApplyDistributionColumnHeaders()
        {
            dataGridView1.Columns[0].HeaderText = "Time (min)";
            dataGridView2.Columns[0].HeaderText = "Time (min)";
            dataGridView3.Columns[0].HeaderText = "Time (min)";
            dataGridView1.Columns[1].HeaderText = "Probability";
            dataGridView2.Columns[1].HeaderText = "Probability";
            dataGridView3.Columns[1].HeaderText = "Probability";
        }
    }
}
