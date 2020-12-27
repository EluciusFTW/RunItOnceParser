using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using System.Collections.Generic;
using System.Text;

namespace RioParser.Domain.Reports.Implementations
{
    public class SplashReport : StakeReportBase
    {
        private decimal _totalSplash;
        private decimal _heroSplash;
        private int _handsWithSplash;
        private decimal _maxSplash;
        private readonly decimal _relativeSplash;
        
        public SplashReport(string hero, IReadOnlyCollection<HandHistory> hands) 
            : base(hands)
        {
            hands.ForEach(hands => ParseHand(hero, hands));
            _relativeSplash = _heroSplash * _factor;
        }

        protected override void ParseHand(string hero, HandHistory hand)
        {
            _totalSplash += hand.Splash;
            if (hand.Winner == hero)
            {
                _heroSplash += hand.Splash;
            }

            if (hand.Splash != default)
            {
                _handsWithSplash++;
                _maxSplash = hand.Splash > _maxSplash 
                    ? hand.Splash 
                    : _maxSplash;
            }
        }

        public override void AppendReport(StringBuilder builder)
            => builder
                .AppendLine("Splash")
                .AppendLine($" - total:                 {_totalSplash:F2}€")
                .AppendLine($" - won by hero:           {_heroSplash:F2}€")
                .AppendLine($" - won by hero in BB/100: {_relativeSplash:F2}")
                .AppendLine($" - splash frequency:      {(double)_handsWithSplash / _hands:P2}")
                .AppendLine($" - biggest splash:        {_maxSplash / _bigBlind:F2} BB");
    }
}
