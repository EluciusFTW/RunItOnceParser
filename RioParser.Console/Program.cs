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
            var handhistoryFiles = LoadFiles();
            System.Console.WriteLine($"Found {handhistoryFiles.Count} files to process.");

            Process(handhistoryFiles);
            System.Console.WriteLine($"Finished processing.");

            System.Console.ReadKey();
        }

        private static void Process(IReadOnlyCollection<HandHistoryFile> handhistoryFiles)
        {
            handhistoryFiles
                .SelectMany(file =>
                {
                    System.Console.WriteLine($"Processing {file.Name} containing {file.Hands.Count} hands.");
                    return file.Hands;
                })
                .GroupBy(hand => hand.BigBlind)
                .ForEach(hands =>
                {
                    var report = new HandsReport("MiamiBlues", hands.ToList());
                    System.Console.WriteLine(report.PrintOut());
                });
        }

        private static IReadOnlyCollection<HandHistoryFile> LoadFiles()
        {
            var directory = @"D:\Temp\RioHHs";
            return new DirectoryInfo(directory)
                .GetFiles()
                .Where(MatchesHandHistoryFileFormat)
                .Select(fileInfo => new HandHistoryFile(fileInfo))
                .ToList();
        }

        private static bool MatchesHandHistoryFileFormat(FileInfo fileInfo)
        {
            return fileInfo.Extension == ".txt";
        }
    }
}
