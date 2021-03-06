﻿using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.Artefact;
using RioParser.Domain.Hands;

namespace RioParser.Domain.Reports.CashGame
{
    public class StakesReport : IReport
    {
        private readonly IReadOnlyCollection<IReport> _stakeReports;

        public StakesReport(ReportOptions reportOptions, IEnumerable<CashGameHand> hands)
        {
            _stakeReports = hands
                .GroupBy(hand => hand.BigBlind)
                .Select(group => new StakeReport(reportOptions, group.ToList()))
                .ToList();
        }

        public IEnumerable<IReportArtefact> Artefacts()
            => _stakeReports.SelectMany(report => report.Artefacts());
    }
}