using System.IO;
using System.Linq;
using System.Reflection;
using RioParser.Domain;
using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Reports;
using RioParser.Domain.Reports.Models;

namespace RioParser.Console
{
    class Program
    {
        private static readonly ConsoleLogger Logger = new();

        /// <param name="gameType">Game type to analyze. Valid values: PLO, NLH</param>
        /// <param name="reportType">Report type to run. Valid values: Debug, RakeAndSplash</param>
        /// <param name="path">Path of folder where the hand histories can be found</param>
        /// <param name="hero">Name of the hero</param>
        static void Main(
            string path, 
            string hero = "MiamiBlues", 
            GameType gameType = GameType.PLO,
            ReportType reportType = ReportType.RakeAndSplash)
        {
            path ??= Path.Combine(Assembly.GetExecutingAssembly().Location.Split("RioParser.Console")[0], "Sample\\HandHistoryBatch");
            LogApplicationStart(path, hero, reportType, gameType);

            var options = new ReportOptions(hero, gameType, reportType);
            GenerateReport(path, options);
        }

        private static void LogApplicationStart(string path, string hero, ReportType reportType, GameType gameType)
        {
            Logger.Chapter("Running RioParser");
            Logger.Paragraph("Configuration");
            Logger.Log("- Report type:             " + reportType);
            Logger.Log("- Game type:               " + gameType);
            Logger.Log("- Hero name:               " + hero);
            Logger.Log("- Hand history files path: " + path);
        }

        private static void GenerateReport(string path, ReportOptions options)
        {
            var handHistoryFiles = new HandHistoryFileLoader()
                .Load(path);
            
            Logger.Paragraph($"Found {handHistoryFiles.Count} files to process");
            var reports = new ReporterFactory(Logger)
                .Create(options)
                .Process(handHistoryFiles);
            
            Logger.Paragraph($"Finished processing {handHistoryFiles.Count} hand history files");
            Logger.LogAlternating(reports.Select(report => report.PrintOut()));
        }
    } 
}