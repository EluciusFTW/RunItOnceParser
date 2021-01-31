using Microsoft.Extensions.FileProviders;
using RioParser.Domain.Sessions;
using System.IO;
using System.Reflection;

namespace RioParser.Domain.Tests
{
    public abstract class CashGameHandsTestsBase
    {
        protected readonly CashGameSession _hands;

        public CashGameHandsTestsBase(string testFileName)
        {
            _hands = GetHandHistoryFileContent(testFileName);
        }

        public CashGameSession GetHandHistoryFileContent(string testFileName)
        {
            using var stream = new EmbeddedFileProvider(Assembly.GetExecutingAssembly())
                .GetFileInfo(testFileName)
                .CreateReadStream();
            using var reader = new StreamReader(stream);

            return new CashGameSession("test", reader.ReadToEnd());
        }
    }
}
