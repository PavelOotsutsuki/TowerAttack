using Cards;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerAI : Tower, ICardDropPlaceImitation
    {
        public override void Init(IPlayCardManager playCardManager)
        {
            base.Init(playCardManager);

            Deactivate();
        }

        public bool TryGetCard(Card card)
        {
            if (TowerSeat.TryGetCard(card))
            {
                PlayCardManager.PlayCard(card);

                return true;
            }

            return false;
        }

        public Vector3 GetCentralСoordinates()
        {
            return transform.position;
        }
    }
}
