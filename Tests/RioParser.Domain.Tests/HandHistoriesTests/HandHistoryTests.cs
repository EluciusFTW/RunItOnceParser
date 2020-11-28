using Microsoft.Extensions.FileProviders;
using RioParser.Domain.HandHistories;
using System.IO;
using System.Reflection;

namespace RioParser.Domain.Tests
{
    public abstract class HandHistoryTests
    {
        protected readonly HandHistoryFile _handHistories;
        
        public HandHistoryTests(string testFileName)
        {
            _handHistories = GetHandHistoryFileContent(testFileName);
        }

        public HandHistoryFile GetHandHistoryFileContent(string testFileName)
        {
            using var stream = new EmbeddedFileProvider(Assembly.GetExecutingAssembly())
                .GetFileInfo(testFileName)
                .CreateReadStream();
            using var reader = new StreamReader(stream);
            
            return new HandHistoryFile("test", reader.ReadToEnd());
        }
    }
}
