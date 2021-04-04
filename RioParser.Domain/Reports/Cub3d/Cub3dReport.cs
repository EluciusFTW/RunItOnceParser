using System.Collections.Generic;
using System.Linq;
using System.Text;
using RioParser.Domain.Artefact;
using RioParser.Domain.Extensions;
using RioParser.Domain.Sessions;

namespace RioParser.Domain.Reports.Cub3d
{
    public class Cub3dReport : IReport
    {
        private readonly string _hero;
        private readonly IReadOnlyCollection<Cub3dSession> _sessions;

        public Cub3dReport(string hero, IReadOnlyCollection<Cub3dSession> cub3dSessions)
        {
            _hero = hero;
            _sessions = cub3dSessions;
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
                .AppendLine($"Cub3d Sngs:           {totalNumber,3} Sngs")
                .AppendLine($"Longest tourney:      {longest,3} Hands")
                .AppendLine($"Shortest won tourney: {shortest,3} Hands")
                .AppendLine()
                .AppendLine($"Total Buyins:         {_sessions.Sum(session => session.Hands.First().EntryFee),8:F2}€")
                .AppendLine($"Total Rake:           {_sessions.Sum(session => session.Hands.First().Rake),8:F2}€")
                .AppendLine()
                .AppendLine($"Buyin distribution: ");

            var groupedByEntryFee = _sessions
                .GroupBy(session => session.Hands.First().EntryFee)
                .Select(group => new { group.Key, Nr = group.Count() })
                .OrderByDescending(entryFeeGroup => entryFeeGroup.Key);

            var maxFeeLength = groupedByEntryFee.First().Key.ToString().Length;
            groupedByEntryFee
                .ForEach(entryFeeGroup 
                    => builder
                        .Append(new string(' ', maxFeeLength - entryFeeGroup.Key.ToString().Length))
                        .AppendLine($"{entryFeeGroup.Key:F2}€    - {entryFeeGroup.Nr,6} times"));

            builder
                .AppendLine()
                .AppendLine($"Position distribution: ");
            
            _sessions
                .Select(HeroPosition)
                .GroupBy(position => position)
                .OrderByDescending(group => -group.Key)
                .Select(group => new { Position = group.Key, Occurences = group.Count() })
                .ForEach(positionData 
                    => builder.AppendLine($"{positionData.Position}. Place   -{positionData.Occurences,6} times   -   {(double)positionData.Occurences / totalNumber:P2}%"));

            return builder.ToString();
        }

        public IEnumerable<IReportArtefact> Artefacts()
        {
            throw new System.NotImplementedException();
        }
    }
}
