using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    internal abstract class Table : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup CanvasGroup;
        [SerializeField] private TableSeat[] _cardSeats;

        protected IPlayCardManager PlayCardManager;
        private int[] _cardSeatsSortIndices;

        public virtual void Init(IPlayCardManager playCardManager)
        {
            PlayCardManager = playCardManager;
            SetCardSeatsIndices();
        }

        public List<CardCharacter> GetAllCardCharacters()
        {
            List<CardCharacter> discardCards = new List<CardCharacter>();

            foreach (TableSeat tableSeat in _cardSeats)
            {
                if (tableSeat.TryDiscardCardCharacter(out CardCharacter cardCharacter))
                {
                    discardCards.Add(cardCharacter);
                }
            }

            return discardCards;
        }

        protected bool TryFindCardSeat(out TableSeat cardSeat)
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
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineAllCardSeats))]
        private void DefineAllCardSeats()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardSeats);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref CanvasGroup, ComponentLocationTypes.InThis);
        }
    }
}
