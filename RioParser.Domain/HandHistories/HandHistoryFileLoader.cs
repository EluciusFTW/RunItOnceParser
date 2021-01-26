using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RioParser.Domain.HandHistories
{
    public class HandHistoryFileLoader
    {
        private readonly Regex _handHistoryRegex = new Regex("([0-9]*)_([0-9]*)");
        private readonly string _handHistoryExtension = ".txt";

        public IReadOnlyCollection<HandHistoryFile> Load(string path)
            => new DirectoryInfo(path)
                .GetFiles()
                .Where(MatchesHandHistoryFileFormat)
                .Select(fileInfo => new 
                    {
                        fileInfo.Name, 
                        Content = GetFileContent(fileInfo) 
                    })
                .Select(pair => new HandHistoryFile(pair.Name, pair.Content))
                .ToList();

        private bool MatchesHandHistoryFileFormat(FileInfo fileInfo)
        {
            var extensionFits = fileInfo.Extension == _handHistoryExtension;
            var nameSchemeFits = _handHistoryRegex
                .Match(fileInfo.Name)
                .Success;

            return nameSchemeFits && extensionFits;
        }

        private static string GetFileContent(FileInfo fileInfo)
        {
            using var reader = fileInfo.OpenText();
            return reader.ReadToEnd();
        }
    }
}
