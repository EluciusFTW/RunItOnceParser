using RioParser.Domain.Extensions;
using RioParser.Domain.Hands;
using RioParser.Domain.Reports.Artefact;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RioParser.Domain.Reports.CashGame
{
    public class SplashReport : StakeReportBase
    {
        private decimal _totalSplash;
        private decimal _heroSplash;
        
        private readonly int _handsWithSplash;
        private readonly decimal _relativeHeroSplash;

        private readonly IList<string> _bigSplashes = new List<string>();
        private readonly IDictionary<decimal, int> _splashDistribution = new Dictionary<decimal, int>();

        public SplashReport(string hero, IReadOnlyCollection<CashGameHand> hands)
            : base(hero, hands)
        {
            hands.ForEach(hand => ParseHand(hero, hand));
            _relativeHeroSplash = _heroSplash * _factor;
            _handsWithSplash = _splashDistribution.Sum(kvp => kvp.Value);
        }

        protected override void ParseHand(string hero, CashGameHand hand)
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
            if (_splashDistribution.ContainsKey(relative))
            {
                _splashDistribution[relative]++;
            }
            else
            {
                _splashDistribution.Add(relative, 1);
            }
        }

        //public override void AppendReport(StringBuilder builder)
        //{
        //    builder
        //        .AppendLine("Splash")
        //        .AppendLine($" - occurrences:            {_handsWithSplash}")
        //        .AppendLine($" - splash frequency:      {(double)_handsWithSplash / _hands:P2}")
        //        .AppendLine($" - total amount:          {_totalSplash:F2}€");

        //    if (_includeHeroStatistics)
        //    {
        //        builder
        //            .AppendLine($" - won by hero:           {_heroSplash:F2}€")
        //            .AppendLine($" - won by hero in BB/100: {_relativeHeroSplash:F2}");
        //    }

        //    builder
        //        .AppendLine("Splash distribution");

        //    _splashDistribution
        //        .OrderBy(kvp => kvp.Key)
        //        .ForEach(kvp => builder.AppendLine($" - {kvp.Key,3}BB {kvp.Value,18} Splash{(kvp.Value > 1 ? @"es" : string.Empty)}"));

        //    if (_bigSplashes.Any())
        //    {
        //        builder.AppendLine($"Exceptional Splashes occurred in hands: {string.Join(", ", _bigSplashes)}");
        //    }
        //}

        public override IEnumerable<IReportArtefact> Artifacts()
        {
            yield return new Table(new[] { "Splash", string.Empty }, Rows());

            var splashRows = _splashDistribution
                .OrderBy(kvp => kvp.Key)
                .ToDictionary(kvp => $" - {kvp.Key}BB", kvp => kvp.Value);

            yield return new ValueList("Splash distibution", splashRows);
        }

        private IEnumerable<IReadOnlyCollection<string>> Rows()
        {
            yield return new[] { "- occurences", $"{_handsWithSplash}" };
            yield return new[] { "- splash frequency", $"{(double)_handsWithSplash / _hands:P2}" };
            yield return new[] { "- totam amount", $"{_totalSplash:F2} EUR" };
            
            if (_includeHeroStatistics)
            {
                yield return new[] { "- won by hero", $"{_heroSplash:F2} EUR" };
                yield return new[] { "- won by hero in BB/100", $"{_relativeHeroSplash:F2}" };
            }
        }
    }
}
