using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;

namespace GeneticAlgorithm.Desktop
{
    public partial class OptimizationView : Form
    {
        private const int ChartUpdateInterval = 5;

        private sealed class GaGenerationReport
        {
            public int Generation { get; set; }
            public double Fitness { get; set; }
            public bool UpdateChart { get; set; }
        }

        private sealed class GaRunParameters
        {
            public int PopulationSize { get; set; }
            public float NumCoal { get; set; }
            public float NumTruckLoad { get; set; }
            public float CostTruckPerDay { get; set; }
            public float CostLoaderPerDay { get; set; }
            public float CostScalerPerDay { get; set; }
            public float ProjectDuration { get; set; }
            public float CostDelayPerDay { get; set; }
            public int MaxTrucks { get; set; }
            public int MaxLoaders { get; set; }
            public int MaxScalers { get; set; }
            public int LastGeneration { get; set; }
            public double MutationRate { get; set; }
            public List<KeyValuePair<int, double>> LoadingElements { get; set; }
            public List<KeyValuePair<int, double>> WeighingElements { get; set; }
            public List<KeyValuePair<int, double>> TravelingElements { get; set; }
        }

        List<KeyValuePair<int, double>> loadingElements = new List<KeyValuePair<int, double>>();
        List<KeyValuePair<int, double>> weighingElements = new List<KeyValuePair<int, double>>();
        List<KeyValuePair<int, double>> travelingElements = new List<KeyValuePair<int, double>>();
        private readonly ViewModels.OptimizationViewModel _viewModel = new ViewModels.OptimizationViewModel();
        DumpTruckSimulation.SimulationOutput SimulationResults;
        private bool _gaRunning;

        public OptimizationView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// this button is made to rn the GA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TryReadGaRunParameters(out GaRunParameters parameters))
                    return;

                await RunGeneticAlgorithmAsync(parameters).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool TryReadGaRunParameters(out GaRunParameters parameters, bool showErrors = true)
        {
            parameters = null;

            if (!FormInputParser.TryParseInt(txtbxPopulationNo.Text, "Population size", out int populationSize, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxmaterialAlgo.Text, "Material volume", out float numCoal, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxlodrprtrukAlgo.Text, "Truck load", out float numTruckLoad, showErrors) || numTruckLoad <= 0f)
            {
                if (showErrors)
                    MessageBox.Show("Truck load must be greater than zero.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtbxcostprtrukAlgo.Text, "Truck cost per day", out float costTruckPerDay, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxCostPrLoderAlgo.Text, "Loader cost per day", out float costLoaderPerDay, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxCostPrScalerAlgo.Text, "Scaler cost per day", out float costScalerPerDay, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxProjectDurationAlgo.Text, "Project duration", out float projectDuration, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxCostOfDelayAlgo.Text, "Cost of delay", out float costDelayPerDay, showErrors))
                return false;
            if (!FormInputParser.TryParseInt(txtbxTrukNoMxAlgo.Text, "Max trucks", out int maxTrucks, showErrors))
                return false;
            if (!FormInputParser.TryParseInt(txtbxScalerNoMxAlgo.Text, "Max scalers", out int maxScalers, showErrors))
                return false;
            if (!FormInputParser.TryParseInt(txtbxLosderNoMxAlgo.Text, "Max loaders", out int maxLoaders, showErrors))
                return false;
            if (!FormInputParser.TryParseInt(txtbxGenerationNo.Text, "Generation count", out int lastGeneration, showErrors))
                return false;
            if (lastGeneration > 100_000)
            {
                if (showErrors)
                    MessageBox.Show("Generation count cannot exceed 100,000.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseMutationRate(txtbxMutationRate.Text, out double mutationRate, showErrors))
                return false;
            if (maxTrucks > SimulationParameters.MaxResourceCount
                || maxLoaders > SimulationParameters.MaxResourceCount
                || maxScalers > SimulationParameters.MaxResourceCount)
            {
                if (showErrors)
                    MessageBox.Show($"Max trucks, loaders, and scalers cannot exceed {SimulationParameters.MaxResourceCount}.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (populationSize > 10_000)
            {
                if (showErrors)
                    MessageBox.Show("Population size cannot exceed 10,000.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            parameters = new GaRunParameters
            {
                PopulationSize = populationSize,
                NumCoal = numCoal,
                NumTruckLoad = numTruckLoad,
                CostTruckPerDay = costTruckPerDay,
                CostLoaderPerDay = costLoaderPerDay,
                CostScalerPerDay = costScalerPerDay,
                ProjectDuration = projectDuration,
                CostDelayPerDay = costDelayPerDay,
                MaxTrucks = maxTrucks,
                MaxScalers = maxScalers,
                MaxLoaders = maxLoaders,
                LastGeneration = lastGeneration,
                MutationRate = mutationRate,
                LoadingElements = new List<KeyValuePair<int, double>>(loadingElements),
                WeighingElements = new List<KeyValuePair<int, double>>(weighingElements),
                TravelingElements = new List<KeyValuePair<int, double>>(travelingElements)
            };
            return true;
        }

        private static OptimizationRunResult RunGeneticAlgorithmCore(
            GaRunParameters parameters,
            IProgress<GaGenerationReport> progress)
        {
            var request = new GeneticOptimizationRequest
            {
                PopulationSize = parameters.PopulationSize,
                MaxTrucks = parameters.MaxTrucks,
                MaxLoaders = parameters.MaxLoaders,
                MaxScalers = parameters.MaxScalers,
                Generations = parameters.LastGeneration,
                MutationRate = parameters.MutationRate,
                Simulation = new SimulationRequest
                {
                    CoalVolume = parameters.NumCoal,
                    TruckLoadVolume = parameters.NumTruckLoad,
                    TruckCostPerDay = parameters.CostTruckPerDay,
                    LoaderCostPerDay = parameters.CostLoaderPerDay,
                    ScalerCostPerDay = parameters.CostScalerPerDay,
                    ProjectDurationDays = parameters.ProjectDuration,
                    DelayCostPerDay = parameters.CostDelayPerDay,
                    LoadingDistribution = parameters.LoadingElements,
                    WeighingDistribution = parameters.WeighingElements,
                    TravelingDistribution = parameters.TravelingElements
                }
            };

            return new GeneticOptimizationService().Run(request, new Progress<GenerationProgress>(p =>
            {
                int generation = p.Generation;
                bool updateChart = generation == 1
                    || generation == parameters.LastGeneration
                    || generation % ChartUpdateInterval == 0;

                progress?.Report(new GaGenerationReport
                {
                    Generation = generation,
                    Fitness = p.BestFitness,
                    UpdateChart = updateChart
                });
            }));
        }

        private async Task RunGeneticAlgorithmAsync(GaRunParameters parameters)
        {
            if (_gaRunning)
                return;

            _gaRunning = true;
            SetGeneticAlgorithmControlsEnabled(false);

            chart1.Series["Series1"].Points.Clear();
            progressBar1.Visible = true;
            progressBar1.Minimum = 1;
            progressBar1.Maximum = parameters.LastGeneration;
            progressBar1.Value = 1;

            var progress = new Progress<GaGenerationReport>(report =>
            {
                if (IsDisposed)
                    return;

                int generation = Math.Min(report.Generation, progressBar1.Maximum);
                progressBar1.Value = Math.Max(progressBar1.Minimum, generation);

                if (report.UpdateChart)
                    chart1.Series["Series1"].Points.AddXY(report.Generation, report.Fitness);
            });

            try
            {
                OptimizationRunResult results = await Task.Run(
                    () => RunGeneticAlgorithmCore(parameters, progress)).ConfigureAwait(true);

                progressBar1.Value = progressBar1.Maximum;
                ApplyGeneticAlgorithmResults(results);
                MessageBox.Show("GA is Done ", "Message Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                SetGeneticAlgorithmControlsEnabled(true);
                _gaRunning = false;
            }
        }

        private void ApplyGeneticAlgorithmResults(OptimizationRunResult results)
        {
            lblTruckNoAlgo.Text = results.BestChromosome.Genes[0].ToString();
            lblLoaderNoAlgo.Text = results.BestChromosome.Genes[1].ToString();
            lblScalerNoAlgo.Text = results.BestChromosome.Genes[2].ToString();
            lblUtiliLoaderAlgo.Text = Math.Round(results.BestChromosome.utilLoader, 3).ToString();
            lblUtiliScalerAlgo.Text = Math.Round(results.BestChromosome.utilScaler, 3).ToString();
            lblUtiliTruckAlgo.Text = Math.Round(results.BestChromosome.utilTruck, 3).ToString();
            lblDaysDelayedAlgo.Text = results.BestChromosome.DaysofDelay.ToString();
            lblCostodDelayAlgo.Text = results.BestChromosome.CostofDelay.ToString();
            lblTotalDaysAlgo.Text = results.BestChromosome.TotalDays.ToString();
            lblTotalCostAlgo.Text = results.BestChromosome.TotalCost.ToString();
        }

        private void SetGeneticAlgorithmControlsEnabled(bool enabled)
        {
            btnRunGenetic.Enabled = enabled;
            button1.Enabled = enabled;
            UseWaitCursor = !enabled;
        }

        private void OptimizationView_Load(object sender, EventArgs e)
        {
            picbxalgo.Enabled = true;
            dataGridView1.Columns[0].Name = "Time";
            dataGridView2.Columns[0].Name = "Time";
            dataGridView3.Columns[0].Name = "Time";
            dataGridView1.Columns[1].Name = "Probability";
            dataGridView2.Columns[1].Name = "Probability";
            dataGridView3.Columns[1].Name = "Probability";
         }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                loadingElements = DistributionGridReader.Read(dataGridView1, "Loading");
                weighingElements = DistributionGridReader.Read(dataGridView2, "Weighing");
                travelingElements = DistributionGridReader.Read(dataGridView3, "Traveling");
                _viewModel.LoadingDistribution.Clear();
                _viewModel.LoadingDistribution.AddRange(loadingElements);
                _viewModel.WeighingDistribution.Clear();
                _viewModel.WeighingDistribution.AddRange(weighingElements);
                _viewModel.TravelingDistribution.Clear();
                _viewModel.TravelingDistribution.AddRange(travelingElements);
                MessageBox.Show("Distributions saved and normalized to sum to 1.", "Simulation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invalid distribution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            grid.Rows[e.RowIndex].ErrorText = "";   
            if (e.ColumnIndex == 0)
            {
                int newInteger;
                if (grid.Rows[e.RowIndex].IsNewRow) { return; }
                if (!int.TryParse(e.FormattedValue.ToString(), out newInteger) || newInteger <= 0)
                {
                    e.Cancel = true;
                    grid.Rows[e.RowIndex].ErrorText = "the value must be a positive integer";
                }
                if (grid.Rows[e.RowIndex].Cells[1].Value == null)
                {
                    grid.AllowUserToAddRows = false;
                }
                else
                {
                    grid.AllowUserToAddRows = true;
                }
            }
            if (e.ColumnIndex == 1)
            {
                double newDouble;
                double sum = 0;
                if (grid.Rows[e.RowIndex].IsNewRow) { return; }
                if (!double.TryParse(Convert.ToString(e.FormattedValue), out newDouble) || newDouble <= 0)
                {
                    e.Cancel = true;
                    grid.Rows[e.RowIndex].ErrorText = "the value must be a non zero positive double";
                }
                else
                {
                    for (int i = 0; i < grid.Rows.Count - 1; i++)
                    {
                        if (grid.Rows[i].Cells[1].Value == null)
                        {
                            continue;
                        }
                        if (i == e.RowIndex)
                        {
                            sum += Convert.ToDouble(e.FormattedValue);

                        }
                        else if (FormInputParser.TryParseGridCellDouble(grid.Rows[i].Cells[1].Value, out double rowWeight))
                        {
                            sum += rowWeight;
                        }
                    }
                    if (sum > 1)
                    {
                        e.Cancel = true;
                        grid.Rows[e.RowIndex].ErrorText = "the total probability must be less than 1";
                    }
                }
                if (grid.Rows[e.RowIndex].Cells[0].Value == null)
                {
                    grid.AllowUserToAddRows = false;
                }
                else
                {
                    grid.AllowUserToAddRows = true;
                }
            }
        }
        /// <summary>
        /// this is simulation button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TryReadSimulationInputs(out SimulationRunInputs inputs))
                    return;

                btnSim.Enabled = false;
                UseWaitCursor = true;

                SimulationResults = await Task.Run(() => new SimulationService().Run(new SimulationRequest
                {
                    CoalVolume = inputs.NumCoal,
                    TruckCount = inputs.NumTruck,
                    TruckLoadVolume = inputs.NumTruckLoad,
                    TruckCostPerDay = inputs.CostTruckPerDay,
                    LoaderCount = inputs.NumLoader,
                    LoaderCostPerDay = inputs.CostLoaderPerDay,
                    ScalerCount = inputs.NumScaler,
                    ScalerCostPerDay = inputs.CostScalerPerDay,
                    ProjectDurationDays = inputs.ProjectDuration,
                    DelayCostPerDay = inputs.CostOfDelay,
                    LoadingDistribution = loadingElements,
                    WeighingDistribution = weighingElements,
                    TravelingDistribution = travelingElements
                })).ConfigureAwait(true);

                ApplySimulationResults(SimulationResults);
                MessageBox.Show("Simulation is Done ", "Message Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnSim.Enabled = true;
                UseWaitCursor = false;
            }
        }

        private sealed class SimulationRunInputs
        {
            public float NumCoal { get; set; }
            public float NumTruck { get; set; }
            public float NumTruckLoad { get; set; }
            public float CostTruckPerDay { get; set; }
            public float NumLoader { get; set; }
            public float CostLoaderPerDay { get; set; }
            public float NumScaler { get; set; }
            public float CostScalerPerDay { get; set; }
            public float ProjectDuration { get; set; }
            public float CostOfDelay { get; set; }
        }

        private bool TryReadSimulationInputs(out SimulationRunInputs inputs)
        {
            inputs = new SimulationRunInputs();

            if (!FormInputParser.TryParseFloat(txtbxmaterialsim.Text, "Material volume", out float numCoal))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxTrukNO.Text, "Truck count", out float numTruck) || numTruck <= 0f)
            {
                MessageBox.Show("Truck count must be greater than zero.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtbxlodrprtruk.Text, "Truck load", out float numTruckLoad) || numTruckLoad <= 0f)
            {
                MessageBox.Show("Truck load must be greater than zero.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtbxcostprtruk.Text, "Truck cost per day", out float costTruckPerDay))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxLoaderNo.Text, "Loader count", out float numLoader) || numLoader < 1f)
            {
                MessageBox.Show("Loader count must be at least 1.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtbxCostPrLoder.Text, "Loader cost per day", out float costLoaderPerDay))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxScalerNo.Text, "Scaler count", out float numScaler) || numScaler < 1f)
            {
                MessageBox.Show("Scaler count must be at least 1.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtbxCostPrScaler.Text, "Scaler cost per day", out float costScalerPerDay))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxProjectDuration.Text, "Project duration", out float projectDuration))
                return false;
            if (!FormInputParser.TryParseFloat(txtbxCostOfDelay.Text, "Cost of delay", out float costOfDelay))
                return false;

            inputs.NumCoal = numCoal;
            inputs.NumTruck = numTruck;
            inputs.NumTruckLoad = numTruckLoad;
            inputs.CostTruckPerDay = costTruckPerDay;
            inputs.NumLoader = numLoader;
            inputs.CostLoaderPerDay = costLoaderPerDay;
            inputs.NumScaler = numScaler;
            inputs.CostScalerPerDay = costScalerPerDay;
            inputs.ProjectDuration = projectDuration;
            inputs.CostOfDelay = costOfDelay;
            return true;
        }

        private void ApplySimulationResults(DumpTruckSimulation.SimulationOutput results)
        {
            chart2.Series["Actual Cost"].Points.Clear();
            chart2.Series["Delay Cost"].Points.Clear();

            lblLoaderCost.Text = results.LoaderCost.ToString();
            lblScalerCost.Text = results.ScalerCost.ToString();
            lblTruckCost.Text = results.TruckCost.ToString();
            lblDelayCost.Text = results.DelayCost.ToString();
            lblprojectDuration.Text = results.TotalDays.ToString();
            lbltotalcost.Text = results.TotalCost.ToString();
            lbldaysofdelayed.Text = results.DelayDays.ToString();
            lblUtilScaler.Text = Math.Round(results.ScalerUtilization, 3).ToString();
            UtilLoader.Text = Math.Round(results.LoaderUtilization, 3).ToString();
            UtilTrucks.Text = Math.Round(results.TruckUtilization, 3).ToString();

            chart2.Series["Actual Cost"].Points.AddXY(0, results.TotalCost);
            chart2.Series["Delay Cost"].Points.AddXY(1, results.DelayCost);
            chart2.Series["Actual Cost"].BorderWidth = 3;
            chart2.Series["Delay Cost"].BorderWidth = 3;
            chart2.ChartAreas[0].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chart2.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
        }
        private static GaRunParameters CreateDemoGaRunParameters() =>
            new GaRunParameters
            {
                PopulationSize = 20,
                NumCoal = 10000,
                NumTruckLoad = 20,
                CostTruckPerDay = 1000,
                CostLoaderPerDay = 2000,
                CostScalerPerDay = 3000,
                ProjectDuration = 120,
                CostDelayPerDay = 10000,
                MaxTrucks = 6,
                MaxLoaders = 2,
                MaxScalers = 2,
                LastGeneration = 100,
                MutationRate = 0.01,
                LoadingElements = CreateDefaultLoadingDistribution(),
                WeighingElements = CreateDefaultWeighingDistribution(),
                TravelingElements = CreateDefaultTravelingDistribution()
            };

        private static List<KeyValuePair<int, double>> CreateDefaultLoadingDistribution() =>
            new List<KeyValuePair<int, double>>
            {
                new KeyValuePair<int, double>(5, 0.3),
                new KeyValuePair<int, double>(10, 0.5),
                new KeyValuePair<int, double>(15, 0.2)
            };

        private static List<KeyValuePair<int, double>> CreateDefaultWeighingDistribution() =>
            new List<KeyValuePair<int, double>>
            {
                new KeyValuePair<int, double>(12, 0.7),
                new KeyValuePair<int, double>(16, 0.3)
            };

        private static List<KeyValuePair<int, double>> CreateDefaultTravelingDistribution() =>
            new List<KeyValuePair<int, double>>
            {
                new KeyValuePair<int, double>(40, 0.4),
                new KeyValuePair<int, double>(60, 0.3),
                new KeyValuePair<int, double>(80, 0.2),
                new KeyValuePair<int, double>(100, 0.1)
            };

        private void ApplyGaRunParametersToForm(GaRunParameters parameters)
        {
            txtbxPopulationNo.Text = parameters.PopulationSize.ToString(CultureInfo.InvariantCulture);
            txtbxmaterialAlgo.Text = parameters.NumCoal.ToString(CultureInfo.InvariantCulture);
            txtbxlodrprtrukAlgo.Text = parameters.NumTruckLoad.ToString(CultureInfo.InvariantCulture);
            txtbxcostprtrukAlgo.Text = parameters.CostTruckPerDay.ToString(CultureInfo.InvariantCulture);
            txtbxCostPrLoderAlgo.Text = parameters.CostLoaderPerDay.ToString(CultureInfo.InvariantCulture);
            txtbxCostPrScalerAlgo.Text = parameters.CostScalerPerDay.ToString(CultureInfo.InvariantCulture);
            txtbxProjectDurationAlgo.Text = parameters.ProjectDuration.ToString(CultureInfo.InvariantCulture);
            txtbxCostOfDelayAlgo.Text = parameters.CostDelayPerDay.ToString(CultureInfo.InvariantCulture);
            txtbxTrukNoMxAlgo.Text = parameters.MaxTrucks.ToString(CultureInfo.InvariantCulture);
            txtbxLosderNoMxAlgo.Text = parameters.MaxLoaders.ToString(CultureInfo.InvariantCulture);
            txtbxScalerNoMxAlgo.Text = parameters.MaxScalers.ToString(CultureInfo.InvariantCulture);
            txtbxGenerationNo.Text = parameters.LastGeneration.ToString(CultureInfo.InvariantCulture);
            txtbxMutationRate.Text = parameters.MutationRate.ToString(CultureInfo.InvariantCulture);

            loadingElements = new List<KeyValuePair<int, double>>(parameters.LoadingElements);
            weighingElements = new List<KeyValuePair<int, double>>(parameters.WeighingElements);
            travelingElements = new List<KeyValuePair<int, double>>(parameters.TravelingElements);

            PopulateDistributionGrid(dataGridView1, loadingElements);
            PopulateDistributionGrid(dataGridView2, weighingElements);
            PopulateDistributionGrid(dataGridView3, travelingElements);

            _viewModel.LoadingDistribution.Clear();
            _viewModel.LoadingDistribution.AddRange(loadingElements);
            _viewModel.WeighingDistribution.Clear();
            _viewModel.WeighingDistribution.AddRange(weighingElements);
            _viewModel.TravelingDistribution.Clear();
            _viewModel.TravelingDistribution.AddRange(travelingElements);
        }

        private static void PopulateDistributionGrid(DataGridView grid, IReadOnlyList<KeyValuePair<int, double>> entries)
        {
            grid.Rows.Clear();
            for (int i = 0; i < entries.Count; i++)
            {
                grid.Rows.Add(
                    entries[i].Key.ToString(CultureInfo.InvariantCulture),
                    entries[i].Value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// this button is made to run GA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                GaRunParameters parameters = CreateDemoGaRunParameters();
                ApplyGaRunParametersToForm(parameters);
                await RunGeneticAlgorithmAsync(parameters).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// this is the clear button for the simulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtbxmaterialsim.Clear();
            txtbxTrukNO.Clear();
            txtbxlodrprtruk.Clear();
            txtbxcostprtruk.Clear();
            txtbxLoaderNo.Clear();
            txtbxCostPrLoder.Clear();
            txtbxScalerNo.Clear();
            txtbxCostPrScaler.Clear();
            txtbxProjectDuration.Clear(); ;
            txtbxCostOfDelay.Clear();
        }

        /// <summary>
        /// this is the clear button for the data grids
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearTables_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
        }
        /// <summary>
        /// this is the clear button for the GA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            txtbxCostOfDelayAlgo.Clear();
            txtbxCostPrLoderAlgo.Clear();
            txtbxCostPrScalerAlgo.Clear();
            txtbxcostprtrukAlgo.Clear();
            txtbxScalerNoMxAlgo.Clear();
            txtbxTrukNoMxAlgo.Clear();
            txtbxPopulationNo.Clear();
            txtbxGenerationNo.Clear();
            txtbxMutationRate.Clear();
            txtbxmaterialAlgo.Clear();
            txtbxlodrprtrukAlgo.Clear();
            txtbxLosderNoMxAlgo.Clear();
            txtbxProjectDurationAlgo.Clear();
        }
    }
}
