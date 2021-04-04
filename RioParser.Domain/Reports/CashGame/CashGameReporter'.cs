using System;
using System.Collections.Generic;
using System.Linq;
using RioParser.Domain.Logging;
using RioParser.Domain.Extensions;
using RioParser.Domain.Sessions;
using RioParser.Domain.Artefact;

namespace RioParser.Domain.Reports.CashGame
{
    internal class CashGameReporter<TReport> : IReporter where TReport : IReport
    {
        private readonly ReportOptions _reportOptions;

        public CashGameReporter(ReportOptions reportOptions)
        {
            _reportOptions = reportOptions;
        }

        public IReadOnlyCollection<IReport> Process(IReadOnlyCollection<SessionBase> sessions)
        {
            var processingReport = new GenericReport();
            var filteredSessions = Filter(sessions, processingReport);
            if (!filteredSessions.Any())
            {
                return new[] 
                { 
                    processingReport
                        .AddArtefact(new SimpleArtefact($"No files left to process.", ArtefactLevel.Warning)) 
                };
            }

            var hands = filteredSessions
                .SelectMany(file =>
                {
                    processingReport.Add($"Processing {file.Name} containing {file.Hands.Count} hands.");
                    return file.Hands;
                })
                .ToList();

            return new IReport[]
                {
                    processingReport,
                    (TReport)Activator.CreateInstance(typeof(TReport), _reportOptions, hands)
                };
        }

        private IReadOnlyCollection<CashGameSession> Filter(IEnumerable<SessionBase> sessions, GenericReport processingReport)
        {
            var (ofCorrectGameType, ofWrongGameType) = sessions
                .Cast<CashGameSession>()
                .ToList()
                .Split(file => file.Hands.First().Game == _reportOptions.GameType);

            if (ofWrongGameType.Any())
            {
                processingReport.AddArtefact(
                    new SimpleArtefact($"Ignoring {ofWrongGameType.Count} files because of different game type.", ArtefactLevel.Warning));
            }

            return ofCorrectGameType.ToList();
        }
    }
}