using RioParser.Domain.Hands;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Sessions
{
    public abstract class TourneySession : SessionBase
    {
        private IReadOnlyCollection<TourneyHand> _hands;
        public IReadOnlyCollection<TourneyHand> Hands
            => _hands
                ?? (_hands = Chunks
                    .Select(chunk => new TourneyHand(chunk))
                    .ToList());

        protected TourneySession(string name, string content)
            : base(name, content)
        {
        }

    }
}
