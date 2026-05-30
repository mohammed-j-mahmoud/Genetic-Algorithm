using System;
using System.Drawing;
using System.Windows.Forms;
using GeneticAlgorithm.Application.Models;
using GeneticAlgorithm.Desktop.Views.Layout;

namespace GeneticAlgorithm.Desktop.Views
{
    /// <summary>
    /// Phase comparison tab: method description, run control, stats, and best result summary.
    /// </summary>
    internal sealed class PhaseResultPanel : Panel
    {
        private readonly Label _lblStats;
        private readonly Button _btnRun;
        private readonly Button _btnStop;
        private readonly BestResultSummaryGroup _results;
        private readonly ProgressBar _progress;
        private readonly Font _titleFont;

        public event EventHandler RunRequested;
        public event EventHandler StopRequested;

        public PhaseResultPanel(
            string title,
            string summary,
            string howItWorks,
            string resultsGroupTitle)
        {
            Dock = DockStyle.Fill;
            Padding = new Padding(AppLayoutMetrics.Margin + 4);
            AutoScroll = true;
            BackColor = AppLayoutMetrics.FormBackColor;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                AutoSize = false
            };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _titleFont = AppLayoutMetrics.SectionTitleFont;
            var lblTitle = new Label
            {
                Text = title,
                Font = _titleFont,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };

            var lblSummary = CreateTextLabel(summary, muted: false);
            var lblHowItWorks = CreateTextLabel(howItWorks, muted: true);

            var runRow = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Margin = new Padding(0, 8, 0, 8)
            };

            _btnRun = new Button
            {
                Text = "Run " + title,
                AutoSize = true,
                MinimumSize = new Size(180, AppLayoutMetrics.PrimaryButtonHeight),
                Margin = new Padding(0, 0, AppLayoutMetrics.ButtonGap, AppLayoutMetrics.ButtonGap)
            };
            _btnRun.Click += (_, __) => RunRequested?.Invoke(this, EventArgs.Empty);
            UiTheme.StylePrimaryButton(_btnRun);

            _btnStop = new Button
            {
                Text = UiCopy.Stop,
                AutoSize = true,
                MinimumSize = new Size(AppLayoutMetrics.StopButtonWidth, AppLayoutMetrics.SecondaryButtonHeight),
                Enabled = false,
                Margin = new Padding(0, 0, AppLayoutMetrics.ButtonGap, AppLayoutMetrics.ButtonGap)
            };
            _btnStop.Click += (_, __) => StopRequested?.Invoke(this, EventArgs.Empty);
            UiTheme.StyleSecondaryButton(_btnStop);

            _progress = new ProgressBar
            {
                Width = 420,
                Height = 22,
                Visible = false,
                Margin = new Padding(0, 6, 0, 0)
            };

            runRow.Controls.Add(_btnRun);
            runRow.Controls.Add(_btnStop);
            runRow.Controls.Add(_progress);

            _lblStats = CreateTextLabel("Simulations: —   |   Cache hits: —   |   Combinations: —", muted: true);

            _results = new BestResultSummaryGroup(resultsGroupTitle)
            {
                Dock = DockStyle.Fill,
                MinimumSize = new Size(500, 118),
                Margin = new Padding(0, 8, 0, 0)
            };

            layout.Controls.Add(lblTitle, 0, 0);
            layout.Controls.Add(lblSummary, 0, 1);
            layout.Controls.Add(lblHowItWorks, 0, 2);
            layout.Controls.Add(runRow, 0, 3);
            layout.Controls.Add(_lblStats, 0, 4);
            layout.Controls.Add(_results, 0, 5);

            Controls.Add(layout);
        }

        public void SetRunning(bool running)
        {
            _btnRun.Enabled = !running;
            _btnStop.Enabled = running;
            _progress.Visible = running;
            if (running)
                _progress.Value = 0;
        }

        public void SetBusy(bool busy)
        {
            _btnRun.Enabled = !busy;
        }

        public void ReportProgress(int percent)
        {
            _progress.Value = Math.Max(0, Math.Min(100, percent));
        }

        public void ApplyResult(OptimizationRunResult result)
        {
            if (result?.BestChromosome == null)
            {
                ClearResult();
                return;
            }

            _results.Apply(result.BestChromosome);
            string stoppedNote = result.StoppedEarly
                ? "Stopped early — showing best result found so far." + Environment.NewLine
                : string.Empty;
            _lblStats.Text =
                stoppedNote +
                $"Simulations: {result.SimulationCalls}   |   Cache hits: {result.CacheHits}   |   " +
                $"Combinations: {result.CombinationsEvaluated}{Environment.NewLine}{result.MethodSummary}";
        }

        public void ClearResult()
        {
            _results.Clear();
            _lblStats.Text = "Simulations: —   |   Cache hits: —   |   Combinations: —";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !ReferenceEquals(_titleFont, AppLayoutMetrics.SectionTitleFont))
                _titleFont.Dispose();
            base.Dispose(disposing);
        }

        private static Label CreateTextLabel(string text, bool muted)
        {
            var label = new Label
            {
                Text = text,
                AutoSize = true,
                MaximumSize = new Size(980, 0),
                Margin = new Padding(0, 0, 0, 4)
            };
            UiTheme.StyleSectionLabel(label, title: !muted);
            return label;
        }
    }
}
