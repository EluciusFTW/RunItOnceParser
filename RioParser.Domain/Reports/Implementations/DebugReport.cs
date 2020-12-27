using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RioParser.Domain.Extensions;
using RioParser.Domain.HandHistories;

namespace RioParser.Domain.Reports.Implementations
{
    public class DebugReport : IHandsReport
    {
        private readonly IList<KeyValuePair<string, string>> _errors = new List<KeyValuePair<string, string>>();
        private readonly IDictionary<string, Func<HandHistory, string>> _properties = new Dictionary<string, Func<HandHistory, string>>
        {
            { "Game", x => x.Game.ToString() },
            { "Rake", x => x.Rake.ToString() },
            { "Total", x => x.Total.ToString() },
            { "Splash", x => x.Splash.ToString() },
            { "Winner", x => x.Winner.ToString() },
            { "BigBlind", x => x.BigBlind.ToString() }
        };

        public DebugReport(string hero, IReadOnlyCollection<HandHistory> hands)
        {
            hands.ForEach(TryParse);
            // DumpErrors();
        }

        private void DumpErrors()
        {
            var lines = new[] { "Extraction errors listed by hand history", Environment.NewLine }
                .Concat(_errors.Select(error => $"Hand Id: {error.Key}, Extraction property: {error.Value}"));

            File.WriteAllLines(@"D:\dump.txt", lines);
        }

        private void TryParse(HandHistory hand) => _properties.ForEach(prop => TryParseProp(hand, prop));

        private void TryParseProp(HandHistory hand, in KeyValuePair<string, Func<HandHistory, string>> prop)
        {
            try
            {
                _ = prop.Value(hand);
            }
            catch
            {
                var key = prop.Key != "Identifier"
                    ? hand.Identifier
                    : "IDENTIFIER FAILURE";
                _errors.Add(new KeyValuePair<string, string>(key, prop.Key));
            }
        }

        public string PrintOut()
        {
            var errorDetails = _errors
                .GroupBy(error => error.Value)
                .Select(grp => $"Accessing '{grp.Key}' found {grp.Count()} errors.");

            return "Debug report run found the following errors: " + Environment.NewLine + string.Join(Environment.NewLine, errorDetails);
        }
    }
}
