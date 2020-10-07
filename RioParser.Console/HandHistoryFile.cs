using System;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Console
{
    internal class HandHistoryFile
    {
        private string content;
        private const string separator = "**** **** **** **** **** **** **** ****";

        private IReadOnlyCollection<HandHistory> hands;

        public HandHistoryFile(string content)
        {
            this.content = content;
            hands = content
                .Split(separator)
                .Select(hand => new HandHistory(hand))
                .ToList();
        }

        public string PrintOut()
        {
            decimal rake = 0;
            decimal stp = 0;
            foreach (var hand in hands.Where(hand => hand.Winner == "MiamiBlues"))
            {
                rake += hand.Rake;
                stp += hand.Splash;
            }

            return
                $"Hands: {hands.Count}" + Environment.NewLine +
                $"Rake: {rake}" + Environment.NewLine +
                $"STP: {stp}";
        }
    }
}