using RioParser.Domain.Logging;
using RioParser.Domain.Reports.Models;
using RioParser.Domain.Sessions;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Reports.Sng
{
    public class SngReporter : IReporter
    {
        private ReportOptions _reportOptions;
        private ILogger _logger;

        public SngReporter(ReportOptions reportOptions, ILogger logger)
        {
            _reportOptions = reportOptions;
            _logger = logger;
        }

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<SessionBase> sessions)
        {
            var sngSessions = sessions
                .Cast<SngSession>()
                .ToList();

            return new[] { new SngReport(_reportOptions.Hero, sngSessions) };
        }
    }
}
