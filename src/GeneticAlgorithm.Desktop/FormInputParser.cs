using System;
using System.Globalization;
using System.Windows.Forms;

namespace GeneticAlgorithm.Desktop
{
    /// <summary>
    /// Culture-invariant parsing and validation for WinForms numeric inputs.
    /// </summary>
    internal static class FormInputParser
    {
        public static bool TryParseInt(string text, string fieldName, out int value, bool showErrors = true) =>
            TryParseIntCore(text, fieldName, out value, showErrors, valueMustBePositive: true);

        public static bool TryParseNonNegativeInt(string text, string fieldName, out int value, bool showErrors = true) =>
            TryParseIntCore(text, fieldName, out value, showErrors, valueMustBePositive: false);

        public static bool TryParseFloat(string text, string fieldName, out float value, bool showErrors = true)
        {
            value = 0f;
            if (!float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            {
                if (showErrors)
                    MessageBox.Show($"{fieldName} must be a number.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (value < 0f)
            {
                if (showErrors)
                    MessageBox.Show($"{fieldName} cannot be negative.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        public static bool TryParseMutationRate(string text, out double rate, bool showErrors = true)
        {
            rate = 0.01;
            if (string.IsNullOrWhiteSpace(text))
                return true;

            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out rate))
            {
                if (showErrors)
                    MessageBox.Show("Mutation rate must be a number between 0 and 1.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (rate < 0 || rate > 1)
            {
                if (showErrors)
                    MessageBox.Show("Mutation rate must be between 0 and 1.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        public static bool TryParseGridCellInt(object cellValue, out int value)
        {
            value = 0;
            if (cellValue == null)
                return false;

            return int.TryParse(Convert.ToString(cellValue, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out value)
                && value > 0;
        }

        public static bool TryParseGridCellDouble(object cellValue, out double value)
        {
            value = 0;
            if (cellValue == null)
                return false;

            return double.TryParse(Convert.ToString(cellValue, CultureInfo.InvariantCulture), NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                && value > 0;
        }

        private static bool TryParseIntCore(string text, string fieldName, out int value, bool showErrors, bool valueMustBePositive)
        {
            value = 0;
            if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
            {
                if (showErrors)
                    MessageBox.Show($"{fieldName} must be a whole number.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (valueMustBePositive && value <= 0)
            {
                if (showErrors)
                    MessageBox.Show($"{fieldName} must be greater than zero.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!valueMustBePositive && value < 0)
            {
                if (showErrors)
                    MessageBox.Show($"{fieldName} cannot be negative.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
