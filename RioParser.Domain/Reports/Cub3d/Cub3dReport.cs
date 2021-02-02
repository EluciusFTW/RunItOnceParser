using RioParser.Domain.Extensions;
using RioParser.Domain.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RioParser.Domain.Reports.SitAndGo
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

        private int HeroPosition(Cub3dSession session)
        {
            var lastHand = session.Hands.Last();
            var heroWon = lastHand.Winner == _hero;

            return !heroWon
                ? lastHand.Players.Count
                : 1;
        }

        public string SessionReport(Cub3dSession session)
        {
            var builder = new StringBuilder();
            var heroPosition = HeroPosition(session);
           
            builder
                .AppendLine($"Hello from session {session.Name}!")
                .AppendLine($"   Players: {string.Join(", ", session.Hands.First().Players)}.")
                .AppendLine($"   Hands: {session.Hands.Count}.")
                .AppendLine($"   Hero Placement: {heroPosition}");

            return builder.ToString();
        }

        public string AggregateReport()
        {
            var builder = new StringBuilder();
            
            var totalNumber = _sessions.Count;
            var longest = _sessions.Max(s => s.Hands.Count);
            var shortest = _sessions.Min(s => 1000 * (HeroPosition(s) - 1) + s.Hands.Count);

            builder
                .AppendLine($"Cub3d Sngs: {totalNumber}")
                .AppendLine()
                .AppendLine($"Longest tourney:       {longest} Hands.")
                .AppendLine($"Shortest won tourney:   {shortest} Hands.")
                .AppendLine()
                .AppendLine($"Total Buyins:     {_sessions.Sum(session => session.Hands.First().EntryFee):F2}€.")
                .AppendLine($"Total Rake:       {_sessions.Sum(session => session.Hands.First().Rake):F2}€.")
                .AppendLine()
                .AppendLine($"Buyin distribution: ");

            _sessions
                .GroupBy(s => s.Hands.First().EntryFee)
                .Select(grp => new { grp.Key, Nr = grp.Count() })
                .OrderByDescending(si => si.Key)
                .ForEach(si => builder.AppendLine($"{si.Key:F2}€   -   {si.Nr} times"));

            builder
                .AppendLine()
                .AppendLine($"Position distribution: ");
            
            _sessions
                .Select(HeroPosition)
                .GroupBy(pos => pos)
                .OrderByDescending(grp => -grp.Key)
                .Select(grp => new { Position = grp.Key, Occurences = grp.Count() })
                .ForEach(positionData 
                    => builder.AppendLine($"{positionData.Position}. Place   -   {positionData.Occurences} times   -   {(double)positionData.Occurences / totalNumber:P2}%"));

            return builder.ToString();
        }
    }
}
