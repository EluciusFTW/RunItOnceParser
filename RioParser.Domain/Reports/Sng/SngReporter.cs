using RioParser.Domain.Logging;
using RioParser.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Reports.Sng
{
    public class SngReporter : IReporter
    {
        private readonly ReportOptions _reportOptions;
        private readonly ILogger _logger;

        public SngReporter(ReportOptions reportOptions, ILogger logger)
        {
            _reportOptions = reportOptions;
            _logger = logger;
        }

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<SessionBase> sessions)
        {
            if (string.IsNullOrEmpty(_reportOptions.Hero))
            {
                _logger.Log("The cub3d report only makes sense if you provide a hero. Please do.");
                return Array.Empty<IReport>();
            }

            var sngSessions = sessions
                .Cast<SngSession>()
                .Where(session => session.Hands.First().Players.Contains(_reportOptions.Hero))
                .ToList();

            if (sngSessions.Any())
            {
                return new[] {new SngReport(_reportOptions.Hero, sngSessions)};
            }
            
            _logger.Paragraph($"No Sng tourneys found where {_reportOptions.Hero} played.");
            return Array.Empty<IReport>();
        }
    }
}
