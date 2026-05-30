using System;
using System.Drawing;

namespace GeneticAlgorithm.Desktop.Views.Layout
{
    /// <summary>
    /// Computes left/right column geometry for two-column tabs (GA, Simulation).
    /// </summary>
    internal readonly struct SplitTabLayout
    {
        public int Margin { get; }
        public int LeftWidth { get; }
        public int RightX { get; }
        public int RightWidth { get; }
        public int TabWidth { get; }
        public int TabHeight { get; }

        public SplitTabLayout(int tabWidth, int tabHeight, int leftWidthPreference)
        {
            Margin = AppLayoutMetrics.Margin;
            int gap = AppLayoutMetrics.ColumnGap;
            TabWidth = Math.Max(FormMinInnerWidth(leftWidthPreference), tabWidth);
            TabHeight = Math.Max(520, tabHeight);
            LeftWidth = leftWidthPreference;
            RightX = Margin + LeftWidth + gap;
            RightWidth = Math.Max(AppLayoutMetrics.MinRightColumnWidth, TabWidth - RightX - Margin);
        }

        public Rectangle LeftColumn(int top, int height) =>
            new Rectangle(Margin, top, LeftWidth, height);

        public Rectangle RightColumn(int top, int height) =>
            new Rectangle(RightX, top, RightWidth, height);

        public int ContentBottom(int top, int contentHeight) => top + contentHeight;

        public Rectangle ChartArea(int top, double heightRatio)
        {
            int height = Math.Max(180, (int)((TabHeight - Margin * 2) * heightRatio));
            return new Rectangle(RightX, top, RightWidth, height);
        }

        public Rectangle StackedPanelArea(Rectangle upperArea, int gap = 0)
        {
            int top = upperArea.Bottom + (gap > 0 ? gap : AppLayoutMetrics.SectionGap);
            int height = Math.Max(160, TabHeight - top - Margin);
            return new Rectangle(RightX, top, RightWidth, height);
        }

        private static int FormMinInnerWidth(int leftWidth) =>
            leftWidth + AppLayoutMetrics.Margin * 2 + AppLayoutMetrics.ColumnGap + AppLayoutMetrics.MinRightColumnWidth;
    }
}
