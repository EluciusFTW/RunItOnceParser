using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Logging;

namespace RioParser.Domain.Reports
{
    internal class RakeAndSplashReporter : IReporter
    {
        private readonly ILogger _logger;
    
        public RakeAndSplashReporter(ILogger logger)
        {
            _logger = logger;
        }

        public IReadOnlyCollection<IHandsReport> Process(IReadOnlyCollection<HandHistoryFile> handHistoryFiles, string hero, GameType gameType)
            => handHistoryFiles
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
}