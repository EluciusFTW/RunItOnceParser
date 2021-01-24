using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RioParser.Domain.Reports.CashGame
{
    public class SplashReport : StakeReportBase
    {
        private decimal _totalSplash;
        private decimal _heroSplash;
        private int _handsWithSplash;
        private readonly decimal _relativeHeroSplash;

        private readonly IList<string> _bigSplashes = new List<string>();
        private readonly IDictionary<decimal, int> _spashDistribution = new Dictionary<decimal, int>();

        public SplashReport(string hero, IReadOnlyCollection<HandHistory> hands)
            : base(hero, hands)
        {
            hands.ForEach(hands => ParseHand(hero, hands));
            _relativeHeroSplash = _heroSplash * _factor;
            _handsWithSplash = _spashDistribution.Sum(kvp => kvp.Value);
        }

        protected override void ParseHand(string hero, HandHistory hand)
        {
            if (hand.BigSplash)
            {
                _bigSplashes.Add(hand.Identifier);
                return;
            }

            if (hand.Splash == default)
            {
                return;
            }

            _totalSplash += hand.Splash;
            if (_includeHeroStatistics && hand.Winner == hero)
            {
                _heroSplash += hand.Splash;
            }

            var relative = hand.Splash / _bigBlind;
            if (_spashDistribution.ContainsKey(relative))
            {
                _spashDistribution[relative]++;
            }
            else
            {
                _spashDistribution.Add(relative, 1);
            }
        }

        public override void AppendReport(StringBuilder builder)
        {
            builder
                .AppendLine("Splash")
                .AppendLine($" - occurences:            {_handsWithSplash}")
                .AppendLine($" - splash frequency:      {(double)_handsWithSplash / _hands:P2}")
                .AppendLine($" - total amount:          {_totalSplash:F2}€");

            if (_includeHeroStatistics)
            {
                builder
                    .AppendLine($" - won by hero:           {_heroSplash:F2}€")
                    .AppendLine($" - won by hero in BB/100: {_relativeHeroSplash:F2}");
            }

            builder
                .AppendLine("Splash distribution");

            _spashDistribution
                .OrderBy(kvp => kvp.Key)
                .ForEach(kvp => builder.AppendLine($" - {kvp.Key,3}BB {kvp.Value,18} Splash{(kvp.Value > 1 ? @"es" : string.Empty)}"));

            if (_bigSplashes.Any())
            {
                builder.AppendLine($"Exceptional Splashes occured in hands: {string.Join(", ", _bigSplashes)}");
            }
        }
    }
}
