using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RioParser.Domain.Reports.Implementations
{
    public class StakeReport : IHandsReport
    {
        private StakeReportBase[] _reports;

        public StakeReport(string hero, IReadOnlyCollection<HandHistory> hands)
        {
            _reports = new StakeReportBase[]
            {
                new RakeReport(hero, hands),
                new SplashReport(hero, hands)
            };
        }

        public string PrintOut()
        {
            var builder = new StringBuilder();
            _reports.First().AppendStakeReport(builder);
            _reports.ForEach(report => report.AppendReport(builder));

            return builder.ToString();
        }
    }
}
