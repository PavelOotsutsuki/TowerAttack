using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.CardTransits;
using GameFields.Persons;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public abstract class Table : MonoBehaviour, IDiscardManager, ICardView, IAutomaticFillComponents
    {
        [SerializeField] private TableSeat[] _tableSeats;

        private TableSeat[] _sortedSeats;

        public bool HasFreeSeat => _sortedSeats.Any(seat => seat.IsEmpty);
        public IEnumerable<Card> AllCards => _sortedSeats.Where(s => s.IsEmpty == false).Select(s => s.PersonEffect.Card);

        public void Init()
        {
            SetCardSeatsIndices();
        }

        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions)
        {
            List<int> existingIndices = new List<int>();
            List<Card> result = new List<Card>();

            IReadOnlyList<Card> cards = Utils.Shuffle(AllCards);

            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (existingIndices.Contains(cards[i].ViewData.Number) == false && exceptions.Contains(cards[i].ViewData.Number) == false)
                    {
                        result.Add(cards[i]);
                        existingIndices.Add(cards[i].ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (exceptions.Contains(cards[i].ViewData.Number) == false)
                        {
                            result.Add(cards[i]);
                            existingIndices.Add(cards[i].ViewData.Number);
                        }
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        result.Add(cards[i]);
                        existingIndices.Add(cards[i].ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
                throw new Exception("Ошибка вычисления чисел. Слишком мало карт!");

            return result;
        }

        public bool Contains(int number)
        {
            return AllCards.Select(с => с.ViewData.Number).Contains(number);
        }

        public bool IsHasCards(int count, IEnumerable<int> exceptions = null)
        {
            exceptions ??= new List<int>();

            return AllCards.Where(s => exceptions.Contains(s.ViewData.Number) == false).Count() >= count;
        }

        public int IndexOf(Card card)
        {
            for (int i = 0; i < _sortedSeats.Length; i++)
            {
                if (_sortedSeats[i].PersonEffect.Card == card)
                    return i;
            }

            return -1;
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