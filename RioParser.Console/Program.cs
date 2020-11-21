using RioParser.Console;
using RioParser.Domain;
using RioParser.Domain.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var handhistoryFiles = LoadFiles(@"D:\Temp\RioHHs");
System.Console.WriteLine($"Found {handhistoryFiles.Count} files to process.");

var reports = Process(handhistoryFiles, "MiamiBlues");
System.Console.WriteLine($"Finished processing hand histories.");

reports.ForEach(report => System.Console.WriteLine(report.PrintOut()));
System.Console.ReadKey();

static IReadOnlyCollection<HandsReport> Process(IReadOnlyCollection<HandHistoryFile> handhistoryFiles, string hero)
    => handhistoryFiles
        .SelectMany(file =>
        {
            System.Console.WriteLine($"Processing {file.Name} containing {file.Hands.Count} hands.");
            return file.Hands;
        })
        .GroupBy(hand => hand.BigBlind)
        .Select(hands => new HandsReport(hero, hands.ToList()))
        .ToList();

static IReadOnlyCollection<HandHistoryFile> LoadFiles(string path)
    => new DirectoryInfo(path)
        .GetFiles()
        .Where(MatchesHandHistoryFileFormat)
        .Select(fileInfo => new HandHistoryFile(fileInfo))
        .ToList();

static bool MatchesHandHistoryFileFormat(FileInfo fileInfo)
    => fileInfo.Extension == ".txt";
