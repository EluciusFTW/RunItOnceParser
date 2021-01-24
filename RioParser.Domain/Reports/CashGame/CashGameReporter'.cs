using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Logging;
using RioParser.Domain.Extensions;
using RioParser.Domain.Reports.Models;

namespace RioParser.Domain.Reports.CashGame
{
    internal class CashGameReporter<TReport> : IReporter where TReport : IReport
    {
        private readonly ILogger _logger;
        private readonly ReportOptions _reportOptions;

        public CashGameReporter(ReportOptions reportOptions, ILogger logger)
        {
            _reportOptions = reportOptions;
            _logger = logger;
        }

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<HandHistoryFile> handHistoryFiles)
        {
            var (sngFiles, cashFiles) = handHistoryFiles
                .Split(file => file.Hands.First().Cubed);

            if (sngFiles.Any())
            {
                _logger.Log($"Ignoring {sngFiles.Count()} files because they are not cash game files.");
            }

            if (!cashFiles.Any())
            {
                _logger.Log($"No files left to process.");
                return Array.Empty<IReport>();
            }

            var (ofCorrectGameType, ofWrongGameType) = cashFiles
                .Split(file => file.Hands.First().Game == _reportOptions.GameType);

            if (ofWrongGameType.Any())
            {
                _logger.Log($"Ignoring {ofWrongGameType.Count()} files because of different game type.");
            }

            if (!ofCorrectGameType.Any())
            {
                _logger.Log($"No files left to process.");
                return Array.Empty<IReport>();
            }

            var hands = cashFiles
                .SelectMany(file =>
                {
                    _logger.Verbose($"Processing {file.Name} containing {file.Hands.Count} hands.");
                    return file.Hands;
                })
                .ToList();

            return new[] { (TReport)Activator.CreateInstance(typeof(TReport), _reportOptions, hands) }
                .Cast<IReport>()
                .ToList();
        }
    }
}