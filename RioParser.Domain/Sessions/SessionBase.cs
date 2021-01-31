using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Sessions
{
    public abstract class SessionBase
    {
        public string Name { get; }

        private const string Separator = "**** **** **** **** **** **** **** ****";
        private readonly string _content;

        internal static SessionType SessionType(string content)
            => content.Substring(0, 89) switch
            {
                string s when s.Contains("Cub3d") => Sessions.SessionType.Cub3d,
                string s when s.Contains("SnG") => Sessions.SessionType.Sng,
                _ => Sessions.SessionType.Cash
            };

        protected IReadOnlyCollection<string> Chunks
            => _content
                .Split(Separator)
                .ToList();

        public SessionBase(string name, string content)
        {
            Name = name;
            _content = content;
        }
    }
}
