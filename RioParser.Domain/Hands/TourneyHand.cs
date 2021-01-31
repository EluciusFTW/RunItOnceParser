namespace RioParser.Domain.Hands
{
    public class TourneyHand : HandBase
    {

        public TourneyHand(string hand) : base(hand)
        {
        }

        public override decimal Total => throw new System.NotImplementedException();

        public override decimal BigBlind => throw new System.NotImplementedException();
    }
}