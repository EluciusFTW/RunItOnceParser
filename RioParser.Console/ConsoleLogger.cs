using RioParser.Domain.Logging;

namespace RioParser.Console
{
    internal class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}
