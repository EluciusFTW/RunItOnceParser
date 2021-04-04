using System;

namespace RioParser.Domain.Artefact
{
    public class SimpleArtefact : IReportArtefact
    {
        public string Value { get; }

        public SimpleArtefact(string value)
        {
            Value = value ?? throw new ArgumentNullException();
        }
    }
}
