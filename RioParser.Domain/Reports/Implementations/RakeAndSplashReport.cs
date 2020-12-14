using System.Collections.Generic;
using System.Linq;
using System.Text;
using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;

namespace RioParser.Domain.Reports.Implementations
{
    public class RakeAndSplashReport : IHandsReport
    {
        private decimal _totalRake;
        private decimal _heroRake;

        private decimal _totalSplash;
        private decimal _heroSplash;
        private int _handsWithSplash;
        private decimal _maxSplash;

        private readonly int _hands;
        private readonly decimal _bigBlind;
        private readonly decimal _relativeRake;
        private readonly decimal _relativeSplash;

        public RakeAndSplashReport(string hero, IReadOnlyCollection<HandHistory> hands)
        {
            _hands = hands.Count;
            _bigBlind = hands.First().BigBlind;

            hands.ForEach(hand => ParseHand(hero, hand));

            var factor = 1 / (_bigBlind * _hands / 100);
            _relativeRake = _heroRake * factor;
            _relativeSplash = _heroSplash * factor;
        }

        private void ParseHand(string hero, HandHistory hand)
        {
            _totalRake += hand.Rake;
            _totalSplash += hand.Splash;

            if (hand.Winner == hero)
            {
                _heroRake += hand.Rake;
                _heroSplash += hand.Splash;
            }

            if (hand.Splash != default)
            {
                _handsWithSplash++;
                _maxSplash = hand.Splash > _maxSplash ? hand.Splash : _maxSplash;
            }
        }

        public string PrintOut() 
            => new StringBuilder()
                .AppendLine()
                .AppendLine($"*** Big Blind: {_bigBlind:F2}€ ---  Hands: {_hands}")
                .AppendLine("Rake")
                .AppendLine($" - total:                 {_totalRake:F2}€")
                .AppendLine($" - by hero:               {_heroRake:F2}€")
                .AppendLine($" - by hero in BB/100:     {_relativeRake:F2}")
                .AppendLine("Splash")
                .AppendLine($" - total:                 {_totalSplash:F2}€")
                .AppendLine($" - won by hero:           {_heroSplash:F2}€")
                .AppendLine($" - won by hero in BB/100: {_relativeSplash:F2}")
                .AppendLine($" - splash frequency:      {(double)_handsWithSplash/_hands:P2}")
                .AppendLine($" - biggest splash:        {_maxSplash/_bigBlind:F2} BB")
                .ToString();
    }
}