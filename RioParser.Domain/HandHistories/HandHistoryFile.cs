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

        public HandHistoryFile(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            Hands = GetFileContent(fileInfo)
                .Split(Separator)
                .Select(hand => new HandHistory(hand))
                .ToList();
        }

        private static string GetFileContent(FileInfo fileInfo)
        {
            using var reader = fileInfo.OpenText();
            return reader.ReadToEnd();
        }
    }
}