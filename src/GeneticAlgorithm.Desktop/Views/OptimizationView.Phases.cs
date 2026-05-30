using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Desktop.Views;
using GeneticAlgorithm.Desktop.Views.Layout;

namespace GeneticAlgorithm.Desktop
{
    public partial class OptimizationView
    {
        private PhaseResultPanel _phase1Panel;
        private PhaseResultPanel _phase2Panel;
        private PhaseResultPanel _phase3Panel;
        private PhaseResultPanel _phase4Panel;
        private GaMethodInfoPanel _gaMethodInfoPanel;
        private bool _phaseRunning;
        private CancellationTokenSource _runCancellation;
        private Button _btnStopGenetic;
        private Button _btnStopSim;
        private Button _btnRunSimDemo;
        private bool _simRunning;

        private void InitializePhaseTabs()
        {
            if (ClientSize.Width < AppLayoutMetrics.FormDefaultWidth || ClientSize.Height < AppLayoutMetrics.FormDefaultHeight)
                ClientSize = new Size(AppLayoutMetrics.FormDefaultWidth, AppLayoutMetrics.FormDefaultHeight);
            StartPosition = FormStartPosition.CenterScreen;

            ApplyInputFieldLabels();
            InputGroupLayout.ConfigureGeneticAlgorithmInputs(
                grpGaInputs,
                lblGaMaterialVolume,
                lblGaLoadPerTruck,
                lblGaTruckCostPerDay,
                lblGaLoaderCostPerDay,
                lblGaScalerCostPerDay,
                lblGaProjectDuration,
                lblGaDelayCostPerDay,
                txtGaMaterialVolume,
                txtGaLoadPerTruck,
                txtGaTruckCostPerDay,
                txtGaLoaderCostPerDay,
                txtGaScalerCostPerDay,
                txtGaProjectDuration,
                txtGaDelayCostPerDay,
                lblGaMaxTrucks,
                txtGaMaxTrucks,
                lblGaMaxLoaders,
                txtGaMaxLoaders,
                lblGaMaxScalers,
                txtGaMaxScalers,
                lblGaPopulation,
                txtGaPopulation,
                lblGaGenerations,
                txtGaGenerations,
                lblGaMutationRate,
                txtGaMutationRate);
            InputGroupLayout.ConfigureSimulationInputs(
                grpSimulationInputs,
                lblSimMaterialVolume,
                lblSimLoadPerTruck,
                lblSimTruckCostPerDay,
                lblSimLoaderCostPerDay,
                lblSimScalerCostPerDay,
                lblSimTruckCount,
                txtSimMaterialVolume,
                txtSimLoadPerTruck,
                txtSimTruckCostPerDay,
                txtSimLoaderCostPerDay,
                txtSimScalerCostPerDay,
                txtSimTruckCount,
                lblSimLoaderCount,
                txtSimLoaderCount,
                lblSimScalerCount,
                txtSimScalerCount,
                lblSimProjectDuration,
                txtSimProjectDuration,
                lblSimDelayCostPerDay,
                txtSimDelayCostPerDay);
            InputGroupLayout.ConfigureSimulationOutputs(
                grpSimulationUtilizationOutputs,
                grpSimulationCostOutputs,
                lblSimUtilTrucksCaption,
                lblSimUtilTrucksValue,
                lblSimUtilLoadersCaption,
                lblSimUtilLoadersValue,
                lblSimUtilScalersCaption,
                lblSimUtilScalersValue,
                lblSimProjectDurationCaption,
                lblSimProjectDurationValue,
                lblSimDaysDelayedCaption,
                lblSimDaysDelayedValue,
                lblSimTruckCostCaption,
                lblSimTruckCostValue,
                lblSimDelayCostCaption,
                lblSimDelayCostValue,
                lblSimLoaderCostCaption,
                lblSimLoaderCostValue,
                lblSimTotalCostCaption,
                lblSimTotalCostValue,
                lblSimScalerCostCaption,
                lblSimScalerCostValue);

            ReparentGaClearButton();

            InitializeRunStopButtons();
            InitializeGaMethodInfo();
            InitializeSimulationTabLayout();
            InitializeDistributionTabLayout();

            _phase1Panel = CreatePhasePanel(
                OptimizationPhaseDisplay.ExhaustiveSearch,
                "Exhaustive search — tries every trucks × loaders × scalers combination.",
                "• Search: brute force all combinations in your max bounds" + Environment.NewLine +
                "• Simulation during search: expected-time (deterministic averages)" + Environment.NewLine +
                "• Cache: yes — same combo is never simulated twice" + Environment.NewLine +
                "• Final result below: one seeded stochastic verify on the best combo" + Environment.NewLine +
                "• Best when: small search space; guaranteed best expected-time rank",
                OptimizationPhase.Phase1);

            _phase2Panel = CreatePhasePanel(
                OptimizationPhaseDisplay.GeneticSearch,
                "Smart search — genetic algorithm on fast expected-time fitness, then verify finalists.",
                "• Search: genetic algorithm (elite selection, crossover, mutation)" + Environment.NewLine +
                "• Simulation during search: expected-time (fast, cached)" + Environment.NewLine +
                "• Final step: parallel seeded stochastic verify on top 5 candidates" + Environment.NewLine +
                "• Best when: medium/large search space; good balance of speed and accuracy",
                OptimizationPhase.Phase2);

            _phase3Panel = CreatePhasePanel(
                OptimizationPhaseDisplay.SurrogateSearch,
                "Surrogate search — expected-time ranking for normal bounds, formula fallback for huge grids.",
                "• Search: expected-time simulation + cache when combos ≤ 50,000 (same model as Exhaustive/DP)" + Environment.NewLine +
                "• Large bounds: pipelined surrogate formula (loader/scaler/truck bottlenecks, 480-min work days)" + Environment.NewLine +
                "• Final step: parallel seeded stochastic verify on top 5 candidates" + Environment.NewLine +
                "• Best when: you want top-5 verify instead of verifying only the single best combo",
                OptimizationPhase.Phase3);

            _phase4Panel = CreatePhasePanel(
                OptimizationPhaseDisplay.DynamicProgrammingSearch,
                "Exact expected-time optimum — 3D DP table with cached subproblems, then verify.",
                "• Search: bottom-up dynamic programming over (trucks, loaders, scalers)" + Environment.NewLine +
                "• Each cell: expected-time simulation (models queues, not the surrogate formula)" + Environment.NewLine +
                "• Recurrence: best fitness with at most t trucks, l loaders, s scalers" + Environment.NewLine +
                "• Final step: one seeded stochastic verify on the DP optimum" + Environment.NewLine +
                "• Same simulation work as Exhaustive Search; DP adds an explicit optimal substructure table" + Environment.NewLine +
                "• Best when: you want the proven best expected-time fleet within bounds",
                OptimizationPhase.Phase4);

            RegisterPhaseTab(OptimizationPhaseDisplay.ExhaustiveSearch, _phase1Panel);
            RegisterPhaseTab(OptimizationPhaseDisplay.GeneticSearch, _phase2Panel);
            RegisterPhaseTab(OptimizationPhaseDisplay.SurrogateSearch, _phase3Panel);
            RegisterPhaseTab(OptimizationPhaseDisplay.DynamicProgrammingSearch, _phase4Panel);

            grpLegacyGaOutputs.Visible = false;
            ApplyGlobalUiPolish();
            EnsureDefaultDistributionsPopulated();
        }

        private void InitializeRunStopButtons()
        {
            _btnStopGenetic = new Button
            {
                Text = "Stop",
                Size = new Size(120, 52),
                Enabled = false
            };
            _btnStopGenetic.Click += (_, __) => _runCancellation?.Cancel();
            tabGeneticAlgorithm.Controls.Add(_btnStopGenetic);

            _btnStopSim = new Button
            {
                Text = "Stop",
                Size = new Size(120, 40),
                Enabled = false,
                Visible = false   // simulation has no cooperative cancellation path yet
            };
            _btnStopSim.Click += (_, __) => _runCancellation?.Cancel();
            tabSimulation.Controls.Add(_btnStopSim);

            _btnRunSimDemo = new Button
            {
                Text = UiCopy.RunDemo,
                Size = new Size(250, AppLayoutMetrics.PrimaryButtonHeight)
            };
            _btnRunSimDemo.Click += async (_, __) => await RunSimulationDemoAsync().ConfigureAwait(true);
            tabSimulation.Controls.Add(_btnRunSimDemo);
        }

        private void InitializeGaMethodInfo()
        {
            tabGeneticAlgorithm.AutoScroll = true;
            _gaMethodInfoPanel = new GaMethodInfoPanel();
            tabGeneticAlgorithm.Controls.Add(_gaMethodInfoPanel);

            Resize += (_, __) =>
            {
                LayoutGeneticAlgorithmTab();
                LayoutSimulationTab();
                LayoutDistributionTab();
            };
            LayoutGeneticAlgorithmTab();
        }

        private void InitializeSimulationTabLayout()
        {
            tabSimulation.AutoScroll = true;
            LayoutSimulationTab();
        }

        private void InitializeDistributionTabLayout()
        {
            tabDistribution.AutoScroll = false;
            grpDistributionDefaults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            grpDistributionNewValues.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            LayoutDistributionTab();
        }

        private void LayoutDistributionTab()
        {
            if (tabDistribution == null || grpDistributionNewValues == null || grpDistributionDefaults == null)
                return;

            int margin = AppLayoutMetrics.Margin;
            int gap = AppLayoutMetrics.ColumnGap;
            int tabWidth = tabDistribution.ClientSize.Width;
            int tabHeight = tabDistribution.ClientSize.Height;

            int leftWidth = Math.Clamp((tabWidth - margin * 2 - gap) / 3, 280, 360);
            int rightWidth = Math.Max(420, tabWidth - margin * 2 - gap - leftWidth);
            int panelHeight = Math.Max(360, tabHeight - margin * 2);

            grpDistributionDefaults.SetBounds(margin, margin, leftWidth, panelHeight);
            grpDistributionNewValues.SetBounds(margin + leftWidth + gap, margin, rightWidth, panelHeight);

            LayoutDefaultValuesPanel(leftWidth, panelHeight);
            LayoutNewValuesPanel(rightWidth, panelHeight);
        }

        private void LayoutDefaultValuesPanel(int panelWidth, int panelHeight)
        {
            const int innerMargin = 12;
            const int sectionGap = 8;
            const int headerSpace = 28;
            int innerWidth = Math.Max(180, panelWidth - innerMargin * 2 - 4);
            int availableHeight = Math.Max(0, panelHeight - headerSpace - sectionGap * 2);
            int sectionHeight = Math.Max(100, availableHeight / 3);

            ConfigureDefaultChart(picDefaultLoadingDistribution, innerMargin, headerSpace, innerWidth, sectionHeight);
            ConfigureDefaultChart(
                picDefaultWeighingDistribution,
                innerMargin,
                headerSpace + sectionHeight + sectionGap,
                innerWidth,
                sectionHeight);
            ConfigureDefaultChart(
                picDefaultTravelDistribution,
                innerMargin,
                headerSpace + (sectionHeight + sectionGap) * 2,
                innerWidth,
                Math.Max(100, panelHeight - headerSpace - (sectionHeight + sectionGap) * 2 - innerMargin));
        }

        private static void ConfigureDefaultChart(PictureBox picture, int x, int y, int width, int height)
        {
            if (picture == null)
                return;

            picture.SetBounds(x, y, width, height);
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            picture.BorderStyle = BorderStyle.FixedSingle;
            picture.BackColor = AppLayoutMetrics.PanelBackColor;
        }

        private void LayoutNewValuesPanel(int panelWidth, int panelHeight)
        {
            const int innerMargin = 12;
            const int columnGap = 12;
            const int labelTop = 20;
            const int labelHeight = 22;
            const int gridTop = labelTop + labelHeight + 4;
            const int buttonHeight = AppLayoutMetrics.PrimaryButtonHeight;
            const int gridBottomGap = 16;
            int contentWidth = Math.Max(360, panelWidth - innerMargin * 2);
            int columnWidth = Math.Max(150, (contentWidth - columnGap * 2) / 3);
            int gridHeight = EstimateDistributionGridHeight(
                gridLoadingDistribution,
                gridWeighingDistribution,
                gridTravelingDistribution);
            int buttonY = gridTop + gridHeight + gridBottomGap;
            int buttonWidth = Math.Max(160, (contentWidth - columnGap) / 2);

            PlaceDistributionColumn(lblLoadingDistributionHeader, gridLoadingDistribution, innerMargin, labelTop, labelHeight, gridTop, columnWidth, gridHeight);
            PlaceDistributionColumn(
                lblWeighingDistributionHeader,
                gridWeighingDistribution,
                innerMargin + columnWidth + columnGap,
                labelTop,
                labelHeight,
                gridTop,
                columnWidth,
                gridHeight);
            PlaceDistributionColumn(
                lblTravelingDistributionHeader,
                gridTravelingDistribution,
                innerMargin + (columnWidth + columnGap) * 2,
                labelTop,
                labelHeight,
                gridTop,
                columnWidth,
                gridHeight);

            btnAddDistributionsToSimulation.SetBounds(innerMargin, buttonY, buttonWidth, buttonHeight);
            btnClearDistributionGrids.SetBounds(innerMargin + buttonWidth + columnGap, buttonY, buttonWidth, buttonHeight);
        }

        private const int GridHeaderHeightPx = 28;
        private const int GridRowHeightPx = 24;
        private const int GridPaddingPx = 6;
        private const int GridMinHeightPx = 130;
        private const int GridMaxHeightPx = 190;

        private static int EstimateDistributionGridHeight(params DataGridView[] grids)
        {
            int maxRows = 4;
            foreach (DataGridView grid in grids)
            {
                if (grid == null)
                    continue;

                int rowCount = grid.AllowUserToAddRows ? grid.Rows.Count - 1 : grid.Rows.Count;
                maxRows = Math.Max(maxRows, Math.Max(1, rowCount));
            }

            return Math.Clamp(
                GridHeaderHeightPx + (maxRows * GridRowHeightPx) + GridPaddingPx,
                GridMinHeightPx,
                GridMaxHeightPx);
        }

        private static void PlaceDistributionColumn(
            Label header,
            DataGridView grid,
            int x,
            int labelTop,
            int labelHeight,
            int gridTop,
            int width,
            int height)
        {
            if (header != null)
            {
                header.AutoSize = false;
                header.AutoEllipsis = true;
                header.SetBounds(x, labelTop, width, labelHeight);
                header.TextAlign = ContentAlignment.MiddleLeft;
            }

            if (grid == null)
                return;

            grid.SetBounds(x, gridTop, width, height);
            grid.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        private void LayoutSimulationTab()
        {
            if (tabSimulation == null)
                return;

            var layout = new SplitTabLayout(tabSimulation.ClientSize.Width, tabSimulation.ClientSize.Height, InputGroupLayout.RequiredWidth);
            const int inputHeight = 210;
            int contentTop = AppLayoutMetrics.Margin + AppLayoutMetrics.HeroImageHeight + AppLayoutMetrics.SectionGap;

            picSimulationHeader.Bounds = layout.LeftColumn(AppLayoutMetrics.Margin, AppLayoutMetrics.HeroImageHeight);
            grpSimulationInputs.SetBounds(layout.Margin, contentTop, layout.LeftWidth, inputHeight);

            int buttonY = contentTop + inputHeight + AppLayoutMetrics.SectionGap;
            ActionButtonStripLayout.ApplyTwoRowPrimaryActions(
                layout.LeftWidth,
                buttonY,
                _btnRunSimDemo,
                btnRunSimulation,
                btnClearSimulationFields,
                _btnStopSim);

            Rectangle chartArea = layout.ChartArea(AppLayoutMetrics.Margin, 0.42);
            chartSimulationCosts.Bounds = chartArea;

            Rectangle outputArea = layout.StackedPanelArea(chartArea);
            int utilizationWidth = Math.Min(300, outputArea.Width / 2 - AppLayoutMetrics.SectionGap);
            grpSimulationUtilizationOutputs.SetBounds(outputArea.X, outputArea.Y, utilizationWidth, outputArea.Height);
            grpSimulationCostOutputs.SetBounds(
                outputArea.X + utilizationWidth + AppLayoutMetrics.SectionGap,
                outputArea.Y,
                Math.Max(280, outputArea.Width - utilizationWidth - AppLayoutMetrics.SectionGap),
                outputArea.Height);
        }

        private void LayoutGeneticAlgorithmTab()
        {
            if (_gaMethodInfoPanel == null || tabGeneticAlgorithm == null)
                return;

            var layout = new SplitTabLayout(tabGeneticAlgorithm.ClientSize.Width, tabGeneticAlgorithm.ClientSize.Height, InputGroupLayout.RequiredWidth);
            const int inputHeight = 310;
            int contentTop = AppLayoutMetrics.Margin + AppLayoutMetrics.HeroImageHeight + AppLayoutMetrics.SectionGap;

            picGaHeader.Bounds = layout.LeftColumn(AppLayoutMetrics.Margin, AppLayoutMetrics.HeroImageHeight);
            grpGaInputs.SetBounds(layout.Margin, contentTop, layout.LeftWidth, inputHeight);

            int buttonY = contentTop + inputHeight + AppLayoutMetrics.SectionGap;
            ActionButtonStripLayout.ApplyTwoRowPrimaryActions(
                layout.LeftWidth,
                buttonY,
                btnRunGaDemo,
                btnRunGeneticAlgorithm,
                btnClearGaFields,
                _btnStopGenetic);

            Rectangle chartArea = layout.ChartArea(AppLayoutMetrics.Margin, 0.28);
            chartGaFitness.Bounds = chartArea;
            lblGaProgressCaption.SetBounds(chartArea.X, chartArea.Bottom + 6, 130, 18);
            progressGaRun.SetBounds(chartArea.X + 135, chartArea.Bottom + 3, Math.Max(200, chartArea.Width - 135), 22);

            Rectangle infoArea = layout.StackedPanelArea(chartArea, gap: 28);
            _gaMethodInfoPanel.Bounds = infoArea;
            _gaMethodInfoPanel.BringToFront();
        }

        private PhaseResultPanel CreatePhasePanel(
            string title,
            string summary,
            string howItWorks,
            OptimizationPhase phase)
        {
            var panel = new PhaseResultPanel(
                title,
                summary,
                howItWorks,
                "Best result (seeded stochastic verify)");
            panel.RunRequested += async (_, __) => await RunPhaseAsync(phase).ConfigureAwait(true);
            panel.StopRequested += (_, __) => _runCancellation?.Cancel();
            return panel;
        }

        private static TabPage CreatePhaseTab(string title, PhaseResultPanel panel)
        {
            var page = new TabPage(title) { UseVisualStyleBackColor = true };
            page.Controls.Add(panel);
            return page;
        }

        private async Task RunPhaseAsync(OptimizationPhase phase)
        {
            if (_phaseRunning)
                return;

            if (!TryReadGaRunParameters(out GaRunParameters parameters))
                return;

            PhaseResultPanel panel = GetPhasePanel(phase);

            try
            {
                _phaseRunning = true;
                _runCancellation?.Cancel();
                _runCancellation?.Dispose();
                _runCancellation = new CancellationTokenSource();
                CancellationToken cancellationToken = _runCancellation.Token;

                panel.SetRunning(true);
                SetGeneticAlgorithmControlsEnabled(false);
                SetPhasePanelsRunning(true, except: panel);
                SetRunStopButtonsEnabled(running: true, geneticTab: false, simulationTab: false);

                GeneticOptimizationRequest request = ToOptimizationRequest(parameters);
                var service = new PhaseOptimizationService();

                OptimizationRunResult result = await Task.Run(() =>
                {
                    var progress = new Progress<int>(p => panel.ReportProgress(p));
                    switch (phase)
                    {
                        case OptimizationPhase.Phase1:
                            return service.RunPhase1(request, progress, cancellationToken);
                        case OptimizationPhase.Phase2:
                            return service.RunPhase2(request, progress, cancellationToken);
                        case OptimizationPhase.Phase3:
                            return service.RunPhase3(request, progress, cancellationToken);
                        case OptimizationPhase.Phase4:
                            return service.RunPhase4(request, progress, cancellationToken);
                        default:
                            throw new ArgumentOutOfRangeException(nameof(phase), phase, "Unknown optimization phase.");
                    }
                }, cancellationToken).ConfigureAwait(true);

                panel.ApplyResult(result);
                string completion = result.StoppedEarly ? " stopped early. Partial results saved." : " complete.";
                MessageBox.Show(
                    OptimizationPhaseDisplay.GetStrategyName(phase) + completion,
                    "Optimization complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(
                    "Run cancelled.",
                    "Optimization complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Optimization complete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                panel.SetRunning(false);
                SetPhasePanelsRunning(false, except: null);
                SetGeneticAlgorithmControlsEnabled(true);
                SetRunStopButtonsEnabled(running: false, geneticTab: false, simulationTab: false);
                _runCancellation?.Dispose();
                _runCancellation = null;
                _phaseRunning = false;
            }
        }

        private void SetRunStopButtonsEnabled(bool running, bool geneticTab, bool simulationTab)
        {
            if (_btnStopGenetic != null)
                _btnStopGenetic.Enabled = running && geneticTab;
            if (_btnStopSim != null)
                _btnStopSim.Enabled = running && simulationTab;
        }

        private PhaseResultPanel GetPhasePanel(OptimizationPhase phase)
        {
            switch (phase)
            {
                case OptimizationPhase.Phase1: return _phase1Panel;
                case OptimizationPhase.Phase2: return _phase2Panel;
                case OptimizationPhase.Phase3: return _phase3Panel;
                case OptimizationPhase.Phase4: return _phase4Panel;
                default: throw new ArgumentOutOfRangeException(nameof(phase), phase, "Unknown optimization phase.");
            }
        }

        private void SetPhasePanelsRunning(bool running, PhaseResultPanel except)
        {
            SetPanelRunning(_phase1Panel, running, except);
            SetPanelRunning(_phase2Panel, running, except);
            SetPanelRunning(_phase3Panel, running, except);
            SetPanelRunning(_phase4Panel, running, except);
        }

        private static void SetPanelRunning(PhaseResultPanel panel, bool running, PhaseResultPanel except)
        {
            if (panel == null)
                return;

            if (!running)
            {
                panel.SetRunning(false);
                return;
            }

            if (panel == except)
                panel.SetRunning(true);
            else
                panel.SetBusy(true);
        }

        private void ApplyGaComparisonResult(OptimizationRunResult results)
        {
            if (_gaMethodInfoPanel == null)
                return;

            _gaMethodInfoPanel.BestResult.Apply(results?.BestChromosome);
            if (results != null)
            {
                int generationsRun = results.GenerationsCompleted > 0
                    ? results.GenerationsCompleted
                    : results.BestGeneration;
                string stoppedNote = results.StoppedEarly
                    ? "Stopped early — showing best result found so far." + Environment.NewLine
                    : string.Empty;
                _gaMethodInfoPanel.RunStats.Text =
                    stoppedNote +
                    $"Simulations: {results.SimulationCalls}   |   Cache hits: {results.CacheHits}   |   " +
                    $"Generations run: {generationsRun}   |   Best found at generation: {results.BestGeneration}";
            }
        }

        private static GeneticOptimizationRequest ToOptimizationRequest(GaRunParameters parameters) =>
            new GeneticOptimizationRequest
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
    }
}
