using System.Linq;
using RioParser.Domain.Extensions;

namespace RioParser.Domain.HandHistories
{
    public class HandHistory
    {
        private const string ShowDownMarker = "*** SHOWDOWN ***";
        private const string ActionMarker = "*** HOLE CARDS ***";
        private const string SummaryMarker = "*** SUMMARY ***";

        private const string HeaderSeparator = "Table ID '";

        private const string PLOIndicator = "Omaha Pot Limit";
        private const string NLHIndicator = "Hold'em No Limit";
        private const string CubedIndicator = "Cub3d";

        private readonly string _header;
        private readonly string _intro;
        private readonly string _action;
        private readonly string _showDown;
        private readonly string _summary;

        public bool Cubed => _header.Contains(CubedIndicator);

        public string Identifier
            => _header
                .AfterFirst("#")
                .Before(":");

        public GameType Game
            => _header.Contains(PLOIndicator)
                ? GameType.PLO
                : _header.Contains(NLHIndicator)
                    ? GameType.NLH
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
            .AfterSingleOrDefault("STP added: €")?
            .TakeAfter('.', 2)
            .ToDecimal() 
            ?? 0;

        public decimal BigBlind => _intro
            .AfterSingle("/€")
            .TakeAfter('.', 2)
            .ToDecimal();

        public string Winner => new string(_summary
            .LineContaining(" won €")
            .AfterFirst(":")
            .BeforeAny(new[] { "(", "showed", "mucked" })
            .ToArray());

        public bool BigSplash => _showDown == null;

        public HandHistory(string hand)
        {
            _intro = hand.Before(ActionMarker);
            _header = _intro.Before(HeaderSeparator);
            _action = hand
                .AfterSingle(ActionMarker)
                .BeforeAny(new[] { ShowDownMarker, SummaryMarker});
            _showDown = hand
                .AfterSingleOrDefault(ShowDownMarker)?
                .Before(SummaryMarker);
            _summary = hand.AfterSingle(SummaryMarker);
        }
    }
}