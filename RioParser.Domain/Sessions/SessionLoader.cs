using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RioParser.Domain.Sessions
{
    public class SessionLoader
    {
        private readonly Regex _handHistoryRegex = new Regex("([0-9]*)_([0-9]*)");
        private readonly string _handHistoryExtension = ".txt";

        public IReadOnlyCollection<SessionBase> Load(string path, SessionType type)
            => new DirectoryInfo(path)
                .GetFiles()
                .Where(MatchesHandHistoryFileFormat)
                .Select(fileInfo =>
                    {
                        var content = GetFileContent(fileInfo);
                        return new PreSession(fileInfo.Name, content, SessionBase.SessionType(content));
                    })
                .Where(preSession => preSession.SessionType == type)
                .Select(CreateSession)
                .ToList();

        private bool MatchesHandHistoryFileFormat(FileInfo fileInfo)
        {
            var extensionFits = fileInfo.Extension == _handHistoryExtension;
            var nameSchemeFits = _handHistoryRegex
                .Match(fileInfo.Name)
                .Success;

            return nameSchemeFits && extensionFits;
        }

        private static SessionBase CreateSession(PreSession preSession)
            => preSession.SessionType switch
            {
                SessionType.Cash => new CashGameSession(preSession.Name, preSession.Content),
                SessionType.Cub3d => new Cub3dSession(preSession.Name, preSession.Content),
                SessionType.Sng => new SngSession(preSession.Name, preSession.Content),
                _ => throw new NotImplementedException()
            };

        private static string GetFileContent(FileInfo fileInfo)
        {
            using var reader = fileInfo.OpenText();
            return reader.ReadToEnd();
        }

        private record PreSession(string Name, string Content, SessionType SessionType);
    }
}
