using System.Drawing;
using System.Windows.Forms;

namespace GeneticAlgorithm.Desktop.Views
{
    /// <summary>
    /// Repositions input group controls so unit suffixes fit without clipping.
    /// </summary>
    internal static class InputGroupLayout
    {
        public const int RequiredWidth = 580;

        private const int LeftLabelX = 10;
        private const int LeftLabelWidth = 200;
        private const int LeftFieldX = 215;
        private const int RightLabelX = 305;
        private const int RightLabelWidth = 170;
        private const int RightFieldX = 478;
        private const int FieldWidth = 82;
        private const int RowHeight = 32;

        public static void ConfigureGeneticAlgorithmInputs(GroupBox groupBox, Button clearButton)
        {
            ConfigureSharedInputs(
                groupBox,
                labelMaterial: Find<Label>(groupBox, "label16"),
                labelLoad: Find<Label>(groupBox, "label19"),
                labelTruckCost: Find<Label>(groupBox, "label20"),
                labelLoaderCost: Find<Label>(groupBox, "label22"),
                labelScalerCost: Find<Label>(groupBox, "label21"),
                labelDuration: Find<Label>(groupBox, "label18"),
                labelDelay: Find<Label>(groupBox, "label17"),
                fieldMaterial: Find<TextBox>(groupBox, "txtbxmaterialAlgo"),
                fieldLoad: Find<TextBox>(groupBox, "txtbxlodrprtrukAlgo"),
                fieldTruckCost: Find<TextBox>(groupBox, "txtbxcostprtrukAlgo"),
                fieldLoaderCost: Find<TextBox>(groupBox, "txtbxCostPrLoderAlgo"),
                fieldScalerCost: Find<TextBox>(groupBox, "txtbxCostPrScalerAlgo"),
                fieldDuration: Find<TextBox>(groupBox, "txtbxProjectDurationAlgo"),
                fieldDelay: Find<TextBox>(groupBox, "txtbxCostOfDelayAlgo"));

            PlaceRow(Find<Label>(groupBox, "label15"), Find<TextBox>(groupBox, "txtbxTrukNoMxAlgo"), 0, right: true);
            PlaceRow(Find<Label>(groupBox, "label7"), Find<TextBox>(groupBox, "txtbxLosderNoMxAlgo"), 1, right: true);
            PlaceRow(Find<Label>(groupBox, "label6"), Find<TextBox>(groupBox, "txtbxScalerNoMxAlgo"), 2, right: true);
            PlaceRow(Find<Label>(groupBox, "label2"), Find<TextBox>(groupBox, "txtbxPopulationNo"), 3, right: true);
            PlaceRow(Find<Label>(groupBox, "label1"), Find<TextBox>(groupBox, "txtbxGenerationNo"), 4, right: true);
            PlaceRow(Find<Label>(groupBox, "label5"), Find<TextBox>(groupBox, "txtbxMutationRate"), 5, right: true);

            if (clearButton != null)
                clearButton.SetBounds(RightLabelX, (RowHeight * 7) + 6, RightFieldX + FieldWidth - RightLabelX, 33);
        }

        public static void ConfigureSimulationInputs(GroupBox groupBox)
        {
            PlaceRow(Find<Label>(groupBox, "label12"), Find<TextBox>(groupBox, "txtbxmaterialsim"), 0, right: false);
            PlaceRow(Find<Label>(groupBox, "label23"), Find<TextBox>(groupBox, "txtbxlodrprtruk"), 1, right: false);
            PlaceRow(Find<Label>(groupBox, "label24"), Find<TextBox>(groupBox, "txtbxcostprtruk"), 2, right: false);
            PlaceRow(Find<Label>(groupBox, "label26"), Find<TextBox>(groupBox, "txtbxCostPrLoder"), 3, right: false);
            PlaceRow(Find<Label>(groupBox, "label25"), Find<TextBox>(groupBox, "txtbxCostPrScaler"), 4, right: false);

            PlaceRow(Find<Label>(groupBox, "label11"), Find<TextBox>(groupBox, "txtbxTrukNO"), 0, right: true);
            PlaceRow(Find<Label>(groupBox, "label10"), Find<TextBox>(groupBox, "txtbxLoaderNo"), 1, right: true);
            PlaceRow(Find<Label>(groupBox, "label9"), Find<TextBox>(groupBox, "txtbxScalerNo"), 2, right: true);
            PlaceRow(Find<Label>(groupBox, "label14"), Find<TextBox>(groupBox, "txtbxProjectDuration"), 3, right: true);
            PlaceRow(Find<Label>(groupBox, "label13"), Find<TextBox>(groupBox, "txtbxCostOfDelay"), 4, right: true);

            if (groupBox != null)
                groupBox.MinimumSize = new Size(RequiredWidth, (RowHeight * 5) + 40);
        }

        public static void ConfigureSimulationOutputs(GroupBox utilizationBox, GroupBox costBox)
        {
            ConfigureUtilizationRow(utilizationBox, "label28", "UtilTrucks", 0);
            ConfigureUtilizationRow(utilizationBox, "label31", "UtilLoader", 1);
            ConfigureUtilizationRow(utilizationBox, "label32", "lblUtilScaler", 2);

            if (utilizationBox != null)
                utilizationBox.MinimumSize = new Size(290, 120);

            ConfigureCostRow(costBox, "label30", "lblprojectDuration", 0, valueColumn: 0);
            ConfigureCostRow(costBox, "label44", "lbldaysofdelayed", 0, valueColumn: 1);
            ConfigureCostRow(costBox, "label27", "lblTruckCost", 1, valueColumn: 0);
            ConfigureCostRow(costBox, "label29", "lblDelayCost", 1, valueColumn: 1);
            ConfigureCostRow(costBox, "label34", "lblLoaderCost", 2, valueColumn: 0);
            ConfigureCostRow(costBox, "label45", "lbltotalcost", 2, valueColumn: 1);
            ConfigureCostRow(costBox, "label33", "lblScalerCost", 3, valueColumn: 0);

            if (costBox != null)
                costBox.MinimumSize = new Size(420, 170);
        }

        private static void ConfigureSharedInputs(
            GroupBox groupBox,
            Label labelMaterial,
            Label labelLoad,
            Label labelTruckCost,
            Label labelLoaderCost,
            Label labelScalerCost,
            Label labelDuration,
            Label labelDelay,
            TextBox fieldMaterial,
            TextBox fieldLoad,
            TextBox fieldTruckCost,
            TextBox fieldLoaderCost,
            TextBox fieldScalerCost,
            TextBox fieldDuration,
            TextBox fieldDelay)
        {
            PlaceRow(labelMaterial, fieldMaterial, 0, right: false);
            PlaceRow(labelLoad, fieldLoad, 1, right: false);
            PlaceRow(labelTruckCost, fieldTruckCost, 2, right: false);
            PlaceRow(labelLoaderCost, fieldLoaderCost, 3, right: false);
            PlaceRow(labelScalerCost, fieldScalerCost, 4, right: false);
            PlaceRow(labelDuration, fieldDuration, 5, right: false);
            PlaceRow(labelDelay, fieldDelay, 6, right: false);

            if (groupBox != null)
                groupBox.MinimumSize = new Size(RequiredWidth, (RowHeight * 7) + 56);
        }

        private static void ConfigureUtilizationRow(GroupBox box, string labelName, string valueName, int row)
        {
            Label label = Find<Label>(box, labelName);
            Label value = Find<Label>(box, valueName);
            if (label == null || value == null)
                return;

            int y = 24 + (row * 30);
            StyleLabel(label, 8, y, 210);
            value.SetBounds(225, y, 55, 20);
        }

        private static void ConfigureCostRow(
            GroupBox box,
            string labelName,
            string valueName,
            int row,
            int valueColumn)
        {
            Label label = Find<Label>(box, labelName);
            Label value = Find<Label>(box, valueName);
            if (label == null || value == null)
                return;

            int y = 24 + (row * 30);
            if (valueColumn == 0)
            {
                StyleLabel(label, 8, y, 130);
                value.SetBounds(145, y, 70, 20);
            }
            else
            {
                StyleLabel(label, 230, y, 150);
                value.SetBounds(385, y, 70, 20);
            }
        }

        private static void PlaceRow(Label label, TextBox field, int row, bool right)
        {
            if (label == null || field == null)
                return;

            int y = 22 + (row * RowHeight);
            if (right)
            {
                StyleLabel(label, RightLabelX, y, RightLabelWidth);
                field.SetBounds(RightFieldX, y - 2, FieldWidth, 20);
            }
            else
            {
                StyleLabel(label, LeftLabelX, y, LeftLabelWidth);
                field.SetBounds(LeftFieldX, y - 2, FieldWidth, 20);
            }
        }

        private static void StyleLabel(Label label, int x, int y, int width)
        {
            label.AutoSize = false;
            label.SetBounds(x, y, width, 28);
        }

        private static T Find<T>(Control parent, string name) where T : Control
        {
            if (parent == null)
                return null;

            foreach (Control control in parent.Controls)
            {
                if (control.Name == name && control is T match)
                    return match;
            }

            return null;
        }
    }
}
