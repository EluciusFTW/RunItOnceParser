using RioParser.Domain;
using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using System.Threading.Tasks;
using RioParser.Domain.Reports;

namespace RioParser.Console
{
    class Program
    {
        private static readonly ConsoleLogger logger = new ConsoleLogger();

        /// <param name="gameType">Gametype to Analyze. Valid values: PLO, NLH</param>
        /// <param name="path" alias="p">Path to hand history folder</param>
        /// <param name="hero">Name of the hero</param>
        static async Task Main(string path = @"D:\Temp\RioHHs", string hero = "MiamiBlues", GameType gameType = GameType.PLO)
        {
            logger.Log("Path: " + path);
            logger.Log("heroName: " + hero);
            logger.Log("Assembly: " + System.Reflection.Assembly.GetExecutingAssembly().Location);
            logger.Log("Directory: " + System.IO.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            
            GenerateReport(gameType, path, hero);
        }

        static void GenerateReport(GameType gameType, string path, string hero)
        {
            var handhistoryFiles = new HandHistoryFileLoader().Load(path);
            logger.Log($"Found {handhistoryFiles.Count} files to process.");

            var reportsGenerator = new ReporterFactory(logger).Create(ReportType.RakeAndSplash);
            var reports = reportsGenerator.Process(handhistoryFiles, hero, GameType.PLO);
            logger.Log($"Finished processing {handhistoryFiles.Count} hand history files.");

            reports.ForEach(report => logger.Log(report.PrintOut()));
        }
    } 
}