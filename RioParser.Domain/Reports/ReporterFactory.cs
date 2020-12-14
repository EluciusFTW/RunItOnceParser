using System;
using RioParser.Domain.Logging;

namespace RioParser.Domain.Reports
{
    public class ReporterFactory
    {
        private ILogger _logger;

        public ReporterFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IReporter Create(ReportType reportType) 
            => reportType switch
            {
                ReportType.RakeAndSplash => new RakeAndSplashReporter(_logger),
                _ => throw new NotImplementedException()
            };
    }
}
