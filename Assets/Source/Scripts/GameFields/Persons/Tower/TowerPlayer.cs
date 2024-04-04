using Cards;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerPlayer : Tower, ICardDropPlace
    {
        public override void Init(IPlayCardManager playCardManager)
        {
            base.Init(playCardManager);

            Activate();
        }

        public bool TrySeatCard(Card card)
        {
            if (TowerSeat.TryGetCard(card))
            {
                PlayCardManager.PlayCard(card);

                Deactivate();

                return true;
            }

            return false;
        }

        public void Activate()
        {
            if (TowerSeat.IsFill() == false)
            {
                CanvasGroup.blocksRaycasts = true;
            }
        }
    }
}
