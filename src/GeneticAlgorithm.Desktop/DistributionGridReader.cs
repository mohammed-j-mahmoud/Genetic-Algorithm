using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GeneticAlgorithm.Application;

namespace GeneticAlgorithm.Desktop
{
    internal static class DistributionGridReader
    {
        public static List<KeyValuePair<int, double>> Read(DataGridView grid, string gridName)
        {
            var entries = new List<KeyValuePair<int, double>>();

            for (int i = 0; i < grid.Rows.Count - 1; i++)
            {
                DataGridViewRow row = grid.Rows[i];
                if (row.IsNewRow)
                    continue;

                if (!FormInputParser.TryParseGridCellInt(row.Cells[0].Value, out int minutes)
                    || !FormInputParser.TryParseGridCellDouble(row.Cells[1].Value, out double weight))
                {
                    throw new InvalidOperationException($"{gridName} row {i + 1} must have a positive time and probability.");
                }

                entries.Add(new KeyValuePair<int, double>(minutes, weight));
            }

            return DistributionNormalizer.Normalize(entries);
        }
    }
}
