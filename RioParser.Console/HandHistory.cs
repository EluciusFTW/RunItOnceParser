using RioParser.Console.Extensions;
using System;
using System.Linq;

namespace RioParser.Console
{
    internal class HandHistory
    {
        private const string ShowDownMarker = "*** SHOWDOWN ***";
        private const string ActionMarker = "*** HOLE CARDS ***";
        private const string SummaryMarker = "*** SUMMARY ***";

        private string _intro;
        private string _action;
        private string _showDown;
        private string _summary;

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

        public string Winner => new String(_summary
            .LineContaining(" won €")
            .Skip(8)
            .TakeWhile(c => c != ' ').ToArray());

        public HandHistory(string hand)
        {
            _intro = hand.Before(ActionMarker);
            _action = hand.BetweenSingle(ActionMarker, ShowDownMarker);
            _showDown = hand.BetweenSingle(ShowDownMarker, SummaryMarker);
            _summary = hand.AfterSingle(SummaryMarker);
        }
    }
}