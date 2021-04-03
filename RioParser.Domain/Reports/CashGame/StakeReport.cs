using RioParser.Domain.Extensions;
using RioParser.Domain.Hands;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using RioParser.Domain.Reports.Artefact;

namespace RioParser.Domain.Reports.CashGame
{
    public class StakeReport : IReport
    {
        private readonly ICollection<StakeReportBase> _reports;
        private readonly Dictionary<PerStakeReportTypes, Func<IReadOnlyCollection<CashGameHand>, string, StakeReportBase>> _reportMap
            = new()
            {
                { PerStakeReportTypes.Rake, (hands, hero) => new RakeReport(hero, hands) },
                { PerStakeReportTypes.Splash, (hands, hero) => new SplashReport(hero, hands) }
            };

        public StakeReport(ReportOptions reportOptions, IReadOnlyCollection<CashGameHand> hands)
        {
            _reports = reportOptions.ReportTypes
                .Select(type => _reportMap[type](hands, reportOptions.Hero))
                .ToList();
        }

        public IEnumerable<IReportArtefact> Artefacts()
        {
            var item = new Item(_reports.First().StakeSummary());

            return new[] {item}.Concat(_reports.SelectMany(r => r.Artifacts()));
        }

        //public IEnumerable<string> PrintOut()
        //{
        //    var builder = new StringBuilder();
        //    _reports.First().StakeSummary();
        //    _reports.ForEach(report => report.AppendReport(builder));

        //    return new[] { builder.ToString() };
        //}
    }
}
