using System;
using RioParser.Domain.Logging;
using RioParser.Domain.Reports.Implementations;

namespace RioParser.Domain.Reports
{
    public class ReporterFactory
    {
        private readonly ILogger _logger;

        public ReporterFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IReporter Create(ReportType reportType) 
            => reportType switch
            {
                ReportType.RakeAndSplash => new Reporter<RakeAndSplashReport>(_logger),
                ReportType.Debug => new Reporter<DebugReport>(_logger),
                _ => throw new NotImplementedException()
            };
    }
}
