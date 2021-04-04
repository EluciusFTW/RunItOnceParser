using System;
using System.Collections.Generic;
using System.Drawing;
using Pastel;
using RioParser.Domain.Extensions;
using RioParser.Domain.Logging;

namespace RioParser.Console.Logging
{
    internal class ConsoleLogger : ILogger
    {
        private readonly Color chapterColor = Color.DarkSalmon;
        private readonly Color paragraphColor = Color.DarkSalmon;
        private readonly Color mainColor = Color.Aquamarine;

        private readonly Color alternatingColorOne = Color.Linen;
        private readonly Color alternatingColorTwo = Color.PeachPuff;

        private static bool _isVerbose;
        private static bool _paragraphBeginning = false;

        public ConsoleLogger()
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public static void SetVerbosity(bool verbosity) => _isVerbose = verbosity;

        public void Log(string message) => Log(message, mainColor);

        public void Verbose(string message)
        {
            if (_isVerbose)
            {
                Log(message);
            }
        }

        public void Chapter(string line)
            => System.Console.WriteLine($"*** {line}".Pastel(chapterColor));

        public void Paragraph(string line)
        {
            var potentialNewLine = !_paragraphBeginning
                ? Environment.NewLine
                : string.Empty;

            System.Console.WriteLine(potentialNewLine + $"* {line}".Pastel(paragraphColor));
            _paragraphBeginning = true;
        }

        internal void LogAlternating(IEnumerable<string> messages)
            => messages.ForEach((message, index) => Log(message, GetAlternatingColor(index)));


        private static void Log(string message, Color color)
        {
            _paragraphBeginning = false;
            System.Console.WriteLine(message.Pastel(color));
        }

        private Color GetAlternatingColor(int index)
           => index % 2 == 0
               ? alternatingColorOne
               : alternatingColorTwo;
    }
}
