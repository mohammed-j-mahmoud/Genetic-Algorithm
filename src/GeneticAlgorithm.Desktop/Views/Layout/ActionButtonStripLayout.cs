using System;
using System.Drawing;
using System.Windows.Forms;

namespace GeneticAlgorithm.Desktop.Views.Layout
{
    /// <summary>
    /// Lays out primary action buttons in two rows within a fixed-width left column.
    /// </summary>
    internal static class ActionButtonStripLayout
    {
        public static void ApplyTwoRowPrimaryActions(
            int columnWidth,
            int startY,
            Button primaryLeft,
            Button primaryRight,
            Button secondaryLeft,
            Button secondaryRight = null)
        {
            int margin = AppLayoutMetrics.Margin;
            int contentWidth = columnWidth - margin * 2;
            int halfWidth = Math.Max(160, (contentWidth - AppLayoutMetrics.ButtonGap) / 2);
            int row1Y = startY;
            int row2Y = row1Y + AppLayoutMetrics.PrimaryButtonHeight + AppLayoutMetrics.ButtonGap;

            Place(primaryLeft, margin, row1Y, halfWidth, AppLayoutMetrics.PrimaryButtonHeight);
            Place(primaryRight, margin + halfWidth + AppLayoutMetrics.ButtonGap, row1Y, halfWidth, AppLayoutMetrics.PrimaryButtonHeight);

            if (secondaryLeft != null)
                Place(secondaryLeft, margin, row2Y, halfWidth, AppLayoutMetrics.SecondaryButtonHeight);

            if (secondaryRight != null)
                Place(secondaryRight, margin + halfWidth + AppLayoutMetrics.ButtonGap, row2Y, halfWidth, AppLayoutMetrics.SecondaryButtonHeight);
        }

        private static void Place(Control control, int x, int y, int width, int height)
        {
            if (control == null)
                return;

            control.SetBounds(x, y, width, height);
        }
    }
}
