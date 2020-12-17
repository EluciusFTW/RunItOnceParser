using System;
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
            System.Console.WriteLine(message);
        }
        
        public void Chapter(string line)
        {
            System.Console.WriteLine($"*** {line}");
        }
        
        public void Paragraph(string line)
        {
            System.Console.WriteLine(Environment.NewLine + $"* {line}");
        }
    }
}
