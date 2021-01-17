using System;
using RioParser.Domain.Logging;
using RioParser.Domain.Reports.Implementations;
using RioParser.Domain.Reports.Models;

namespace RioParser.Domain.Reports
{
    public class ReporterFactory
    {
        private readonly ILogger _logger;

        public ReporterFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IReporter Create(ReportOptions reportOptions) 
            => reportOptions switch
            {
                { IsPerStakeReport: true } => new Reporter<PerStakeReport>(reportOptions, _logger),
                { IsDebug: true } => new Reporter<DebugReport>(reportOptions, _logger),
                _ => throw new NotImplementedException()
            };
    }
}
