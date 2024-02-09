using Cards;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    internal class TableAI : Table, ICardDropPlaceImitation
    {
        public override void Init(IPlayCardManager playCardManager)
        {
            base.Init(playCardManager);

            Deactivate();
        }

        public bool TryGetCard(Card card)
        {
            if (TryFindCardSeat(out TableSeat freeCardSeat))
            {
                PlayCardManager.PlayCard(card);
                card.Play(out CardCharacter cardCharacter);
                freeCardSeat.SetCardCharacter(cardCharacter);

                return true;
            }

            return false;
        }

        public Vector3 GetCentralСoordinates()
        {
            return transform.position;
        }

        private void Deactivate()
        {
            CanvasGroup.blocksRaycasts = false;
        }
    }
}
