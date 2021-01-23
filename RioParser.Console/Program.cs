using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using RioParser.Domain;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Reports;
using RioParser.Domain.Reports.Models;

namespace RioParser.Console
{
    class Program
    {
        private static readonly ConsoleLogger Logger = new();

        /// <param name="gameType">Game type to analyze. Valid values: PLO, NLH</param>
        /// <param name="reportType">Report type to run. Valid values: Rake, Splash, RakeAndSplash.</param>
        /// <param name="path">Path of folder where the hand histories can be found</param>
        /// <param name="hero">Name of the hero</param>
        /// <param name="verbose">Set to true for more detailed output</param>
        static void Main(
            string path,
            string hero, 
            bool verbose = false,
            GameType gameType = GameType.PLO,
            ReportType reportType = ReportType.RakeAndSplash)
        {
            ConsoleLogger.SetVerbosity(verbose);
            
            path ??= SamplePath();
            LogApplicationStart(path, hero, reportType, gameType, verbose);
            var options = new ReportOptions(hero, gameType, reportType);

            GenerateReport(path, options);
        }
        
        private static void LogApplicationStart(string path, string hero, ReportType reportType, GameType gameType, bool verbose)
        {
            Logger.Chapter("RioParser: Parse your cash game hand histories played on Run It Once Poker!");

            Logger.Log("Stay up to date with newest development and features by");
            Logger.Log(" * following me on Twitter (@EluciusFTW)");
            Logger.Log(" * visiting the GitHub page (https://github.com/EluciusFTW/RunItOnceParser)");

            Logger.Paragraph("Configuration");
            Logger.Log("- Output:                  " + (verbose ? "verbose" : "terse"));
            Logger.Log("- Report type:             " + reportType);
            Logger.Log("- Game type:               " + gameType);
            Logger.Log("- Hero name:               " + hero);
            Logger.Log("- Hand history files path: " + path);
        }

        private static void GenerateReport(string path, ReportOptions options)
        {
            var stopwatch = Stopwatch.StartNew();
            var handHistoryFiles = new HandHistoryFileLoader()
                .Load(path);
            
            if (!handHistoryFiles.Any())
            {
                Logger.Paragraph($"Didn't find any hand history files in {path}.");
                return;
            }

            Logger.Paragraph($"Loaded {handHistoryFiles.Count} hand history files to memory in {stopwatch.Elapsed} seconds.");
            var reports = new ReporterFactory(Logger)
                .Create(options)
                .Process(handHistoryFiles);
            
            Logger.Log($"Finished processing {handHistoryFiles.Count} hand history files after {stopwatch.Elapsed} seconds.");

            if (reports.Any())
            {
                Logger.Paragraph($"Writing out reports.");
                Logger.LogAlternating(reports.SelectMany(report => report.PrintOut()));
                Logger.Paragraph($"Finished reports after {stopwatch.Elapsed} seconds.");
            }
        }
        private static string SamplePath()
        {
            var fi = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            return Path.Combine(fi.Split("RioParser.Console")[0], "Sample\\HandHistoryBatch");
        }
    }
}