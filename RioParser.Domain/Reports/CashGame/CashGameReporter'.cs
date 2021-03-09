using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.Logging;
using RioParser.Domain.Extensions;
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
            var filteredSessions = Filter(sessions);
            if (!filteredSessions.Any())
            {
                _logger.Log($"No files left to process.");
                return Array.Empty<IReport>();
            }

            var hands = filteredSessions
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

        private IReadOnlyCollection<CashGameSession> Filter(IEnumerable<SessionBase> sessions)
        {
            var (ofCorrectGameType, ofWrongGameType) = sessions
                .Cast<CashGameSession>()
                .ToList()
                .Split(file => file.Hands.First().Game == _reportOptions.GameType);

            if (ofWrongGameType.Any())
            {
                _logger.Log($"Ignoring {ofWrongGameType.Count} files because of different game type.");
            }

            return ofCorrectGameType.ToList();
        }
    }
}