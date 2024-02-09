using Cards;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    internal class TowerPlayer : Tower, ICardDropPlace
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
            if (TowerSeat.IsVoid())
            {
                CanvasGroup.blocksRaycasts = true;
            }
        }
    }
}
