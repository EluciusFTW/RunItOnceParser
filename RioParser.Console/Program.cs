using RioParser.Console;
using RioParser.Domain;
using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;

var logger = new ConsoleLogger();
var handhistoryFiles = new HandHistoryFileLoader()
    .Load(@"D:\Temp\RioHHs");

logger.Log($"Found {handhistoryFiles.Count} files to process.");

var reportsGenerator = new ReporterFactory(logger)
    .Create(ReportType.RakeAndSplash);
var reports = reportsGenerator.Process(handhistoryFiles, "MiamiBlues", GameType.PLO);
logger.Log($"Finished processing hand histories.");

reports.ForEach(report => logger.Log(report.PrintOut()));
System.Console.ReadKey();
