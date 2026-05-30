using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneticAlgorithm.Application;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Desktop.Views;

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
        private bool _simRunning;

        private void InitializePhaseTabs()
        {
            MinimumSize = new Size(1200, 680);
            if (ClientSize.Width < 1320 || ClientSize.Height < 780)
                ClientSize = new Size(1320, 780);

            ApplyInputFieldLabels();
            InputGroupLayout.ConfigureGeneticAlgorithmInputs(groupBox1, button5);
            InputGroupLayout.ConfigureSimulationInputs(groupBox5);
            InputGroupLayout.ConfigureSimulationOutputs(groupBox6, groupBox7);

            InitializeGaMethodInfo();
            InitializeRunStopButtons();
            InitializeSimulationTabLayout();
            groupBox8.Visible = false;

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
                "Fastest search — surrogate cost formula ranks combos, then verify finalists.",
                "• Search: analytical surrogate model (no discrete-event simulation)" + Environment.NewLine +
                "• Ranks all combinations in milliseconds" + Environment.NewLine +
                "• Final step: parallel seeded stochastic verify on top 5 candidates" + Environment.NewLine +
                "• Best when: very large bounds; trust final verify, not surrogate rank alone",
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

            tabControl1.Controls.Add(CreatePhaseTab(OptimizationPhaseDisplay.ExhaustiveSearch, _phase1Panel));
            tabControl1.Controls.Add(CreatePhaseTab(OptimizationPhaseDisplay.GeneticSearch, _phase2Panel));
            tabControl1.Controls.Add(CreatePhaseTab(OptimizationPhaseDisplay.SurrogateSearch, _phase3Panel));
            tabControl1.Controls.Add(CreatePhaseTab(OptimizationPhaseDisplay.DynamicProgrammingSearch, _phase4Panel));
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
            tabPage1.Controls.Add(_btnStopGenetic);

            _btnStopSim = new Button
            {
                Text = "Stop",
                Size = new Size(120, 40),
                Enabled = false,
                Visible = false   // simulation has no cooperative cancellation path yet
            };
            _btnStopSim.Click += (_, __) => _runCancellation?.Cancel();
            tabPage3.Controls.Add(_btnStopSim);
        }

        private void InitializeGaMethodInfo()
        {
            tabPage1.AutoScroll = true;
            _gaMethodInfoPanel = new GaMethodInfoPanel();
            tabPage1.Controls.Add(_gaMethodInfoPanel);

            Resize += (_, __) =>
            {
                LayoutGeneticAlgorithmTab();
                LayoutSimulationTab();
            };
            LayoutGeneticAlgorithmTab();
        }

        private void InitializeSimulationTabLayout()
        {
            tabPage3.AutoScroll = true;
            LayoutSimulationTab();
        }

        private void LayoutSimulationTab()
        {
            if (tabPage3 == null)
                return;

            const int margin = 10;
            int leftWidth = InputGroupLayout.RequiredWidth;
            const int gap = 16;
            const int inputHeight = 210;
            int rightX = margin + leftWidth + gap;
            int rightWidth = Math.Max(480, tabPage3.ClientSize.Width - rightX - margin);
            int tabHeight = Math.Max(680, tabPage3.ClientSize.Height);

            picbxsimulation.SetBounds(margin, margin, leftWidth, 175);
            groupBox5.SetBounds(margin, 195, leftWidth, inputHeight);

            int buttonY = 195 + inputHeight + 12;
            btnSim.SetBounds(margin, buttonY, 250, 40);
            btnClear.SetBounds(margin + 260, buttonY, 250, 40);
            _btnStopSim.SetBounds(margin + 520, buttonY, 120, 40);

            chart2.SetBounds(rightX, margin, rightWidth, Math.Min(320, tabHeight - margin * 2));
            int outputTop = Math.Max(340, margin + 320);
            int outputHeight = Math.Max(170, tabHeight - outputTop - margin);
            groupBox6.SetBounds(rightX, outputTop, 300, outputHeight);
            groupBox7.SetBounds(rightX + 310, outputTop, Math.Max(420, rightWidth - 310), outputHeight);
        }

        private void LayoutGeneticAlgorithmTab()
        {
            if (_gaMethodInfoPanel == null || tabPage1 == null)
                return;

            const int margin = 10;
            int leftWidth = InputGroupLayout.RequiredWidth;
            const int gap = 16;
            const int inputHeight = 310;
            int rightX = margin + leftWidth + gap;
            int rightWidth = Math.Max(480, tabPage1.ClientSize.Width - rightX - margin);
            int tabHeight = Math.Max(680, tabPage1.ClientSize.Height);

            picbxalgo.SetBounds(margin, margin, leftWidth, 175);
            groupBox1.SetBounds(margin, 195, leftWidth, inputHeight);

            int buttonY = 195 + inputHeight + 12;
            button1.SetBounds(margin, buttonY, 250, 52);
            btnRunGenetic.SetBounds(margin + 260, buttonY, 250, 52);
            _btnStopGenetic.SetBounds(margin + 520, buttonY, 120, 52);

            chart1.SetBounds(rightX, margin, rightWidth, 215);
            label3.SetBounds(rightX, 228, 130, 18);
            progressBar1.SetBounds(rightX + 135, 225, Math.Max(200, rightWidth - 135), 22);

            const int infoTop = 258;
            int infoHeight = Math.Max(300, tabHeight - infoTop - margin);
            _gaMethodInfoPanel.SetBounds(rightX, infoTop, rightWidth, infoHeight);
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
            if (panel == null || panel == except)
                return;
            panel.SetRunning(running);
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
