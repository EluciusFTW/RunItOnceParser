using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.Hands;
using RioParser.Domain.Reports.Models;

namespace RioParser.Domain.Reports.CashGame
{
    public class StakesReport : IReport
    {
        private readonly IReadOnlyCollection<IReport> _stakeReports;

        public StakesReport(ReportOptions reportOptions, IReadOnlyCollection<CashGameHand> hands)
        {
            _stakeReports = hands
                .GroupBy(hand => hand.BigBlind)
                .Select(group => new StakeReport(reportOptions, group.ToList()))
                .ToList();
        }

        public IEnumerable<string> PrintOut()
            => _stakeReports.Select(report => string.Join(Environment.NewLine, report.PrintOut()));
    }
}