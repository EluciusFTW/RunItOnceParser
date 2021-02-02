using System;
using RioParser.Domain.Logging;
using RioParser.Domain.Reports.CashGame;
using RioParser.Domain.Reports.Implementations;
using RioParser.Domain.Reports.Models;
using RioParser.Domain.Reports.SitAndGo;
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
                // { IsCub3d: true } => new SitAndGoReporter<Cub3dReport>(reportOptions, _logger),
                { IsPerStakeReport: true } => new CashGameReporter<StakesReport>(reportOptions, _logger),
                { IsDebug: true } => new CashGameReporter<DebugReport>(reportOptions, _logger),
                { SessionType: SessionType.Cub3d } => new Cub3dReporter(reportOptions, _logger),
                _ => throw new NotImplementedException()
            };
    }
}
