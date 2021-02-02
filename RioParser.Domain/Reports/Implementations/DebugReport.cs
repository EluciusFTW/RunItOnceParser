﻿using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.Extensions;
using RioParser.Domain.Hands;
using RioParser.Domain.Reports.Models;

namespace RioParser.Domain.Reports.Implementations
{
    public class DebugReport : IReport
    {
        private readonly IList<KeyValuePair<string, string>> _errors = new List<KeyValuePair<string, string>>();
        private readonly IDictionary<string, Func<HandBase, string>> _properties = new Dictionary<string, Func<HandBase, string>>
        {
            // { "Game", x => x.Game.ToString() },
            // { "Rake", x => x.Rake.ToString() },
            { "Total", x => x.Total.ToString() },
            // { "Splash", x => x.Splash.ToString() },
            { "Winner", x => x.Winner.ToString() },
            { "BigBlind", x => x.BigBlind.ToString() }
        };

        public DebugReport(ReportOptions reportOptions, IReadOnlyCollection<HandBase> hands) => hands.ForEach(TryParse);

        private void TryParse(HandBase hand) => _properties.ForEach(prop => TryParseProp(hand, prop));

        private void TryParseProp(HandBase hand, in KeyValuePair<string, Func<HandBase, string>> prop)
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

        public IEnumerable<string> PrintOut()
        {
            var errorDetails = _errors
                .GroupBy(error => error.Value)
                .Select(grp => ErrorDetailsLine(grp.Key, grp.Select(kvp => kvp.Key).ToList()));

            return new[] { "Debug report run found the following errors: " + Environment.NewLine + string.Join(Environment.NewLine, errorDetails) };
        }

        private string ErrorDetailsLine(string prop, IReadOnlyCollection<string> identigfiers)
            => $"Accessing property '{prop}' resulted in {identigfiers.Count()} errors, in the following hands: "
                + Environment.NewLine + string.Join(", ", identigfiers) + Environment.NewLine;
    }
}