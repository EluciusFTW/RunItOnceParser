using RioParser.Domain.Sessions;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Reports.Sng
{
    public class SngReport : IReport
    {
        private readonly string _hero;
        private readonly IReadOnlyCollection<SngSession> _sessions;

        public SngReport(string hero, IReadOnlyCollection<SngSession> sngSessions)
        {
            _hero = hero;
            _sessions = sngSessions;
        }

        public IEnumerable<string> PrintOut()
        {
            return _sessions.Select(s => $"Hello from {s.Identifier}");
        }
    }
}
