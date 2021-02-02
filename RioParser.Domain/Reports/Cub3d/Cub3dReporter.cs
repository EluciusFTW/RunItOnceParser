using RioParser.Domain.Logging;
using RioParser.Domain.Reports.Models;
using RioParser.Domain.Sessions;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Reports.SitAndGo
{
    public class Cub3dReporter : IReporter
    {
        private ReportOptions _reportOptions;
        private ILogger _logger;

        public Cub3dReporter(ReportOptions reportOptions, ILogger logger)
        {
            _reportOptions = reportOptions;
            _logger = logger;
        }

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<SessionBase> sessions)
        {
            var cub3dSessions = sessions
                .Cast<Cub3dSession>()
                .ToList();

            return new[] { new Cub3dReport(_reportOptions.Hero, cub3dSessions) };
        }
    }
}
