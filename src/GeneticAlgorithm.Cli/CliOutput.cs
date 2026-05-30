using System;

namespace GeneticAlgorithm.Cli
{
    /// <summary>
    /// Single place for all CLI console-output primitives so every help class
    /// writes through the same channel (SRP, DRY).
    /// </summary>
    internal static class CliOutput
    {
        internal static void WriteTitle(string text)
        {
            Console.WriteLine(text);
            Console.WriteLine(new string('=', text.Length));
        }

        internal static void WriteSubTitle(string text)
        {
            Console.WriteLine(text);
            Console.WriteLine(new string('-', text.Length));
        }

        internal static void WriteLine(string text) => Console.WriteLine(text);

        internal static void WriteBlankLine() => Console.WriteLine();
    }
}
