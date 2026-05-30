using System.Drawing;
using System.Windows.Forms;
using GeneticAlgorithm.Core.Genetics;
using GeneticAlgorithm.Desktop.Views.Layout;

namespace GeneticAlgorithm.Desktop.Views
{
    /// <summary>
    /// Standard "Best result" block used on GA and phase comparison tabs.
    /// </summary>
    internal sealed class BestResultSummaryGroup : GroupBox
    {
        private readonly Label _valTrucks;
        private readonly Label _valLoaders;
        private readonly Label _valScalers;
        private readonly Label _valTotalCost;
        private readonly Label _valTotalDays;
        private readonly Label _valFitness;

        public BestResultSummaryGroup(string title)
        {
            Text = title;
            Padding = new Padding(12, 8, 12, 8);
            Font = AppLayoutMetrics.CaptionFont;
            MinimumSize = new Size(400, 110);
            Height = 118;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4,
                RowCount = 3,
                GrowStyle = TableLayoutPanelGrowStyle.FixedSize
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 72F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

            _valTrucks = AddRow(layout, 0, "Trucks", "—");
            _valLoaders = AddRow(layout, 1, "Loaders", "—");
            _valScalers = AddRow(layout, 2, "Scalers", "—");
            _valTotalCost = AddRow(layout, 0, "Total cost", "—", valueColumn: 3);
            _valTotalDays = AddRow(layout, 1, "Total days", "—", valueColumn: 3);
            _valFitness = AddRow(layout, 2, "Fitness", "—", valueColumn: 3);

            Controls.Add(layout);
        }

        public void Apply(DNA chromosome)
        {
            if (chromosome == null || chromosome.Genes.Count < 3)
            {
                Clear();
                return;
            }

            _valTrucks.Text = chromosome.Genes[0].ToString();
            _valLoaders.Text = chromosome.Genes[1].ToString();
            _valScalers.Text = chromosome.Genes[2].ToString();
            _valTotalCost.Text = chromosome.TotalCost.ToString("N2");
            _valTotalDays.Text = chromosome.TotalDays.ToString("N2");
            _valFitness.Text = chromosome.Fitness.ToString("N4");
        }

        public void Clear()
        {
            _valTrucks.Text = "—";
            _valLoaders.Text = "—";
            _valScalers.Text = "—";
            _valTotalCost.Text = "—";
            _valTotalDays.Text = "—";
            _valFitness.Text = "—";
        }

        private static Label AddRow(
            TableLayoutPanel layout,
            int row,
            string caption,
            string initialValue,
            int valueColumn = 1)
        {
            var captionLabel = new Label
            {
                Text = caption + ":",
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 0, 8, 0)
            };

            var valueLabel = new Label
            {
                Text = initialValue,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0)
            };

            layout.Controls.Add(captionLabel, valueColumn == 1 ? 0 : 2, row);
            layout.Controls.Add(valueLabel, valueColumn, row);
            return valueLabel;
        }
    }
}
