using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Reports
{
    public class RakeAndSplashReport : IHandsReport
    {
        private int _hands;
        private decimal _bigBlind;

        private decimal _totalRake;
        private decimal _heroRake;

        private decimal _totalSplash;
        private decimal _heroSplash;

        private decimal _relativeRake;
        private decimal _relativeSplash;

        public RakeAndSplashReport(string hero, IReadOnlyCollection<HandHistory> hands)
        {
            _hands = hands.Count;
            _bigBlind = hands.First().BigBlind;

            hands
                .ForEach(hand =>
                {
                    _totalRake += hand.Rake;
                    _totalSplash += hand.Splash;

                    if(hand.Winner == hero)
                    {
                        _heroRake += hand.Rake;
                        _heroSplash += hand.Splash;
                    }
                });

            var factor = 1 / (_bigBlind * _hands / 100);
            _relativeRake = _heroRake * factor;
            _relativeSplash = _heroSplash * factor;
        }

        public string PrintOut()
        {
            return
                Environment.NewLine + 
                $"*** Big Blind: {_bigBlind:F2}€ ---  Hands: {_hands}" + Environment.NewLine +
                $"Rake" + Environment.NewLine +
                $" - total:                 {_totalRake:F2}€" + Environment.NewLine +
                $" - by hero:               {_heroRake:F2}€" + Environment.NewLine +
                $" - by hero in BB/100:     {_relativeRake:F2}" + Environment.NewLine +
                $"Splashes:" + Environment.NewLine +
                $" - total:                 {_totalSplash:F2}€" + Environment.NewLine +
                $" - won by hero:           {_heroSplash:F2}€" + Environment.NewLine +
                $" - won by hero in BB/100: {_relativeSplash:F2}" + Environment.NewLine;
        }
    }
}