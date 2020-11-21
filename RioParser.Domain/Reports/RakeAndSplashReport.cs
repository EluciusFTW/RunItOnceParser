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
        private decimal _rake;
        private decimal _splash;

        private decimal _relativeRake;
        private decimal _relativeSplash;

        public RakeAndSplashReport(string hero, IReadOnlyCollection<HandHistory> hands)
        {
            _hands = hands.Count;
            _bigBlind = hands.First().BigBlind;

            hands
                .Where(hand => hand.Winner == hero)
                .ForEach(hand =>
                {
                    _rake += hand.Rake;
                    _splash += hand.Splash;
                });

            var factor = 1 / (_bigBlind * _hands / 100);
            _relativeRake = _rake * factor;
            _relativeSplash = _splash * factor;
        }

        public string PrintOut()
        {
            return
                $"Big Blind: {_bigBlind:F2}€" + Environment.NewLine +
                $"Hands: {_hands}" + Environment.NewLine +
                $"Rake: {_rake:F2}€" + Environment.NewLine +
                $"Rake in BB/100: {_relativeRake:F2}€" + Environment.NewLine +
                $"STP: {_splash:F2}€" + Environment.NewLine +
                $"STP in BB/100: {_relativeSplash:F2}€" + Environment.NewLine;
        }
    }
}