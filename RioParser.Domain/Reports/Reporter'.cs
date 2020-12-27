using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Logging;
using RioParser.Domain.Reports.Models;

namespace RioParser.Domain.Reports
{
    internal class Reporter<TReport> : IReporter where TReport: IHandsReport
    {
        private readonly ILogger _logger;
        private readonly ReportOptions _reportOptions;
    
        public Reporter(ReportOptions reportOptions, ILogger logger)
        {
            _reportOptions = reportOptions;
            _logger = logger;
        }

        public IReadOnlyCollection<IHandsReport> Process(IReadOnlyCollection<HandHistoryFile> handHistoryFiles)
            => handHistoryFiles
                .SelectMany(file =>
                {
                    _logger.Log($"Processing {file.Name} containing {file.Hands.Count} hands.");
                    return file.Hands;
                })
                .Where(hand => hand.Game == _reportOptions.GameType)
                .GroupBy(hand => hand.BigBlind)
                .Select(hands => (TReport)Activator.CreateInstance(typeof(TReport), _reportOptions, hands.ToList()))
                .Cast<IHandsReport>()
                .ToList();
    }
}