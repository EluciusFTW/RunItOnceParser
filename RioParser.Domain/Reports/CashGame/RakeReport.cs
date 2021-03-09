﻿using RioParser.Domain.Extensions;
using RioParser.Domain.Hands;
using System.Collections.Generic;
using System.Text;

namespace RioParser.Domain.Reports.CashGame
{
    public class RakeReport : StakeReportBase
    {
        private decimal _totalRake;
        private decimal _heroRake;
        private readonly decimal _relativeHeroRake;

        public RakeReport(string hero, IReadOnlyCollection<CashGameHand> hands)
            : base(hero, hands)
        {
            hands.ForEach(hand => ParseHand(hero, hand));
            _relativeHeroRake = _heroRake * _factor;
        }

        public override void AppendReport(StringBuilder builder)
        {
            builder
                .AppendLine("Rake")
                .AppendLine($" - total:                 {_totalRake:F2}€");

            if (_includeHeroStatistics)
            {
                builder
                    .AppendLine($" - by hero:               {_heroRake:F2}€")
                    .AppendLine($" - by hero in BB/100:     {_relativeHeroRake:F2}");
            }
        }

        protected override void ParseHand(string hero, CashGameHand hand)
        {
            _totalRake += hand.Rake;
            if (_includeHeroStatistics && !hand.BigSplash && hand.Winner == hero)
            {
                _heroRake += hand.Rake;
            }
        }
    }
}
