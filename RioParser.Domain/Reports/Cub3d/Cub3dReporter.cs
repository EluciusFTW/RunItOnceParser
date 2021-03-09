using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.Logging;
using RioParser.Domain.Sessions;

namespace RioParser.Domain.Reports.Cub3d
{
    public class Cub3dReporter : IReporter
    {
        private readonly ReportOptions _reportOptions;
        private readonly ILogger _logger;

        public Cub3dReporter(ReportOptions reportOptions, ILogger logger)
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

            var cub3dSessions = sessions
                .Cast<Cub3dSession>()
                .Where(session => session.Hands.First().Players.Contains(_reportOptions.Hero))
                .ToList();

            if (cub3dSessions.Any())
            {
                return new[] {new Cub3dReport(_reportOptions.Hero, cub3dSessions)};
            }
            
            _logger.Paragraph($"No Cub3d tourneys found where {_reportOptions.Hero} played.");
            return Array.Empty<IReport>();
        }
    }
}
