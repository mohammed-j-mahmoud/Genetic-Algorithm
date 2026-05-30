using System;
using System.Threading;

namespace GeneticAlgorithm.Core.Optimization
{
    /// <summary>
    /// Thread-safe, monotonic progress reporting for parallel search loops.
    /// </summary>
    internal static class ParallelProgress
    {
        public static void ReportCompletion(
            IProgress<int> progress,
            ref int completed,
            ref int lastReportedPercent,
            int total)
        {
            int done = Interlocked.Increment(ref completed);
            if (progress == null || total <= 0)
                return;

            int percent = done * 100 / total;
            while (percent > Volatile.Read(ref lastReportedPercent))
            {
                int previous = lastReportedPercent;
                if (Interlocked.CompareExchange(ref lastReportedPercent, percent, previous) == previous)
                {
                    progress.Report(percent);
                    break;
                }
            }
        }
    }
}
