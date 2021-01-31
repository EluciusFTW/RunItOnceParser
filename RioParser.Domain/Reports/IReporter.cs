using RioParser.Domain.Sessions;
using System.Collections.Generic;

namespace RioParser.Domain.Reports
{
    public interface IReporter
    {
        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<SessionBase> sessions);
    }
}