using RioParser.Domain.Artefact;
using RioParser.Domain.Extensions;
using RioParser.Domain.Hands;
using System.Collections.Generic;

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
            yield return new TableArtefact(new[] { "Rake", string.Empty }, Rows());
        }

        private IEnumerable<IReadOnlyCollection<string>> Rows()
        {
            yield return new[] { "- total", $"{_totalRake:F2}€" };
            if (_includeHeroStatistics)
            {
                yield return new[] { "- by hero", $"{_heroRake:F2}€" };
                yield return new[] { "- by hero in BB/100", $"{_relativeHeroRake:F2}" };
            }
        }
    }
}
