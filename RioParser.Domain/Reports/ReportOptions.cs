using System.Linq;
using RioParser.Domain.Reports.CashGame;
using RioParser.Domain.Sessions;

namespace RioParser.Domain.Reports
{
    public class ReportOptions
    {
        public GameType GameType { get; }
        public bool Verbose { get; }
        public string Path { get; }
        public PerStakeReportTypes[] ReportTypes { get; private set; }
        public string Hero { get; }
        public bool IsPerStakeReport => ReportTypes.Any();
        public SessionType SessionType { get;  }
        
        public ReportOptions(string path, string hero, GameType gameType, ReportType reportType, bool verbose)
        {
            Path = path;
            Hero = hero;
            GameType = gameType;
            Verbose = verbose;
            ReportTypes = reportType switch
            {
                ReportType.Rake => new[] { PerStakeReportTypes.Rake },
                ReportType.Splash => new[] { PerStakeReportTypes.Splash },
                ReportType.RakeAndSplash => new[] { PerStakeReportTypes.Rake, PerStakeReportTypes.Splash },
                _ => System.Array.Empty<PerStakeReportTypes>()
            };

            SessionType = reportType switch
            {
                ReportType.Cub3d => SessionType.Cub3d,
                ReportType.Sng => SessionType.Sng,
                _ => SessionType.Cash
            };
        }
    }
}
