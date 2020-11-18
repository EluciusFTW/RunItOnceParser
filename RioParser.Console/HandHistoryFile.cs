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
    }
}