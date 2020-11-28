using System.Linq;
using RioParser.Domain.Extensions;

namespace RioParser.Domain.HandHistories
{
    public class HandHistory
    {
        private const string ShowDownMarker = "*** SHOWDOWN ***";
        private const string ActionMarker = "*** HOLE CARDS ***";
        private const string SummaryMarker = "*** SUMMARY ***";

        private string _intro;
        private string _action;
        private string _showDown;
        private string _summary;

        private string _PloIndicator = "Omaha Pot Limit";

        public string Identifier
            => _intro
                .AfterFirst("#")
                .Before(":");

        public GameType Game
            => _intro.Contains(_PloIndicator)
                ? GameType.PLO
                : GameType.Unknown;

        public decimal Rake => _summary
            .AfterSingle("Rake €")
            .TakeAfter('.', 2)
            .ToDecimal();

        public decimal Total => _summary
            .AfterSingle("Total pot €")
            .TakeAfter('.', 2)
            .ToDecimal();

        public decimal Splash => _action
            .AfterSingle("STP added: €")
            .TakeAfter('.', 2)
            .ToDecimal();

        public decimal BigBlind => _intro
            .AfterSingle("/€")
            .TakeAfter('.', 2)
            .ToDecimal();

        public string Winner => new string(_summary
            .LineContaining(" won €")
            .AfterFirst(":")
            .BeforeAny(new[] { "(", "showed", "mucked" })
            .ToArray());

        public HandHistory(string hand)
        {
            _intro = hand.Before(ActionMarker);
            _action = hand.BetweenSingle(ActionMarker, ShowDownMarker);
            _showDown = hand.BetweenSingle(ShowDownMarker, SummaryMarker);
            _summary = hand.AfterSingle(SummaryMarker);
        }
    }
}