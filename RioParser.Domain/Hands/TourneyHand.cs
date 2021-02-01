using RioParser.Domain.Extensions;

namespace RioParser.Domain.Hands
{
    public class TourneyHand : HandBase
    {
        public decimal EntryFee => Rake + Prize;
        
        public decimal Rake => _intro
            .AfterSingle("+€")
            .Before(" ")
            .ToDecimal();

        public decimal Prize => _intro
            .BetweenSingle("€", "+€")
            .ToDecimal();

        public TourneyHand(string hand) : base(hand)
        {
        }

        public override decimal Total => throw new System.NotImplementedException();

        public override decimal BigBlind => throw new System.NotImplementedException();
    }
}