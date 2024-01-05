using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerAI_2 : Tower_2, ICardDropPlaceImitation
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
