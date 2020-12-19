using System;
using System.Drawing;
using Pastel;
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
            System.Console.WriteLine(message.Pastel(Color.Aquamarine));
        }
        
        public void Chapter(string line)
        {
            System.Console.WriteLine($"*** {line}".Pastel(Color.Goldenrod));
        }
        
        public void Paragraph(string line)
        {
            System.Console.WriteLine(Environment.NewLine + $"* {line}".Pastel(Color.DarkSalmon));
        }
    }
}
