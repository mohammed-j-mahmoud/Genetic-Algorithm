using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Core.Simulation;
using GeneticAlgorithm.Desktop.Views;

namespace GeneticAlgorithm.Desktop
{
    public partial class OptimizationView : Form
    {
        private const int ChartUpdateInterval = 1;
        private const string GaFitnessChartSeries = "BestThisGeneration";

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
            InitializePhaseTabs();
        }
        /// <summary>
        /// this button is made to rn the GA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRunGeneticAlgorithm_Click(object sender, EventArgs e)
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

            if (!FormInputParser.TryParseInt(txtGaPopulation.Text, "Population size", out int populationSize, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtGaMaterialVolume.Text, "Material volume", out float numCoal, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtGaLoadPerTruck.Text, "Truck load", out float numTruckLoad, showErrors) || numTruckLoad <= 0f)
            {
                if (showErrors)
                    MessageBox.Show("Truck load must be greater than zero.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtGaTruckCostPerDay.Text, "Truck cost per day", out float costTruckPerDay, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtGaLoaderCostPerDay.Text, "Loader cost per day", out float costLoaderPerDay, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtGaScalerCostPerDay.Text, "Scaler cost per day", out float costScalerPerDay, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtGaProjectDuration.Text, "Project duration", out float projectDuration, showErrors))
                return false;
            if (!FormInputParser.TryParseFloat(txtGaDelayCostPerDay.Text, "Cost of delay", out float costDelayPerDay, showErrors))
                return false;
            if (!FormInputParser.TryParseInt(txtGaMaxTrucks.Text, "Max trucks", out int maxTrucks, showErrors))
                return false;
            if (!FormInputParser.TryParseInt(txtGaMaxScalers.Text, "Max scalers", out int maxScalers, showErrors))
                return false;
            if (!FormInputParser.TryParseInt(txtGaMaxLoaders.Text, "Max loaders", out int maxLoaders, showErrors))
                return false;
            if (!FormInputParser.TryParseInt(txtGaGenerations.Text, "Generation count", out int lastGeneration, showErrors))
                return false;
            if (lastGeneration > 100_000)
            {
                if (showErrors)
                    MessageBox.Show("Generation count cannot exceed 100,000.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseMutationRate(txtGaMutationRate.Text, out double mutationRate, showErrors))
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
            IProgress<GaGenerationReport> progress,
            CancellationToken cancellationToken)
        {
            GeneticOptimizationRequest request = ToOptimizationRequest(parameters);

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
            }), cancellationToken);
        }

        private async Task RunGeneticAlgorithmAsync(GaRunParameters parameters)
        {
            if (_gaRunning)
                return;

            try
            {
                _gaRunning = true;
                SetGeneticAlgorithmControlsEnabled(false);
                SetRunStopButtonsEnabled(running: true, geneticTab: true, simulationTab: false);
                _runCancellation?.Cancel();
                _runCancellation?.Dispose();
                _runCancellation = new CancellationTokenSource();
                CancellationToken cancellationToken = _runCancellation.Token;

                chartGaFitness.Series[GaFitnessChartSeries].Points.Clear();
                progressGaRun.Visible = true;
                progressGaRun.Minimum = 1;
                progressGaRun.Maximum = parameters.LastGeneration;
                progressGaRun.Value = 1;

                var progress = new Progress<GaGenerationReport>(report =>
                {
                    if (IsDisposed)
                        return;

                    int generation = Math.Min(report.Generation, progressGaRun.Maximum);
                    progressGaRun.Value = Math.Max(progressGaRun.Minimum, generation);

                    if (report.UpdateChart)
                        chartGaFitness.Series[GaFitnessChartSeries].Points.AddXY(report.Generation, report.Fitness);
                });

                OptimizationRunResult results = await Task.Run(
                    () => RunGeneticAlgorithmCore(parameters, progress, cancellationToken),
                    cancellationToken).ConfigureAwait(true);

                progressGaRun.Value = progressGaRun.Maximum;
                ApplyGaComparisonResult(results);
                string completion = results.StoppedEarly
                    ? "GA stopped early. Best result so far is shown."
                    : "GA is Done ";
                MessageBox.Show(completion, "Message Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Run cancelled.", "Message Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                progressGaRun.Visible = false;
                SetGeneticAlgorithmControlsEnabled(true);
                SetRunStopButtonsEnabled(running: false, geneticTab: false, simulationTab: false);
                _runCancellation?.Dispose();
                _runCancellation = null;
                _gaRunning = false;
            }
        }

        private void SetGeneticAlgorithmControlsEnabled(bool enabled)
        {
            btnRunGeneticAlgorithm.Enabled = enabled;
            btnRunGaDemo.Enabled = enabled;
            btnRunSimulation.Enabled = enabled;
            btnClearGaFields.Enabled = enabled;
            if (_btnRunSimDemo != null)
                _btnRunSimDemo.Enabled = enabled;
            UseWaitCursor = !enabled;
        }

        private void OptimizationView_Load(object sender, EventArgs e)
        {
            picGaHeader.Enabled = true;
        }

        private void EnsureDefaultDistributionsPopulated()
        {
            if (!IsDistributionGridEmpty(gridLoadingDistribution) &&
                !IsDistributionGridEmpty(gridWeighingDistribution) &&
                !IsDistributionGridEmpty(gridTravelingDistribution))
            {
                return;
            }

            ApplyDefaultDistributionsToForm();
            LayoutDistributionTab();
        }

        private static bool IsDistributionGridEmpty(DataGridView grid)
        {
            if (grid == null || grid.Rows.Count == 0)
                return true;

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                object timeValue = row.Cells.Count > 0 ? row.Cells[0].Value : null;
                if (timeValue != null && !string.IsNullOrWhiteSpace(timeValue.ToString()))
                    return false;
            }

            return true;
        }

        private void btnAddDistributionsToSimulation_Click(object sender, EventArgs e)
        {
            try
            {
                loadingElements = DistributionGridReader.Read(gridLoadingDistribution, "Loading");
                weighingElements = DistributionGridReader.Read(gridWeighingDistribution, "Weighing");
                travelingElements = DistributionGridReader.Read(gridTravelingDistribution, "Traveling");
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
        private async void btnRunSimulation_Click(object sender, EventArgs e)
        {
            if (!TryReadSimulationInputs(out SimulationRunInputs inputs))
                return;

            await RunSimulationAsync(inputs).ConfigureAwait(true);
        }

        private async Task RunSimulationDemoAsync()
        {
            SimulationRunInputs inputs = CreateDemoSimulationRunInputs();
            ApplySimulationRunInputsToForm(inputs);
            ApplyDefaultDistributionsToForm();
            await RunSimulationAsync(inputs).ConfigureAwait(true);
        }

        private async Task RunSimulationAsync(SimulationRunInputs inputs)
        {
            if (_simRunning)
                return;

            try
            {
                _simRunning = true;
                btnClearSimulationFields.Enabled = false;
                SetGeneticAlgorithmControlsEnabled(false);
                SetRunStopButtonsEnabled(running: true, geneticTab: false, simulationTab: true);
                UseWaitCursor = true;
                _runCancellation?.Cancel();
                _runCancellation?.Dispose();
                _runCancellation = new CancellationTokenSource();
                CancellationToken cancellationToken = _runCancellation.Token;

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
                }), cancellationToken).ConfigureAwait(true);

                if (cancellationToken.IsCancellationRequested)
                    return;

                ApplySimulationResults(SimulationResults);
                MessageBox.Show("Simulation is Done ", "Message Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Simulation cancelled.", "Message Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnClearSimulationFields.Enabled = true;
                SetGeneticAlgorithmControlsEnabled(true);
                SetRunStopButtonsEnabled(running: false, geneticTab: false, simulationTab: false);
                _runCancellation?.Dispose();
                _runCancellation = null;
                UseWaitCursor = false;
                _simRunning = false;
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

            if (!FormInputParser.TryParseFloat(txtSimMaterialVolume.Text, "Material volume", out float numCoal))
                return false;
            if (!FormInputParser.TryParseFloat(txtSimTruckCount.Text, "Truck count", out float numTruck) || numTruck <= 0f)
            {
                MessageBox.Show("Truck count must be greater than zero.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtSimLoadPerTruck.Text, "Truck load", out float numTruckLoad) || numTruckLoad <= 0f)
            {
                MessageBox.Show("Truck load must be greater than zero.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtSimTruckCostPerDay.Text, "Truck cost per day", out float costTruckPerDay))
                return false;
            if (!FormInputParser.TryParseFloat(txtSimLoaderCount.Text, "Loader count", out float numLoader) || numLoader < 1f)
            {
                MessageBox.Show("Loader count must be at least 1.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtSimLoaderCostPerDay.Text, "Loader cost per day", out float costLoaderPerDay))
                return false;
            if (!FormInputParser.TryParseFloat(txtSimScalerCount.Text, "Scaler count", out float numScaler) || numScaler < 1f)
            {
                MessageBox.Show("Scaler count must be at least 1.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!FormInputParser.TryParseFloat(txtSimScalerCostPerDay.Text, "Scaler cost per day", out float costScalerPerDay))
                return false;
            if (!FormInputParser.TryParseFloat(txtSimProjectDuration.Text, "Project duration", out float projectDuration))
                return false;
            if (!FormInputParser.TryParseFloat(txtSimDelayCostPerDay.Text, "Cost of delay", out float costOfDelay))
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
            chartSimulationCosts.Series["Actual Cost"].Points.Clear();
            chartSimulationCosts.Series["Delay Cost"].Points.Clear();

            lblSimLoaderCostValue.Text = results.LoaderCost.ToString("N2");
            lblSimScalerCostValue.Text = results.ScalerCost.ToString("N2");
            lblSimTruckCostValue.Text = results.TruckCost.ToString("N2");
            lblSimDelayCostValue.Text = results.DelayCost.ToString("N2");
            lblSimProjectDurationValue.Text = results.TotalDays.ToString("N2");
            lblSimTotalCostValue.Text = results.TotalCost.ToString("N2");
            lblSimDaysDelayedValue.Text = results.DelayDays.ToString("N2");
            lblSimUtilScalersValue.Text = Math.Round(results.ScalerUtilization, 3).ToString();
            lblSimUtilLoadersValue.Text = Math.Round(results.LoaderUtilization, 3).ToString();
            lblSimUtilTrucksValue.Text = Math.Round(results.TruckUtilization, 3).ToString();

            chartSimulationCosts.Series["Actual Cost"].Points.AddXY(0, results.TotalCost);
            chartSimulationCosts.Series["Delay Cost"].Points.AddXY(1, results.DelayCost);
            chartSimulationCosts.Series["Actual Cost"].BorderWidth = 3;
            chartSimulationCosts.Series["Delay Cost"].BorderWidth = 3;
            chartSimulationCosts.ChartAreas[0].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartSimulationCosts.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
        }
        private static SimulationRunInputs CreateDemoSimulationRunInputs() =>
            new SimulationRunInputs
            {
                NumCoal = DemoDefaults.MaterialVolume,
                NumTruck = DemoDefaults.DemoTrucks,
                NumTruckLoad = DemoDefaults.TruckLoadVolume,
                CostTruckPerDay = DemoDefaults.TruckCostPerDay,
                NumLoader = DemoDefaults.DemoLoaders,
                CostLoaderPerDay = DemoDefaults.LoaderCostPerDay,
                NumScaler = DemoDefaults.DemoScalers,
                CostScalerPerDay = DemoDefaults.ScalerCostPerDay,
                ProjectDuration = DemoDefaults.ProjectDurationDays,
                CostOfDelay = DemoDefaults.DelayCostPerDay
            };

        private void ApplySimulationRunInputsToForm(SimulationRunInputs inputs)
        {
            txtSimMaterialVolume.Text = inputs.NumCoal.ToString(CultureInfo.InvariantCulture);
            txtSimTruckCount.Text = inputs.NumTruck.ToString(CultureInfo.InvariantCulture);
            txtSimLoadPerTruck.Text = inputs.NumTruckLoad.ToString(CultureInfo.InvariantCulture);
            txtSimTruckCostPerDay.Text = inputs.CostTruckPerDay.ToString(CultureInfo.InvariantCulture);
            txtSimLoaderCount.Text = inputs.NumLoader.ToString(CultureInfo.InvariantCulture);
            txtSimLoaderCostPerDay.Text = inputs.CostLoaderPerDay.ToString(CultureInfo.InvariantCulture);
            txtSimScalerCount.Text = inputs.NumScaler.ToString(CultureInfo.InvariantCulture);
            txtSimScalerCostPerDay.Text = inputs.CostScalerPerDay.ToString(CultureInfo.InvariantCulture);
            txtSimProjectDuration.Text = inputs.ProjectDuration.ToString(CultureInfo.InvariantCulture);
            txtSimDelayCostPerDay.Text = inputs.CostOfDelay.ToString(CultureInfo.InvariantCulture);
        }

        private void ApplyDefaultDistributionsToForm()
        {
            loadingElements = CreateDefaultLoadingDistribution();
            weighingElements = CreateDefaultWeighingDistribution();
            travelingElements = CreateDefaultTravelingDistribution();

            PopulateDistributionGrid(gridLoadingDistribution, loadingElements);
            PopulateDistributionGrid(gridWeighingDistribution, weighingElements);
            PopulateDistributionGrid(gridTravelingDistribution, travelingElements);

            _viewModel.LoadingDistribution.Clear();
            _viewModel.LoadingDistribution.AddRange(loadingElements);
            _viewModel.WeighingDistribution.Clear();
            _viewModel.WeighingDistribution.AddRange(weighingElements);
            _viewModel.TravelingDistribution.Clear();
            _viewModel.TravelingDistribution.AddRange(travelingElements);
        }

        private static GaRunParameters CreateDemoGaRunParameters() =>
            new GaRunParameters
            {
                PopulationSize = DemoDefaults.PopulationSize,
                NumCoal = DemoDefaults.MaterialVolume,
                NumTruckLoad = DemoDefaults.TruckLoadVolume,
                CostTruckPerDay = DemoDefaults.TruckCostPerDay,
                CostLoaderPerDay = DemoDefaults.LoaderCostPerDay,
                CostScalerPerDay = DemoDefaults.ScalerCostPerDay,
                ProjectDuration = DemoDefaults.ProjectDurationDays,
                CostDelayPerDay = DemoDefaults.DelayCostPerDay,
                MaxTrucks = DemoDefaults.DemoTrucks,
                MaxLoaders = DemoDefaults.DemoLoaders,
                MaxScalers = DemoDefaults.DemoScalers,
                LastGeneration = DemoDefaults.Generations,
                MutationRate = DemoDefaults.MutationRate,
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
            txtGaPopulation.Text = parameters.PopulationSize.ToString(CultureInfo.InvariantCulture);
            txtGaMaterialVolume.Text = parameters.NumCoal.ToString(CultureInfo.InvariantCulture);
            txtGaLoadPerTruck.Text = parameters.NumTruckLoad.ToString(CultureInfo.InvariantCulture);
            txtGaTruckCostPerDay.Text = parameters.CostTruckPerDay.ToString(CultureInfo.InvariantCulture);
            txtGaLoaderCostPerDay.Text = parameters.CostLoaderPerDay.ToString(CultureInfo.InvariantCulture);
            txtGaScalerCostPerDay.Text = parameters.CostScalerPerDay.ToString(CultureInfo.InvariantCulture);
            txtGaProjectDuration.Text = parameters.ProjectDuration.ToString(CultureInfo.InvariantCulture);
            txtGaDelayCostPerDay.Text = parameters.CostDelayPerDay.ToString(CultureInfo.InvariantCulture);
            txtGaMaxTrucks.Text = parameters.MaxTrucks.ToString(CultureInfo.InvariantCulture);
            txtGaMaxLoaders.Text = parameters.MaxLoaders.ToString(CultureInfo.InvariantCulture);
            txtGaMaxScalers.Text = parameters.MaxScalers.ToString(CultureInfo.InvariantCulture);
            txtGaGenerations.Text = parameters.LastGeneration.ToString(CultureInfo.InvariantCulture);
            txtGaMutationRate.Text = parameters.MutationRate.ToString(CultureInfo.InvariantCulture);

            loadingElements = new List<KeyValuePair<int, double>>(parameters.LoadingElements);
            weighingElements = new List<KeyValuePair<int, double>>(parameters.WeighingElements);
            travelingElements = new List<KeyValuePair<int, double>>(parameters.TravelingElements);

            PopulateDistributionGrid(gridLoadingDistribution, loadingElements);
            PopulateDistributionGrid(gridWeighingDistribution, weighingElements);
            PopulateDistributionGrid(gridTravelingDistribution, travelingElements);

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
        private async void btnRunGaDemo_Click(object sender, EventArgs e)
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

        private void btnClearSimulationFields_Click(object sender, EventArgs e)
        {
            txtSimMaterialVolume.Clear();
            txtSimTruckCount.Clear();
            txtSimLoadPerTruck.Clear();
            txtSimTruckCostPerDay.Clear();
            txtSimLoaderCount.Clear();
            txtSimLoaderCostPerDay.Clear();
            txtSimScalerCount.Clear();
            txtSimScalerCostPerDay.Clear();
            txtSimProjectDuration.Clear();
            txtSimDelayCostPerDay.Clear();
        }

        private void btnClearDistributionGrids_Click(object sender, EventArgs e)
        {
            gridLoadingDistribution.Rows.Clear();
            gridWeighingDistribution.Rows.Clear();
            gridTravelingDistribution.Rows.Clear();
        }
        /// <summary>
        /// this is the clear button for the GA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearGaFields_Click(object sender, EventArgs e)
        {
            txtGaDelayCostPerDay.Clear();
            txtGaLoaderCostPerDay.Clear();
            txtGaScalerCostPerDay.Clear();
            txtGaTruckCostPerDay.Clear();
            txtGaMaxScalers.Clear();
            txtGaMaxTrucks.Clear();
            txtGaPopulation.Clear();
            txtGaGenerations.Clear();
            txtGaMutationRate.Clear();
            txtGaMaterialVolume.Clear();
            txtGaLoadPerTruck.Clear();
            txtGaMaxLoaders.Clear();
            txtGaProjectDuration.Clear();
        }
    }
}
