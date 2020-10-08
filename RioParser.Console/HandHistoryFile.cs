using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RioParser.Console
{
    internal class HandHistoryFile
    {
        private const string separator = "**** **** **** **** **** **** **** ****";
        public string Name { get; }

        public IReadOnlyCollection<HandHistory> Hands { get; }

        public HandHistoryFile(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            Hands = GetFileContent(fileInfo)
                .Split(separator)
                .Select(hand => new HandHistory(hand))
                .ToList();
        }

        private string GetFileContent(FileInfo fileInfo)
        {
            using StreamReader reader = fileInfo.OpenText();
            return reader.ReadToEnd();
        }

        public string PrintOut()
        {
            decimal rake = 0;
            decimal stp = 0;
            foreach (var hand in Hands.Where(hand => hand.Winner == "MiamiBlues"))
            {
                rake += hand.Rake;
                stp += hand.Splash;
            }
            var bigBlind = Hands.First().BigBlind;
            var hands = Hands.Count;
            var factor = 1 / bigBlind * (decimal)hands / 100;
            return
                $"Hands: {hands}" + Environment.NewLine +
                $"Big Blind: {bigBlind}" + Environment.NewLine +
                $"Rake: {rake}" + Environment.NewLine +
                $"Rake in BB / 100: {rake * factor}" + Environment.NewLine +
                $"STP: {stp }" + Environment.NewLine +
                $"STP / BB: {stp * factor}" + Environment.NewLine;
        }
    }
}