using RioParser.Domain.Extensions;
using RioParser.Domain.Hands;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Sessions
{
    public abstract class TourneySession : SessionBase
    {
        private IReadOnlyCollection<TourneyHand> _hands;
        public IReadOnlyCollection<TourneyHand> Hands
            => _hands ??= Chunks
                .Select(chunk => new TourneyHand(chunk))
                .ToList();

        public string Identifier
            => Headline
                .AfterFirst("Tournament #")
                .Before(",");

        protected TourneySession(string name, string content)
            : base(name, content)
        {
        }
    }
}
