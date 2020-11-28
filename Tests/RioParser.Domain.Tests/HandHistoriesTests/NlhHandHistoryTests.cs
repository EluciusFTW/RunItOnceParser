using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RioParser.Domain.Tests.HandHistoriesTests
{
    public class NlhHandHistoryTests : HandHistoryTests
    {
        public NlhHandHistoryTests()
            : base("_TestData/manipulated-hand-histories-nlh-20.txt")
        {
        }

        [Fact]
        public void Extracts_GameType_Nlh()
        {
            _handHistories.Hands
                .All(hand => hand.Game == GameType.NLH)
                .Should().BeTrue();
        }

        [Fact]
        public void Extracts_Rake()
        {
            var expectedRakeSet = new[] { 0.00, 0.00, 0.00, 0.00 };

            _handHistories.Hands
                .Select(hand => hand.Rake)
                .Should().BeEquivalentTo(expectedRakeSet);
        }

        [Fact]
        public void Extracts_BigBlind()
        {
            _handHistories.Hands.First().BigBlind
                .Should().Be((decimal)0.20);
        }

        [Fact]
        public void Extracts_Identifier()
        {
            _handHistories.Hands.First().Identifier
                .Should().Be("46744454");
        }

        [Fact]
        public void Extracts_Total()
        {
            var expectedTotals = new[] { 0.50, 0.50, 0.50, 0.50 };

            _handHistories.Hands
                .Select(hand => hand.Total)
                .Should().BeEquivalentTo(expectedTotals);
        }

        [Fact]
        public void Extracts_Winner()
        {
            var expectedWinners = new[] { "Clark H", "Maddison Z", "Maddison Z", "MiamiBlues" };

            _handHistories.Hands
                .Select(hand => hand.Winner)
                .Should().BeEquivalentTo(expectedWinners);
        }
    }
}
