using System.Collections.Generic;
using System.Linq;
using Cards;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public abstract class Table : MonoBehaviour
    {
        [SerializeField] private TableSeat[] _cardSeats;

        private TableSeat[] _sortedSeats;

        public bool HasFreeSeat => _sortedSeats.Any(seat => seat.IsEmpty);

        public void Init()
        {
            SetCardSeatsIndices();
        }

        public void FreeSeats(IEnumerable<Card> seatables)
        {
            foreach(TableSeat seat in _cardSeats)
            {
                if (seatables.Any(card => seat.IsCardEqual(card)))
                {
                    seat.Reset();
                }
            }
        }

        public void SeatCard(Card card)
        {
            TableSeat freeCardSeat = GetFreeSeat();
            freeCardSeat.SetCard(card);
        }

        private TableSeat GetFreeSeat() => _sortedSeats.First(seat => seat.IsEmpty);

        private void SetCardSeatsIndices()
        {
            int countSeats = _cardSeats.Length;

            _sortedSeats = new TableSeat[countSeats];

            for (int i = 0; i < countSeats; i++)
            {
                _sortedSeats[i] = _cardSeats[GetSortIndex(i, countSeats)];
            }
        }

        private static int GetSortIndex(int inputIndex, int countSeats)
        {
            return (countSeats + 1) / 2 + (inputIndex + 1) / 2 * ((inputIndex + 1) % 2 * 2 - 1) - 1;
        }

        #region AutomaticFillComponents
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
        #endregion 
    }
}
