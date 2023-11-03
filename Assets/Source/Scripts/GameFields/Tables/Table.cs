using UnityEngine;
using UnityEngine.EventSystems;
using Cards;
using Tools;

namespace GameFields.Tables
{
    internal class Table : MonoBehaviour, IDropHandler
    {
        [SerializeField] private CardSeat[] _cardSeats;

        private int[] _cardSeatsSortIndices;
        private ICardFromHandIntoTableLayable _cardLayable;

        public void Init(ICardFromHandIntoTableLayable cardlayable)
        {
            _cardLayable = cardlayable;
            SetCardSeatsIndices();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (EventSystem.current.TryGetComponentInRaycasts(eventData, out Card card))
            {
                //if (eventData.pointerDrag.TryGetComponent(out Card card))
                //{
                    if (TryFindCardSeat(out CardSeat freeCardSeat))
                    {
                        _cardLayable.LayInTableFromHand(card);
                        card.AddToTable(out CardCharacter cardCharacter);
                        freeCardSeat.SetCardCharacter(cardCharacter);
                    }
                //}
            }
        }

        private bool TryFindCardSeat(out CardSeat cardSeat)
        {
            cardSeat = null;

            foreach (int index in _cardSeatsSortIndices)
            {
                if (_cardSeats[index].IsEmpty())
                {
                    cardSeat = _cardSeats[index];
                    return true;
                }
            }

            return false;
        }

        private void SetCardSeatsIndices()
        {
            int countSeats = _cardSeats.Length;

            _cardSeatsSortIndices = new int[countSeats];

            for (int i = 0; i < countSeats; i++)
            {
                _cardSeatsSortIndices[i] = GetSortIndex(i, countSeats);
            }
        }

        private int GetSortIndex(int inputIndex, int countSeats)
        {
            return (countSeats + 1) / 2 + (inputIndex + 1) / 2 * ((inputIndex + 1) % 2 * 2 - 1) - 1;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineAllCardSeats();
        }

        [ContextMenu(nameof(DefineAllCardSeats))]
        private void DefineAllCardSeats()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardSeats);
        }

    }
}