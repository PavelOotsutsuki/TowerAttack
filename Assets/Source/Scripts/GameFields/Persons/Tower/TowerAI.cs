using Cards;

namespace GameFields.Persons.Towers
{
    public class TowerAI : Tower, ICardDropPlaceImitation
    {
        public override void Init(IPlayCardManager playCardManager)
        {
            base.Init(playCardManager);

            Deactivate();
        }

        public void GetCard(Card card)
        {
            PlayCardManager.PlayCard(card);

            TowerSeat.GetCard(card);
        }
    }
}
