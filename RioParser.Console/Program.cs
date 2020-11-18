using RioParser.Console.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RioParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var handhistoryFiles = LoadFiles(@"D:\Temp\RioHHs");
            System.Console.WriteLine($"Found {handhistoryFiles.Count} files to process.");

            var reports = Process(handhistoryFiles, "MiamiBlues");
            System.Console.WriteLine($"Finished processing hand histories.");

            reports.ForEach(report => System.Console.WriteLine(report.PrintOut()));
            System.Console.ReadKey();
        }

        private static IReadOnlyCollection<HandsReport> Process(IReadOnlyCollection<HandHistoryFile> handhistoryFiles, string hero) 
            => handhistoryFiles
                .SelectMany(file =>
                {
                    System.Console.WriteLine($"Processing {file.Name} containing {file.Hands.Count} hands.");
                    return file.Hands;
                })
                .GroupBy(hand => hand.BigBlind)
                .Select(hands => new HandsReport(hero, hands.ToList()))
                .ToList();

        private static IReadOnlyCollection<HandHistoryFile> LoadFiles(string path) 
            => new DirectoryInfo(path)
                .GetFiles()
                .Where(MatchesHandHistoryFileFormat)
                .Select(fileInfo => new HandHistoryFile(fileInfo))
                .ToList();

        private static bool MatchesHandHistoryFileFormat(FileInfo fileInfo) 
            => fileInfo.Extension == ".txt";
    }
}
