﻿using FluentAssertions;
using System.Linq;
using Xunit;

namespace RioParser.Domain.Tests.HandHistoriesTests
{
    public class PloHandTests: CashGameHandsTestsBase
    {
        public PloHandTests()
            : base ("_TestData/manipulated-hand-histories-plo-20.txt")
        {
        }

        [Fact]
        public void Extracts_GameType_Plo()
        {
            var expectedRakeSet = new[] { 0.00, 0.30, 0.00, 0.00, 0.00, 0.00, 0.05, 0.25, 0.76, 0.023 };

            _hands.Hands
                .All(hand => hand.Game == GameType.PLO)
                .Should().BeTrue();
        }

        [Fact]
        public void Extracts_Rake()
        {
            var expectedRakeSet = new[] { 0.00, 0.30, 0.00, 0.00, 0.00, 0.00, 0.05, 0.25, 0.76, 0.23 };

            _hands.Hands
                .Take(10)
                .Select(hand => hand.Rake)
                .Should().BeEquivalentTo(expectedRakeSet);
        }

        [Fact]
        public void Extracts_BigBlind()
        {
            _hands.Hands.First().BigBlind
                .Should().Be((decimal)0.20);
        }

        [Fact]
        public void Extracts_Identifier()
        {
            _hands.Hands.First().Identifier
                .Should().Be("44774946");
        }

        [Fact]
        public void Extracts_Total()
        {
            var expectedTotals = new[] { 0.30, 6.60, 0.50, 0.50, 0.30, 0.30, 1.20, 5.60, 17.70, 5.70 };

            _hands.Hands
                .Select(hand => hand.Total)
                .Should().BeEquivalentTo(expectedTotals);
        }

        [Fact]
        public void Extracts_Splash()
        {
            var expectedSplashes = new[] { 0.80, 0.60 };

            _hands.Hands
                .Skip(8)
                .Select(hand => hand.Splash)
                .Should().BeEquivalentTo(expectedSplashes);
        }

        [Fact]
        public void Extracts_Winner()
        {
            var expectedWinners = new[] { "MiamiBlues", "MiamiBlues", "Carson A", "Kassidy N", "Carson A", "MiamiBlues", "MiamiBlues", "Amy E", "Brad A", "Curtis B" };

            _hands.Hands
                .Select(hand => hand.Winner)
                .Should().BeEquivalentTo(expectedWinners);
        }
    }
}
