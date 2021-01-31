using RioParser.Domain.Logging;
using RioParser.Domain.Reports.Models;
using RioParser.Domain.Sessions;
using System;
using System.Collections.Generic;

namespace RioParser.Domain.Reports.SitAndGo
{
    public class SitAndGoReporter<TReport> : IReporter where TReport : IReport
    {
        private ReportOptions _reportOptions;
        private ILogger _logger;

        public SitAndGoReporter(ReportOptions reportOptions, ILogger logger)
        {
            _reportOptions = reportOptions;
            _logger = logger;
        }

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<SessionBase> sessions)
        {
            throw new NotImplementedException();
        }
    }
}
