using FluentAssertions;
using Microsoft.Extensions.FileProviders;
using RioParser.Domain.HandHistories;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace RioParser.Domain.Tests
{
    public class HandHistoryTests
    {
        private readonly HandHistoryFile _handHistories;
        
        public HandHistoryTests()
        {
            _handHistories = GetHandHistoryFileContent();
        }

        [Fact]
        public void Extracts_Rake()
        {
            var expectedRakeSet = new[] { 0.00, 0.30, 0.00, 0.00, 0.00, 0.00, 0.05, 0.25, 0.76, 0.023 };
            
            _handHistories.Hands
                .Take(10)
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
                .Should().Be("44774946");
        }

        [Fact]
        public void Extracts_Total()
        {
            var expectedTotals = new[] { 0.30, 6.60, 0.50, 0.50, 0.30, 0.30, 1.20, 5.60, 17.70, 5.70 };

            _handHistories.Hands
                .Select(hand => hand.Total)
                .Should().BeEquivalentTo(expectedTotals);
        }

        [Fact]
        public void Extracts_Splash()
        {
            var expectedSplashes = new[] { 0.80, 0.60 };

            _handHistories.Hands
                .Skip(8)
                .Select(hand => hand.Splash)
                .Should().BeEquivalentTo(expectedSplashes);
        }

        [Fact]
        public void Extracts_Winner()
        {
            var expectedWinners = new[] { "MiamiBlues", "MiamiBlues", "Carson A", "Kassidy N", "Carson A", "MiamiBlues", "MiamiBlues", "Amy E", "Brad A", "Curtis B" };

            _handHistories.Hands
                .Select(hand => hand.Winner)
                .Should().BeEquivalentTo(expectedWinners);
        }

        public HandHistoryFile GetHandHistoryFileContent()
        {
            using var stream = new EmbeddedFileProvider(Assembly.GetExecutingAssembly())
                .GetFileInfo("_TestData/manipulated-hand-histories-plo-20.txt")
                .CreateReadStream();
            using var reader = new StreamReader(stream);
            
            return new HandHistoryFile("test", reader.ReadToEnd());
        }
    }
}
