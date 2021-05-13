using System;
using RioParser.Domain.Reports.CashGame;
using RioParser.Domain.Reports.Cub3d;
using RioParser.Domain.Reports.Sng;
using RioParser.Domain.Sessions;

namespace RioParser.Domain.Reports
{
    public class ReporterFactory
    {
        public static IReporter Create(ReportOptions reportOptions) 
            => reportOptions switch
            {
                { IsPerStakeReport: true } => new CashGameReporter<StakesReport>(reportOptions),
                { SessionType: SessionType.Cub3d } => new Cub3dReporter(reportOptions),
                { SessionType: SessionType.Sng } => new SngReporter(reportOptions),
                _ => throw new NotImplementedException()
            };
    }
}
