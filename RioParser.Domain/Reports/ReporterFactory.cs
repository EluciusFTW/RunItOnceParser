using RioParser.Domain.Logging;
using System;

public class ReporterFactory
{
    private ILogger _logger;

    public ReporterFactory(ILogger logger)
    {
        _logger = logger;
    }

    public IReporter Create(ReportType reportType)
    {
        return reportType switch
        {
            ReportType.RakeAndSplash => new RakeAndSplashReporter(_logger),
            _ => throw new NotImplementedException()
        };
    }
}
