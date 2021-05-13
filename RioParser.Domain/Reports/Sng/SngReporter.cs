using RioParser.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Reports.Sng
{
    public class SngReporter : IReporter
    {
        private readonly ReportOptions _reportOptions;

        public SngReporter(ReportOptions reportOptions)
        {
            _reportOptions = reportOptions;
        }

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<SessionBase> sessions)
        {
            if (string.IsNullOrEmpty(_reportOptions.Hero))
            {
                return new[] { new GenericReport().Add("The sng report only makes sense if you provide a hero. Please do.") };
            }

            var sngSessions = sessions
                .Cast<SngSession>()
                .Where(session => session.Hands.First().Players.Contains(_reportOptions.Hero))
                .ToList();

            return sngSessions.Any()
                ? new[] { new SngReport(_reportOptions.Hero, sngSessions) }
                : new[] { new GenericReport().Add($"No Sng tourneys found where {_reportOptions.Hero} played.") };
        }
    }
}
