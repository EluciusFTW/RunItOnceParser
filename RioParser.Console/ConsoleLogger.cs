using System;
using System.Collections.Generic;
using System.Drawing;
using Pastel;
using RioParser.Domain.Extensions;
using RioParser.Domain.Logging;

namespace RioParser.Console
{
    internal class ConsoleLogger : ILogger
    {
        private readonly Color chapterColor = Color.DarkSalmon;
        private readonly Color paragraphColor = Color.DarkSalmon;
        private readonly Color mainColor = Color.Aquamarine;
        
        private readonly Color alternatingColorOne = Color.PaleVioletRed;
        private readonly Color alternatingColorTwo = Color.Plum;
        
        private static bool _isVerbose;

        public ConsoleLogger()
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
       
        public static void SetVerbose() => _isVerbose = true;

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
            => System.Console.WriteLine(Environment.NewLine + $"* {line}".Pastel(paragraphColor));

        internal void LogAlternating(IEnumerable<string> messages) 
            => messages.ForEach((message, index) => Log(message, GetAlternatingColor(index)));

        private static void Log(string message, Color color)
            => System.Console.WriteLine(message.Pastel(color));

        private Color GetAlternatingColor(int index)
           => index % 2 == 0
               ? alternatingColorOne
               : alternatingColorTwo;
    }
}
