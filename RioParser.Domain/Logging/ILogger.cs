namespace RioParser.Domain.Logging
{
    public interface ILogger
    {
        void Log(string message);
        void Chapter(string line);
        void Paragraph(string line);
    }
}
