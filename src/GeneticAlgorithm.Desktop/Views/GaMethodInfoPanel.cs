using System.Drawing;
using System.Windows.Forms;

namespace GeneticAlgorithm.Desktop.Views
{
    /// <summary>
    /// Right-side GA tab panel: method text, best result summary, and run stats.
    /// </summary>
    internal sealed class GaMethodInfoPanel : GroupBox
    {
        public GaMethodInfoPanel()
        {
            Text = "Method description and best result";
            Padding = new Padding(10, 4, 10, 10);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                AutoSize = false
            };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            MethodSummary = CreateTextLabel(
                "Classic GA — evolves a population over many generations using GeneticSharp.");
            HowItWorks = CreateTextLabel(
                "• Search: genetic algorithm (sample of search space, not every combo)" + System.Environment.NewLine +
                "• Simulation during search: seeded stochastic (full random load/weigh/travel)" + System.Environment.NewLine +
                "• Cache: yes — same fleet triple always gets the same random seed" + System.Environment.NewLine +
                "• Compare this tab to Exhaustive Search, Genetic Search, Surrogate Search, and Dynamic Programming Search using the result box below");

            BestResult = new BestResultSummaryGroup("Best result (seeded stochastic simulation)")
            {
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(0, 8, 0, 8)
            };

            RunStats = CreateTextLabel("Simulations: —   |   Cache hits: —");

            layout.Controls.Add(MethodSummary, 0, 0);
            layout.Controls.Add(HowItWorks, 0, 1);
            layout.Controls.Add(BestResult, 0, 2);
            layout.Controls.Add(RunStats, 0, 3);
            Controls.Add(layout);
        }

        public Label MethodSummary { get; }

        public Label HowItWorks { get; }

        public BestResultSummaryGroup BestResult { get; }

        public Label RunStats { get; }

        private static Label CreateTextLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                MaximumSize = new Size(900, 0),
                Margin = new Padding(0, 0, 0, 6)
            };
        }
    }
}
