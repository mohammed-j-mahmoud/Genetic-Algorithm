using System.Drawing;
using System.Windows.Forms;

namespace GeneticAlgorithm.Desktop.Views.Layout
{
    /// <summary>
    /// Single source of truth for spacing, sizing, and typography across the desktop UI.
    /// </summary>
    internal static class AppLayoutMetrics
    {
        public const int Margin = 12;
        public const int ColumnGap = 16;
        public const int SectionGap = 10;
        public const int ButtonGap = 10;
        public const int MinRightColumnWidth = 480;
        public const int InputColumnWidth = 580;
        public const int HeroImageHeight = 175;
        public const int PrimaryButtonHeight = 44;
        public const int SecondaryButtonHeight = 36;
        public const int StopButtonWidth = 120;
        public const int FormMinWidth = 1200;
        public const int FormMinHeight = 680;
        public const int FormDefaultWidth = 1320;
        public const int FormDefaultHeight = 780;

        public static readonly Font BodyFont = SystemFonts.MessageBoxFont;
        public static readonly Font SectionTitleFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 10F, FontStyle.Bold);
        public static readonly Font CaptionFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 9F, FontStyle.Regular);
        public static readonly Color FormBackColor = SystemColors.Control;
        public static readonly Color PanelBackColor = Color.White;
        public static readonly Color MutedTextColor = SystemColors.GrayText;
    }
}
