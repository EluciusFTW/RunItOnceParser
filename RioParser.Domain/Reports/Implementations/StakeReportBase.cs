using RioParser.Domain.HandHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RioParser.Domain.Reports.Implementations
{
    public abstract class StakeReportBase
    {
        protected readonly int _hands;
        protected readonly decimal _bigBlind;
        protected readonly decimal _factor;

        public StakeReportBase(IReadOnlyCollection<HandHistory> hands)
        {
            var bigBlinds = hands.GroupBy(hand => hand.BigBlind);
            if (bigBlinds.Count() != 1)
            {
                throw new ArgumentException("A stake report can only contain hands of one stake!");
            }
            
            _hands = hands.Count;
            _bigBlind = bigBlinds.Single().Key;

            _factor = 1 / (_bigBlind * _hands / 100);
        }

        protected abstract void ParseHand(string hero, HandHistory hand);
        public abstract void AppendReport(StringBuilder builder);

        public void AppendStakeReport(StringBuilder builder)
            => builder
                .AppendLine($"*** Big Blind: {_bigBlind:F2}€ ---  Hands: {_hands}");
    }
}