using FluentAssertions;
using System.Linq;
using Xunit;

namespace RioParser.Domain.Tests.HandHistoriesTests
{
    public class NlhHandTests : CashGameHandsTestsBase
    {
        public NlhHandTests()
            : base("_TestData/manipulated-hand-histories-nlh-20.txt")
        {
        }

        [Fact]
        public void Extracts_GameType_Nlh()
        {
            _hands.Hands
                .All(hand => hand.Game == GameType.NLH)
                .Should().BeTrue();
        }

        [Fact]
        public void Extracts_Rake()
        {
            var expectedRakeSet = new[] { 0.00, 0.00, 0.00, 0.00 };

            _hands.Hands
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
                .Should().Be("46744454");
        }

        [Fact]
        public void Extracts_Total()
        {
            var expectedTotals = new[] { 0.50, 0.50, 0.50, 0.50 };

            _hands.Hands
                .Select(hand => hand.Total)
                .Should().BeEquivalentTo(expectedTotals);
        }

        [Fact]
        public void Extracts_Winner()
        {
            var expectedWinners = new[] { "Clark H", "Maddison Z", "Maddison Z", "MiamiBlues" };

            _hands.Hands
                .Select(hand => hand.Winner)
                .Should().BeEquivalentTo(expectedWinners);
        }
    }
}
