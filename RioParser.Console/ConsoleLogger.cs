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
        public ConsoleLogger()
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public void Log(string message)
        {
            Log(message, Color.Aquamarine);
        }

        private void Log(string message, Color color)
        {
            System.Console.WriteLine(message.Pastel(color));
        }
        
        public void Chapter(string line)
        {
            System.Console.WriteLine($"*** {line}".Pastel(Color.Goldenrod));
        }
        
        public void Paragraph(string line)
        {
            System.Console.WriteLine(Environment.NewLine + $"* {line}".Pastel(Color.DarkSalmon));
        }

        internal void LogAlternating(IEnumerable<string> messages)
        {
            messages.ForEach((message, index) => 
                {
                    var color = index % 2 == 0
                        ? Color.PaleVioletRed 
                        : Color.Plum;
                    Log(message, color);
                });
        }
    }
}
