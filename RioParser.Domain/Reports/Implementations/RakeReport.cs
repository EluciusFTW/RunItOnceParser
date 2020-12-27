using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using System.Collections.Generic;
using System.Text;

namespace RioParser.Domain.Reports.Implementations
{
    public class RakeReport : StakeReportBase
    {
        private decimal _totalRake;
        private decimal _heroRake;
        private decimal _relativeRake;
        
        public RakeReport(string hero, IReadOnlyCollection<HandHistory> hands) 
            : base(hands)
        {
            hands.ForEach(hands => ParseHand(hero, hands));
            _relativeRake =  _heroRake * _factor;
        }

        public override void AppendReport(StringBuilder builder)
            => builder
                .AppendLine("Rake")
                .AppendLine($" - total:                 {_totalRake:F2}€")
                .AppendLine($" - by hero:               {_heroRake:F2}€")
                .AppendLine($" - by hero in BB/100:     {_relativeRake:F2}");

        protected override void ParseHand(string hero, HandHistory hand)
        {
            _totalRake += hand.Rake;
            if (hand.Winner == hero)
            {
                _heroRake += hand.Rake;
            }
        }
    }
}
