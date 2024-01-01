using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerPlayer_2 : Tower_2, ICardDropPlace
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
