using Cards;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    internal class TableAI_2 : Table_2, ICardDropPlaceImitation
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

        private void Deactivate()
        {
            CanvasGroup.blocksRaycasts = false;
        }
    }
}
