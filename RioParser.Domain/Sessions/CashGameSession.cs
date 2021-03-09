using RioParser.Domain.Hands;
using System.Collections.Generic;
using System.Linq;

namespace RioParser.Domain.Sessions
{
    public class CashGameSession : SessionBase
    {
        private IReadOnlyCollection<CashGameHand> _hands;
        public IReadOnlyCollection<CashGameHand> Hands
            => _hands ??= Chunks
                .Select(chunk => new CashGameHand(chunk))
                .ToList();

        public CashGameSession(string name, string content)
            : base(name, content)
        {
        }
    }
}
