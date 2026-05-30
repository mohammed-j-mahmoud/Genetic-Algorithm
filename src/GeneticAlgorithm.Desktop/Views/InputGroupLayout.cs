using System.Drawing;
using System.Windows.Forms;
using GeneticAlgorithm.Desktop.Views.Layout;

namespace GeneticAlgorithm.Desktop.Views
{
    /// <summary>
    /// Repositions input group controls so unit suffixes fit without clipping.
    /// </summary>
    internal static class InputGroupLayout
    {
        public const int RequiredWidth = AppLayoutMetrics.InputColumnWidth;

        private const int LeftLabelX = 10;
        private const int LeftLabelWidth = 200;
        private const int LeftFieldX = 215;
        private const int RightLabelX = 305;
        private const int RightLabelWidth = 170;
        private const int RightFieldX = 478;
        private const int FieldWidth = 82;
        private const int RowHeight = 32;

        public static void ConfigureGeneticAlgorithmInputs(
            GroupBox groupBox,
            Label lblMaterialVolume,
            Label lblLoadPerTruck,
            Label lblTruckCostPerDay,
            Label lblLoaderCostPerDay,
            Label lblScalerCostPerDay,
            Label lblProjectDuration,
            Label lblDelayCostPerDay,
            TextBox txtMaterialVolume,
            TextBox txtLoadPerTruck,
            TextBox txtTruckCostPerDay,
            TextBox txtLoaderCostPerDay,
            TextBox txtScalerCostPerDay,
            TextBox txtProjectDuration,
            TextBox txtDelayCostPerDay,
            Label lblMaxTrucks,
            TextBox txtMaxTrucks,
            Label lblMaxLoaders,
            TextBox txtMaxLoaders,
            Label lblMaxScalers,
            TextBox txtMaxScalers,
            Label lblPopulation,
            TextBox txtPopulation,
            Label lblGenerations,
            TextBox txtGenerations,
            Label lblMutationRate,
            TextBox txtMutationRate)
        {
            ConfigureSharedInputs(
                lblMaterialVolume,
                lblLoadPerTruck,
                lblTruckCostPerDay,
                lblLoaderCostPerDay,
                lblScalerCostPerDay,
                lblProjectDuration,
                lblDelayCostPerDay,
                txtMaterialVolume,
                txtLoadPerTruck,
                txtTruckCostPerDay,
                txtLoaderCostPerDay,
                txtScalerCostPerDay,
                txtProjectDuration,
                txtDelayCostPerDay);

            PlaceRow(lblMaxTrucks, txtMaxTrucks, 0, right: true);
            PlaceRow(lblMaxLoaders, txtMaxLoaders, 1, right: true);
            PlaceRow(lblMaxScalers, txtMaxScalers, 2, right: true);
            PlaceRow(lblPopulation, txtPopulation, 3, right: true);
            PlaceRow(lblGenerations, txtGenerations, 4, right: true);
            PlaceRow(lblMutationRate, txtMutationRate, 5, right: true);

            if (groupBox != null)
                groupBox.MinimumSize = new Size(RequiredWidth, (RowHeight * 7) + 28);
        }

        public static void ConfigureSimulationInputs(
            GroupBox groupBox,
            Label lblMaterialVolume,
            Label lblLoadPerTruck,
            Label lblTruckCostPerDay,
            Label lblLoaderCostPerDay,
            Label lblScalerCostPerDay,
            Label lblTruckCount,
            TextBox txtMaterialVolume,
            TextBox txtLoadPerTruck,
            TextBox txtTruckCostPerDay,
            TextBox txtLoaderCostPerDay,
            TextBox txtScalerCostPerDay,
            TextBox txtTruckCount,
            Label lblLoaderCount,
            TextBox txtLoaderCount,
            Label lblScalerCount,
            TextBox txtScalerCount,
            Label lblProjectDuration,
            TextBox txtProjectDuration,
            Label lblDelayCostPerDay,
            TextBox txtDelayCostPerDay)
        {
            ConfigureSharedInputs(
                lblMaterialVolume,
                lblLoadPerTruck,
                lblTruckCostPerDay,
                lblLoaderCostPerDay,
                lblScalerCostPerDay,
                lblProjectDuration,
                lblDelayCostPerDay,
                txtMaterialVolume,
                txtLoadPerTruck,
                txtTruckCostPerDay,
                txtLoaderCostPerDay,
                txtScalerCostPerDay,
                txtProjectDuration,
                txtDelayCostPerDay);

            PlaceRow(lblTruckCount, txtTruckCount, 0, right: true);
            PlaceRow(lblLoaderCount, txtLoaderCount, 1, right: true);
            PlaceRow(lblScalerCount, txtScalerCount, 2, right: true);
            PlaceRow(lblProjectDuration, txtProjectDuration, 3, right: true);
            PlaceRow(lblDelayCostPerDay, txtDelayCostPerDay, 4, right: true);

            if (groupBox != null)
                groupBox.MinimumSize = new Size(RequiredWidth, (RowHeight * 5) + 40);
        }

        public static void ConfigureSimulationOutputs(
            GroupBox utilizationBox,
            GroupBox costBox,
            Label lblUtilTrucksCaption,
            Label lblUtilTrucksValue,
            Label lblUtilLoadersCaption,
            Label lblUtilLoadersValue,
            Label lblUtilScalersCaption,
            Label lblUtilScalersValue,
            Label lblProjectDurationCaption,
            Label lblProjectDurationValue,
            Label lblDaysDelayedCaption,
            Label lblDaysDelayedValue,
            Label lblTruckCostCaption,
            Label lblTruckCostValue,
            Label lblDelayCostCaption,
            Label lblDelayCostValue,
            Label lblLoaderCostCaption,
            Label lblLoaderCostValue,
            Label lblTotalCostCaption,
            Label lblTotalCostValue,
            Label lblScalerCostCaption,
            Label lblScalerCostValue)
        {
            ConfigureUtilizationRow(lblUtilTrucksCaption, lblUtilTrucksValue, 0);
            ConfigureUtilizationRow(lblUtilLoadersCaption, lblUtilLoadersValue, 1);
            ConfigureUtilizationRow(lblUtilScalersCaption, lblUtilScalersValue, 2);

            if (utilizationBox != null)
                utilizationBox.MinimumSize = new Size(290, 120);

            ConfigureCostRow(lblProjectDurationCaption, lblProjectDurationValue, 0, valueColumn: 0);
            ConfigureCostRow(lblDaysDelayedCaption, lblDaysDelayedValue, 0, valueColumn: 1);
            ConfigureCostRow(lblTruckCostCaption, lblTruckCostValue, 1, valueColumn: 0);
            ConfigureCostRow(lblDelayCostCaption, lblDelayCostValue, 1, valueColumn: 1);
            ConfigureCostRow(lblLoaderCostCaption, lblLoaderCostValue, 2, valueColumn: 0);
            ConfigureCostRow(lblTotalCostCaption, lblTotalCostValue, 2, valueColumn: 1);
            ConfigureCostRow(lblScalerCostCaption, lblScalerCostValue, 3, valueColumn: 0);

            if (costBox != null)
                costBox.MinimumSize = new Size(420, 170);
        }

        private static void ConfigureSharedInputs(
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
        }

        private static void ConfigureUtilizationRow(Label label, Label value, int row)
        {
            if (label == null || value == null)
                return;

            int y = 24 + (row * 30);
            StyleLabel(label, 8, y, 210);
            value.SetBounds(225, y, 55, 20);
        }

        private static void ConfigureCostRow(Label label, Label value, int row, int valueColumn)
        {
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
    }
}
