using RioParser.Domain.Reports.CashGame;
using System.Linq;

namespace RioParser.Domain.Reports.Models
{
    public class ReportOptions
    {
        public GameType GameType { get; }
        public PerStakeReportTypes[] ReportTypes { get; private set; }
        public string Hero { get; }
        public bool IsPerStakeReport => ReportTypes.Any();
        public bool IsDebug { get; }
        public bool IsCub3d { get; }
        
        public ReportOptions(string hero, GameType gameType, ReportType reportType)
        {
            Hero = hero;
            GameType = gameType;
            ReportTypes = reportType switch
            {
                ReportType.Rake => new[] { PerStakeReportTypes.Rake },
                ReportType.Splash => new[] { PerStakeReportTypes.Splash },
                ReportType.RakeAndSplash => new[] { PerStakeReportTypes.Rake, PerStakeReportTypes.Splash },
                _ => System.Array.Empty<PerStakeReportTypes>()
            };
            IsCub3d = reportType == ReportType.Cub3d;
            IsDebug = reportType == ReportType.Debug;
        }
    }
}
