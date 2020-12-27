using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.HandHistories;

namespace RioParser.Domain.Reports.Implementations
{
    public class RakeAndSplashReport : IHandsReport
    {
        private readonly IReadOnlyCollection<IHandsReport> _stakeReports;

        public RakeAndSplashReport(string hero, IReadOnlyCollection<HandHistory> hands)
        {
            _stakeReports = hands.GroupBy(hand => hand.BigBlind)
                .Select(group => new StakeReport(hero, group.ToList()))
                .ToList();
        }

        public string PrintOut() 
            => string.Join(Environment.NewLine, _stakeReports.Select(report => report.PrintOut()));
    }
}