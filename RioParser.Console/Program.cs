using RioParser.Domain;
using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Reports;

namespace RioParser.Console
{
    class Program
    {
        private static readonly ConsoleLogger logger = new ConsoleLogger();

        /// <param name="gameType">Game type to analyze. Valid values: PLO, NLH</param>
        /// <param name="path" alias="p">Path of folder where the hand histories can be found</param>
        /// <param name="hero">Name of the hero</param>
        static void Main(string path = @"C:\Users\bussg\EluciusFTW\RunItOnceParser\Sample\HandHistoryBatch", string hero = "MiamiBlues", GameType gameType = GameType.PLO)
        {
            logger.Log("Path: " + path);
            logger.Log("heroName: " + hero);
            logger.Log("Assembly: " + System.Reflection.Assembly.GetExecutingAssembly().Location);
            
            GenerateReport(gameType, path, hero);
        }

        private static void GenerateReport(GameType gameType, string path, string hero)
        {
            var handHistoryFiles = new HandHistoryFileLoader().Load(path);
            logger.Log($"Found {handHistoryFiles.Count} files to process.");

            var reportsGenerator = new ReporterFactory(logger).Create(ReportType.RakeAndSplash);
            var reports = reportsGenerator.Process(handHistoryFiles, hero, GameType.PLO);
            logger.Log($"Finished processing {handHistoryFiles.Count} hand history files.");

            reports.ForEach(report => logger.Log(report.PrintOut()));
        }
    } 
}