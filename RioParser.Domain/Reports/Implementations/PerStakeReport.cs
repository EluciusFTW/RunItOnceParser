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

        public string PrintOut() 
            => string.Join(Environment.NewLine + Environment.NewLine, _stakeReports.Select(report => report.PrintOut()));
    }
}