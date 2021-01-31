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

        public IEnumerable<string> PrintOut() => _sessions.Select(SessionReport);

        public string SessionReport(Cub3dSession session)
        {
            var builder = new StringBuilder();

            var lastHand = session.Hands.Last();
            var heroWon = lastHand.Winner == _hero;
            var heroPosition = !heroWon
                ? lastHand.Players.Count
                : 1;

            builder
                .AppendLine($"Hello from session {session.Name}!")
                .AppendLine($"   Players: {string.Join(", ", session.Hands.First().Players)}.")
                .AppendLine($"   Hands: {session.Hands.Count}.")
                .AppendLine($"   Hero Placement: {heroPosition}");

            return builder.ToString();
        }
    }
}
