using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GeneticAlgorithm.Desktop.Views.Layout
{
    /// <summary>
    /// Applies consistent visual styling to WinForms controls.
    /// </summary>
    internal static class UiTheme
    {
        public static void ApplyForm(Form form)
        {
            form.Font = AppLayoutMetrics.BodyFont;
            form.BackColor = AppLayoutMetrics.FormBackColor;
            form.MinimumSize = new Size(AppLayoutMetrics.FormMinWidth, AppLayoutMetrics.FormMinHeight);
        }

        public static void StylePrimaryButton(Button button)
        {
            if (button == null)
                return;

            button.Height = AppLayoutMetrics.PrimaryButtonHeight;
            button.FlatStyle = FlatStyle.System;
            button.UseVisualStyleBackColor = true;
        }

        public static void StyleSecondaryButton(Button button)
        {
            if (button == null)
                return;

            button.Height = AppLayoutMetrics.SecondaryButtonHeight;
            button.FlatStyle = FlatStyle.System;
            button.UseVisualStyleBackColor = true;
        }

        public static void StyleGroupBox(GroupBox groupBox)
        {
            if (groupBox == null)
                return;

            groupBox.Padding = new Padding(10, 6, 10, 10);
            groupBox.Font = AppLayoutMetrics.CaptionFont;
        }

        public static void StyleSectionLabel(Label label, bool title = false)
        {
            if (label == null)
                return;

            label.Font = title ? AppLayoutMetrics.SectionTitleFont : AppLayoutMetrics.CaptionFont;
            label.ForeColor = title ? SystemColors.ControlText : AppLayoutMetrics.MutedTextColor;
        }

        public static void StyleDataGrid(DataGridView grid)
        {
            if (grid == null)
                return;

            grid.BackgroundColor = AppLayoutMetrics.PanelBackColor;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.ColumnHeadersDefaultCellStyle.Font = AppLayoutMetrics.CaptionFont;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.EnableHeadersVisualStyles = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            grid.RowHeadersVisible = false;
            grid.AllowUserToResizeRows = false;
            grid.ScrollBars = ScrollBars.Vertical;
        }

        public static void StyleChart(Chart chart)
        {
            if (chart == null)
                return;

            chart.BackColor = AppLayoutMetrics.PanelBackColor;
            chart.BorderlineColor = SystemColors.ControlDark;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
        }
    }
}
