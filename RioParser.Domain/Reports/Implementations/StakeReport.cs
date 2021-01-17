using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Reports.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace RioParser.Domain.Reports.Implementations
{
    public class StakeReport : IHandsReport
    {
        private ICollection<StakeReportBase> _reports;
        private Dictionary<PerStakeReportTypes, Func<IReadOnlyCollection<HandHistory>, string, StakeReportBase>> reports 
            = new Dictionary<PerStakeReportTypes, Func<IReadOnlyCollection<HandHistory>, string, StakeReportBase>>
            {
                { PerStakeReportTypes.Rake, (hands, hero) => new RakeReport(hero, hands) },
                { PerStakeReportTypes.Splash, (hands, hero) => new SplashReport(hero, hands) }
            };

        public StakeReport(ReportOptions reportOptions, IReadOnlyCollection<HandHistory> hands)
        {
            _reports = reportOptions.ReportTypes
                .Select(type => reports[type](hands, reportOptions.Hero))
                .ToList();
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
