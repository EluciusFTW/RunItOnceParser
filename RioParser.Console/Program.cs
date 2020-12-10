using RioParser.Console;
using RioParser.Domain;
using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using System.CommandLine;
using System.CommandLine.Invocation;

var logger = new ConsoleLogger();

var reportCommand = new RootCommand("RioParser Report Generator")
{
    GameTypeOption(),
    PathOption(),
    HeroNameOption()
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

Option<GameType> GameTypeOption()
{
    var option = new Option<GameType>("--game", getDefaultValue: () => GameType.PLO, description: "Gametype to Analyze");
    option.AddAlias("-g");
    return option;
}

Option<string> PathOption()
{
    var option = new Option<string>("--path", getDefaultValue: () => @"C:\Users\bussg\EluciusFTW\RunItOnceParser\Sample\HandHistoryBatch", "Path to hand history folder");
    option.AddAlias("-p");
    return option;
}

Option<string> HeroNameOption()
{
    var option = new Option<string>("--hero", getDefaultValue: () => "MiamiBlues", "Name of the hero");
    option.AddAlias("-h");
    return option;
}