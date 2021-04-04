using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.Sessions;

namespace RioParser.Domain.Reports.Cub3d
{
    public class Cub3dReporter : IReporter
    {
        private readonly ReportOptions _reportOptions;

        public Cub3dReporter(ReportOptions reportOptions)
        {
            _reportOptions = reportOptions;
        }

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<SessionBase> sessions)
        {
            if (string.IsNullOrEmpty(_reportOptions.Hero))
            {
                return new[] { new GenericReport().Add("The cub3d report only makes sense if you provide a hero. Please do.") };
            }

            var cub3dSessions = sessions
                .Cast<Cub3dSession>()
                .Where(session => session.Hands.First().Players.Contains(_reportOptions.Hero))
                .ToList();

            return cub3dSessions.Any()
                ? new[] { new Cub3dReport(_reportOptions.Hero, cub3dSessions) }
                : new[] { new GenericReport().Add($"No Cub3d tourneys found where {_reportOptions.Hero} played.") };
        }
    }
}
