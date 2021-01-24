﻿using RioParser.Domain.HandHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RioParser.Domain.Reports.CashGame
{
    public abstract class StakeReportBase
    {
        protected readonly int _hands;
        protected readonly decimal _bigBlind;
        private readonly GameType _gameType;
        protected readonly decimal _factor;
        protected readonly bool _includeHeroStatistics;

        public StakeReportBase(string hero, IReadOnlyCollection<HandHistory> hands)
        {
            var bigBlinds = hands.GroupBy(hand => hand.BigBlind);
            if (bigBlinds.Count() != 1)
            {
                throw new ArgumentException("A stake report can only contain hands of one stake!");
            }

            _includeHeroStatistics = !string.IsNullOrEmpty(hero);
            _hands = hands.Count;
            _bigBlind = bigBlinds.Single().Key;
            _gameType = hands.First().Game;

            _factor = 1 / (_bigBlind * _hands / 100);
        }

        protected abstract void ParseHand(string hero, HandHistory hand);
        public abstract void AppendReport(StringBuilder builder);

        public void AppendStakeReport(StringBuilder builder)
            => builder
                .AppendLine($"----- Big Blind: {_bigBlind:F2}€ - {_gameType} - Hands: {_hands} -----");
    }
}