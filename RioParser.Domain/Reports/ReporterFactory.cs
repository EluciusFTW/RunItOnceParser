using System;
using RioParser.Domain.Logging;
using RioParser.Domain.Reports.CashGame;
using RioParser.Domain.Reports.Cub3d;
using RioParser.Domain.Reports.Sng;
using RioParser.Domain.Sessions;

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
                { IsPerStakeReport: true } => new CashGameReporter<StakesReport>(reportOptions, _logger),
                { SessionType: SessionType.Cub3d } => new Cub3dReporter(reportOptions, _logger),
                { SessionType: SessionType.Sng } => new SngReporter(reportOptions, _logger),
                _ => throw new NotImplementedException()
            };
    }
}
