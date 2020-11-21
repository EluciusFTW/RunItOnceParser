using RioParser.Domain;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Logging;
using RioParser.Domain.Reports;
using System.Collections.Generic;
using System.Linq;

internal class RakeAndSplashReporter : IReporter
{
    private ILogger _logger;
    
    public RakeAndSplashReporter(ILogger logger)
    {
        _logger = logger;
    }

    public IReadOnlyCollection<IHandsReport> Process(IReadOnlyCollection<HandHistoryFile> handhistoryFiles, string hero, GameType gameType)
        => handhistoryFiles
            .SelectMany(file =>
            {
                _logger.Log($"Processing {file.Name} containing {file.Hands.Count} hands.");
                return file.Hands;
            })
            .Where(hand => hand.Game == gameType)
            .GroupBy(hand => hand.BigBlind)
            .Select(hands => new RakeAndSplashReport(hero, hands.ToList()))
            .ToList();
}