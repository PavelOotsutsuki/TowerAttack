using System.Collections.Generic;
using System.Linq;
using Cards;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public abstract class Table : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private TableSeat[] _tableSeats;

        private TableSeat[] _sortedSeats;

        public bool HasFreeSeat => _sortedSeats.Any(seat => seat.IsEmpty);

        public void Init()
        {
            SetCardSeatsIndices();
        }

        public void FreeSeats(IEnumerable<Card> seatables)
        {
            foreach (TableSeat seat in _tableSeats)
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
            int countSeats = _tableSeats.Length;

            _sortedSeats = new TableSeat[countSeats];

            for (int i = 0; i < countSeats; i++)
            {
                _sortedSeats[i] = _tableSeats[GetSortIndex(i, countSeats)];
            }
        }

        private static int GetSortIndex(int inputIndex, int countSeats)
        {
            return (countSeats + 1) / 2 + (inputIndex + 1) / 2 * ((inputIndex + 1) % 2 * 2 - 1) - 1;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Table))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAllTableSeats()
            };

            return list;
        }

        [ContextMenu(nameof(DefineAllTableSeats))]
        private ComponentAttachInfo DefineAllTableSeats()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _tableSeats);
        }
        #endregion 
    }
}