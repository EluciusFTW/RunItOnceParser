using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Logging;

namespace RioParser.Domain.Reports
{
    internal class Reporter<TReport> : IReporter where TReport: IHandsReport
    {
        private readonly ILogger _logger;
    
        public Reporter(ILogger logger)
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
                .Select(hands => (TReport)Activator.CreateInstance(typeof(TReport), hero, hands.ToList()))
                .Cast<IHandsReport>()
                .ToList();
    }
}