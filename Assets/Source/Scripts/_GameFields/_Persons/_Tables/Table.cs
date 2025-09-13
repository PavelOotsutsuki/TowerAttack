using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Persons.Commons;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public abstract class Table : MonoBehaviour, IDiscardManager, IAutomaticFillComponents
    {
        [SerializeField] private TableSeat[] _tableSeats;

        private TableSeat[] _sortedSeats;

        public bool HasFreeSeat => _sortedSeats.Any(seat => seat.IsEmpty);
        public IEnumerable<Card> Cards => _sortedSeats.Where(s => s.IsEmpty == false).Select(s => s.PersonEffect.Card);

        public void Init()
        {
            SetCardSeatsIndices();
        }

        //public void FreeSeats(IEnumerable<Card> seatables)
        //{
        //    foreach (TableSeat seat in _tableSeats)
        //    {
        //        if (seatables.Any(card => seat.IsCardEqual(card)))
        //        {
        //            seat.Reset();
        //        }
        //    }
        //}
        public bool HasCard(Card card)
        {
            return _sortedSeats.Where(s => s.IsEmpty == false).Any(s => s.PersonEffect.Card == card);
        }

        public void Discard(Card card)
        {
            TableSeat tableSeat = _sortedSeats.Where(s => s.IsEmpty == false).FirstOrDefault(s => s.PersonEffect.Card == card);
            //TableSeat tableSeat = _sortedSeats.FirstOrDefault(s => s.PersonEffect.Card == card);

            if (tableSeat == null)
                throw new System.Exception("Пытаешься сбросить карты которой нет на столе");

            PersonEffect personEffect = tableSeat.PersonEffect;

            personEffect.Discard();
            tableSeat.Reset();
        }

        public void TryDiscard(Card card)
        {
            TableSeat tableSeat = _sortedSeats.Where(s => s.IsEmpty == false).FirstOrDefault(s => s.PersonEffect.Card == card);
            //TableSeat tableSeat = _sortedSeats.FirstOrDefault(s => s.PersonEffect.Card == card);

            if (tableSeat == null)
                throw new System.Exception("Пытаешься сбросить карты которой нет на столе");

            PersonEffect personEffect = tableSeat.PersonEffect;

            if (personEffect.TryDiscard())
            {
                tableSeat.Reset();
            }

            //personEffect.DecreaseCounter();
        }

        //public void SeatCard(Card card)
        //{
        //    TableSeat freeCardSeat = GetFreeSeat();
        //    freeCardSeat.SetCard(card);
        //}

        public void SeatCard(PersonEffect personEffect)
        {
            TableSeat freeCardSeat = GetFreeSeat();
            freeCardSeat.SetCard(personEffect);
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