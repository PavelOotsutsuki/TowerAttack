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

        public void GetCard(Card card)
        {
            PlayCardManager.PlayCard(card);

            TowerSeat.GetCard(card);

            Deactivate();
        }

        private void Activate()
        {
            CanvasGroup.blocksRaycasts = true;
        }
    }
}
