using System;
using System.Diagnostics;
using System.Linq;
using RioParser.Console.Logging;
using RioParser.Domain;
using RioParser.Domain.Reports;
using RioParser.Domain.Sessions;

namespace RioParser.Console
{
    class Program
    {
        private static readonly SpectreLogger Logger = new();

        /// <param name="path">Required: Path of folder where the hand histories can be found.</param>
        /// <param name="gameType">Game type to analyze. Valid values: PLO, NLH</param>
        /// <param name="reportType">Report type to run. Valid values: Rake, Splash, RakeAndSplash.</param>
        /// <param name="hero">Name of the hero. If none is provided, only general stats will be computed.</param>
        /// <param name="verbose">Set to true for more detailed output</param>
        static void Main(
            string hero,
            string path,
            bool verbose = false,
            GameType gameType = GameType.Unknown,
            ReportType reportType = ReportType.Unknown)
        {
            ConsoleLogger.SetVerbosity(verbose);
            LogApplicationStart();
            
            var (configSuccess, options) = new ReportOptionsBuilder(Logger)
                .Build(path, hero, reportType, gameType, verbose);

            if (!configSuccess)
            {
                Environment.Exit(0);
            }

            GenerateReport(options);
        }

        private static void LogApplicationStart()
        {
            var title = "RioParser: Analyze your hand histories played on Run It Once Poker!";
            var contents = new[]
            {
                "Stay up to date with newest development and features by",
                "- following me on Twitter (@EluciusFTW)",
                "- visiting the GitHub page (https://github.com/EluciusFTW/RunItOnceParser)"
            };
            Logger.LogGroup(title, contents);
        }

        private static void GenerateReport(ReportOptions options)
        {
            var stopwatch = Stopwatch.StartNew();
            var sessions = new SessionLoader()
                .Load(options.Path, options.SessionType);
            
            if (!sessions.Any())
            {
                Logger.Paragraph($"Didn't find any hand history files in {options.Path} of type {options.SessionType}.");
                return;
            }

            Logger.Paragraph($"Loaded {sessions.Count} hand history files to memory in {stopwatch.Elapsed} seconds.");
            var reports = new ReporterFactory(Logger)
                .Create(options)
                .Process(sessions);
            
            Logger.Log($"Finished processing {sessions.Count} hand history files after {stopwatch.Elapsed} seconds.");

            if (reports.Any())
            {
                var artefacts = reports.SelectMany(report => report.Artefacts());
                Logger.LogArtefacts(artefacts);

                Logger.Paragraph($"Finished reports after {stopwatch.Elapsed} seconds.");
            }
        }
    }
}