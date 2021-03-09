using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Sessions
{
    public abstract class SessionBase
    {
        public string Name { get; }

        private const string Separator = "**** **** **** **** **** **** **** ****";
        
        private readonly string _content;

        protected string Headline => _content.Substring(0, 89);

        internal static SessionType SessionType(string content)
            => content.Substring(0, 89) switch
            {
                string s when s.Contains("Tournament") && s.Contains("Cub3d") => Sessions.SessionType.Cub3d,
                string s when s.Contains("Tournament") && s.Contains("Classic") => Sessions.SessionType.Sng,
                _ => Sessions.SessionType.Cash
            };

        protected IEnumerable<string> Chunks
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
