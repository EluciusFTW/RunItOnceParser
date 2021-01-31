using RioParser.Domain.Extensions;

namespace RioParser.Domain.Hands
{
    public class CashGameHand : HandBase
    {
        public GameType Game
            => _header.Contains(PLOIndicator)
                ? GameType.PLO
                : _header.Contains(NLHIndicator)
                    ? GameType.NLH
                    : GameType.Unknown;

        public decimal Rake => _summary
            .AfterSingle("Rake €")
            .TakeAfter('.', 2)
            .ToDecimal();

        public decimal Splash => _action
           .AfterSingleOrDefault("STP added: €")?
           .TakeAfter('.', 2)
           .ToDecimal()
           ?? 0;

        public override decimal BigBlind => _intro
            .AfterSingle("/€")
            .TakeAfter('.', 2)
            .ToDecimal();

        public override decimal Total => _summary
           .AfterSingle("Total pot €")
           .TakeAfter('.', 2)
           .ToDecimal();

        public bool BigSplash => _showDown == null;

        public CashGameHand(string hand) 
            : base(hand)
        {
        }
    }
}