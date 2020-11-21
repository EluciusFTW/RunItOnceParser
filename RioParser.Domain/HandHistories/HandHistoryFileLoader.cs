using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RioParser.Domain.HandHistories
{
    public class HandHistoryFileLoader
    {
        private Regex _handHistoryRegex = new Regex("([0-9]*)_([0-9]*)");

        public IReadOnlyCollection<HandHistoryFile> Load(string path)
            => new DirectoryInfo(path)
                .GetFiles()
                .Where(MatchesHandHistoryFileFormat)
                .Select(fileInfo => new HandHistoryFile(fileInfo))
                .ToList();

        private bool MatchesHandHistoryFileFormat(FileInfo fileInfo)
        {
            var extensionFits = fileInfo.Extension == ".txt";
            var nameSchemeFits = _handHistoryRegex
                .Match(fileInfo.Name)
                .Success;

            return nameSchemeFits && extensionFits;
        }
    }
}
