using RioParser.Domain.HandHistories;
using RioParser.Domain.Logging;
using RioParser.Domain.Reports.Models;
using System;
using System.Collections.Generic;

namespace RioParser.Domain.Reports.SitAndGo
{
    public class SitAndGoReporter<TReport> : IReporter where TReport : IReport
    {
        private ReportOptions reportOptions;
        private ILogger logger;

        public SitAndGoReporter(ReportOptions reportOptions, ILogger logger)
        {
            this.reportOptions = reportOptions;
            this.logger = logger;
        }

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<HandHistoryFile> handHistoryFiles)
        {
            throw new NotImplementedException();
        }
    }
}
