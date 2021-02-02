using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.Extensions;

namespace RioParser.Domain.Hands
{
    public abstract class HandBase
    {
        private const string ShowDownMarker = "*** SHOWDOWN ***";
        private const string ActionMarker = "*** HOLE CARDS ***";
        private const string SummaryMarker = "*** SUMMARY ***";

        protected const string HeaderSeparator = "Table ID '";

        protected const string PLOIndicator = "Omaha Pot Limit";
        protected const string NLHIndicator = "Hold'em No Limit";
        protected const string CubedIndicator = "Cub3d";

        protected readonly string _header;
        protected readonly string _intro;
        protected readonly string _action;
        protected readonly string _showDown;
        protected readonly string _summary;

        public bool Cubed => _header.Contains(CubedIndicator);

        public string Identifier
            => _header
                .AfterFirst("#")
                .Before(":");
       
        public abstract decimal Total { get; }
        public abstract decimal BigBlind { get; }

        public IReadOnlyCollection<string> Players => _intro
            .SplitIntoLines()
            .Where(line => line.StartsWith("Seat"))
            .Select(line => line.BetweenSingle(": ", " ("))
            .ToList();
        
        public string Winner => new string(_summary
            .LineContaining(" won ")
            .AfterFirst(":")
            .BeforeAny(new[] { "(", "showed", "mucked" })
            .ToArray());

        public HandBase(string hand)
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