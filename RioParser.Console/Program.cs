using System.Diagnostics;
using System.Linq;
using RioParser.Domain;
using RioParser.Domain.Reports;
using RioParser.Domain.Reports.Models;
using RioParser.Domain.Sessions;

namespace RioParser.Console
{
    class Program
    {
        private static readonly ConsoleLogger Logger = new();

        /// <param name="path">Required: Path of folder where the hand histories can be found.</param>
        /// <param name="gameType">Game type to analyze. Valid values: PLO, NLH</param>
        /// <param name="reportType">Report type to run. Valid values: Rake, Splash, RakeAndSplash.</param>
        /// <param name="hero">Name of the hero. If none is provided, only general stats will be computed.</param>
        /// <param name="verbose">Set to true for more detailed output</param>
        static void Main(
            string hero,
            string path = "d:\\Temp\\RioCub3d",
            bool verbose = false,
            GameType gameType = GameType.PLO,
            ReportType reportType = ReportType.Cub3d)
        {
            ConsoleLogger.SetVerbosity(verbose);
            LogApplicationStart(path, hero, reportType, gameType, verbose);
            
            if (string.IsNullOrEmpty(path))
            {
                Logger.Paragraph($"A path is required, please provide it via \"--{nameof(path)} <path to hand history folder>\".");
                return;
            }
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
            var sessions = new SessionLoader()
                .Load(path, options.SessionType);
            
            if (!sessions.Any())
            {
                Logger.Paragraph($"Didn't find any hand history files in {path} of type {options.SessionType}.");
                return;
            }

            Logger.Paragraph($"Loaded {sessions.Count} hand history files to memory in {stopwatch.Elapsed} seconds.");
            var reports = new ReporterFactory(Logger)
                .Create(options)
                .Process(sessions);
            
            Logger.Log($"Finished processing {sessions.Count} hand history files after {stopwatch.Elapsed} seconds.");

            if (reports.Any())
            {
                Logger.Paragraph($"Writing out reports.");
                Logger.LogAlternating(reports.SelectMany(report => report.PrintOut()));
                Logger.Paragraph($"Finished reports after {stopwatch.Elapsed} seconds.");
            }
        }
    }
}