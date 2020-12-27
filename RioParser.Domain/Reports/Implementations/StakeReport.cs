using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Reports.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RioParser.Domain.Reports.Implementations
{
    public class StakeReport : IHandsReport
    {
        private StakeReportBase[] _reports;

        public StakeReport(ReportOptions reportOptions, IReadOnlyCollection<HandHistory> hands)
        {
            _reports = new StakeReportBase[]
            {
                new RakeReport(reportOptions.Hero, hands),
                new SplashReport(reportOptions.Hero, hands)
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
