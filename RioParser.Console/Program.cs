using RioParser.Console;
using RioParser.Domain;
using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using System.CommandLine;
using System.CommandLine.Invocation;

var logger = new ConsoleLogger();

var reportCommand = new RootCommand("RioParser Report Generator")
{
    new Option<GameType>("--game", getDefaultValue: () => GameType.PLO, description: "Gametype to Analyze"),
    new Option<string>("--path", getDefaultValue: () => @"D:\Temp\RioHHs", "Path to hand history folder"),
    new Option<string>("--hero", getDefaultValue: () => "MiamiBlues", "Name of the hero")
};
reportCommand.Handler = CommandHandler.Create<GameType, string, string>(GenerateReport);

return reportCommand.InvokeAsync(args).Result;

void GenerateReport(GameType gameType, string path, string hero)
{
    var handhistoryFiles = new HandHistoryFileLoader().Load(path);
    logger.Log($"Found {handhistoryFiles.Count} files to process.");

    var reportsGenerator = new ReporterFactory(logger).Create(ReportType.RakeAndSplash);
    var reports = reportsGenerator.Process(handhistoryFiles, hero, GameType.PLO);
    logger.Log($"Finished processing {handhistoryFiles.Count} hand history files.");

    reports.ForEach(report => logger.Log(report.PrintOut()));
};
