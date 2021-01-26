using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RioParser.Domain.HandHistories
{
    public class HandHistoryFile
    {
        private const string Separator = "**** **** **** **** **** **** **** ****";
        public string Name { get; }
        public IReadOnlyCollection<HandHistory> Hands { get; }

        public HandHistoryFile(string name, string content)
        {
            Name = name;
            Hands = content
                .Split(Separator)
                .Select(hand => new HandHistory(hand))
                .ToList();
        }
    }
}