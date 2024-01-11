using Cards;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    internal class TablePlayer : Table, ICardDropPlace
    {
        public override void Init(IPlayCardManager playCardManager)
        {
            base.Init(playCardManager);

            Deactivate();
        }

        public void GetCard(Card card)
        {
            if (TryFindCardSeat(out TableSeat freeCardSeat))
            {
                PlayCardManager.PlayCard(card);
                card.Play(out CardCharacter cardCharacter);
                freeCardSeat.SetCardCharacter(cardCharacter);
            }
        }

        public void Activate()
        {
            CanvasGroup.blocksRaycasts = true;
        }

        public void Deactivate()
        {
            CanvasGroup.blocksRaycasts = false;
        }
    }
}