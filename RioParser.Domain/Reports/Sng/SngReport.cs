using RioParser.Domain.Extensions;
using RioParser.Domain.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public IEnumerable<string> PrintOut() { yield return AggregateReport(); }

        private int HeroPosition(TourneySession session)
        {
            var lastHand = session.Hands.Last();
            var heroWon = lastHand.Winner == _hero;

            return !heroWon
                ? lastHand.Players.Count
                : 1;
        }

        public string AggregateReport()
        {
            var builder = new StringBuilder().AppendLine();

            var totalNumber = _sessions.Count;
            var longest = _sessions.Max(s => s.Hands.Count);
            var shortest = _sessions.Min(s => 1000 * (HeroPosition(s) - 1) + s.Hands.Count);

            builder
                .AppendLine($"Classic Sngs:         {totalNumber,3} Sngs")
                .AppendLine($"Longest tourney:      {longest,3} Hands")
                .AppendLine($"Shortest won tourney: {shortest,3} Hands")
                .AppendLine()
                .AppendLine($"Total Buy-ins:         {_sessions.Sum(session => session.Hands.First().EntryFee),8:F2}€")
                .AppendLine($"Total Rake:           {_sessions.Sum(session => session.Hands.First().Rake),8:F2}€")
                .AppendLine()
                .AppendLine("Buy-in distribution: ");

            var groupedByEntryFee = _sessions
                .GroupBy(session => session.Hands.First().EntryFee)
                .Select(group => new { group.Key, Nr = group.Count() })
                .OrderByDescending(entryFeeGroup => entryFeeGroup.Key)
                .ToList();

            var maxFeeLength = groupedByEntryFee.First().Key.ToString().Length;
            groupedByEntryFee
                .ForEach(entryFeeGroup
                    => builder
                        .Append(new string(' ', maxFeeLength - entryFeeGroup.Key.ToString().Length))
                        .AppendLine($"{entryFeeGroup.Key:F2}€    - {entryFeeGroup.Nr,6} times"));

            builder
                .AppendLine()
                .AppendLine("Position distribution: ");

            _sessions
                .Select(HeroPosition)
                .GroupBy(position => position)
                .OrderByDescending(group => -group.Key)
                .Select(group => new { Position = group.Key, Occurences = group.Count() })
                .ForEach(positionData
                    => builder.AppendLine($"{positionData.Position}. Place   -{positionData.Occurences,6} times   -   {(double)positionData.Occurences / totalNumber:P2}%"));

            return builder.ToString();
        }

        public IEnumerable<Artefact.IReportArtefact> Artefacts()
        {
            throw new System.NotImplementedException();
        }
    }
}
