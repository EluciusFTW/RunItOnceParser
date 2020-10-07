using System.IO;
using System.Linq;

namespace RioParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = @"D:\Temp\RioHHs";
            var files = Directory.GetFiles(directory);

            foreach(var f in files)
            {
                System.Console.WriteLine("Found: " + f);
                System.Console.WriteLine(Parse(f));
            }

            System.Console.ReadKey();
        }

        private static string Parse(string f)
        {
            var content = File.ReadAllText(f);
            var hhFile = new HandHistoryFile(content);
            return hhFile.PrintOut();
        }
    }
}
