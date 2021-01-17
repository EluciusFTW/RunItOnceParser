using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.HandHistories;
using RioParser.Domain.Reports.Models;

namespace RioParser.Domain.Reports.Implementations
{
    public class PerStakeReport : IHandsReport
    {
        private readonly IReadOnlyCollection<IHandsReport> _stakeReports;

        public PerStakeReport(ReportOptions reportOptions, IReadOnlyCollection<HandHistory> hands)
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