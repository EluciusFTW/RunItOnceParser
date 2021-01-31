using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.Logging;
using RioParser.Domain.Extensions;
using RioParser.Domain.Reports.Models;
using RioParser.Domain.Sessions;

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

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<SessionBase> sessions)
        {
            var cashGameSessions = sessions
                .Cast<CashGameSession>();

            var (ofCorrectGameType, ofWrongGameType) = cashGameSessions
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

            var hands = cashGameSessions
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