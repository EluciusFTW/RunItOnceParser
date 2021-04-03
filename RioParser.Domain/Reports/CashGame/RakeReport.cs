using RioParser.Domain.Extensions;
using RioParser.Domain.Hands;
using RioParser.Domain.Reports.Artefact;
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



        //public override void AppendReport(StringBuilder builder)
        //{
        //    builder
        //        .AppendLine("Rake")
        //        .AppendLine($" - total:                 {_totalRake:F2}€");

        //    if (_includeHeroStatistics)
        //    {
        //        builder
        //            .AppendLine($" - by hero:               {_heroRake:F2}€")
        //            .AppendLine($" - by hero in BB/100:     {_relativeHeroRake:F2}");
        //    }
        //}

        protected override void ParseHand(string hero, CashGameHand hand)
        {
            _totalRake += hand.Rake;
            if (_includeHeroStatistics && !hand.BigSplash && hand.Winner == hero)
            {
                _heroRake += hand.Rake;
            }
        }

        public override IEnumerable<IReportArtefact> Artifacts()
        {
            yield return new Table(new[] { "Rake", string.Empty }, Rows());
        }

        private IEnumerable<IReadOnlyCollection<string>> Rows()
        {
            yield return new[] { "- total", $"{_totalRake:F2} EUR" };
            if (_includeHeroStatistics)
            {
                yield return new[] { "- by hero", $"{_heroRake:F2} EUR" };
                yield return new[] { "- by hero in BB/100", $"{_relativeHeroRake:F2}" };
            }
        }
    }
}
