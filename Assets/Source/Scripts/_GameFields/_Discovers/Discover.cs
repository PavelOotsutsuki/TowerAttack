using System;
using System.Collections.Generic;
using Cards;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public abstract class Discover : MonoBehaviour, IDiscoverChoiceHandler
    {
        [SerializeField] protected DiscoverSeat[] Seats;
        [SerializeField] private float _offset = 400f;
        [SerializeField] private float _positionY = 0f;

        protected List<Card> Cards;

        private Action<Card> _callback;

        public int MaxSeats => Seats.Length;

        public virtual void Init()
        {
            InitSeats();

            gameObject.SetActive(false);
        }

        public void OnMakeChoice(Card card)
        {
            foreach (DiscoverSeat seat in Seats)
            {
                seat.Reset();
            }

            _callback?.Invoke(card);
            _callback = null;

            Deactivate();
        }

        public virtual void Activate(List<Card> cards, string activateMessage, Action<Card> callback)
        {
            Cards = cards;
            _callback = callback;

            SortDiscoverSeats();

            gameObject.SetActive(true);

            for (int i = 0; i < cards.Count; i++)
            {
                Seats[i].SetCard(cards[i]);
            }
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void SortDiscoverSeats()
        {
            float startPositionX;
            Vector3 seatPosition;

            if (Cards.Count % 2 == 1)
            {
                startPositionX = (Cards.Count / 2 * _offset) * -1;
            }
            else
            {
                startPositionX = ((Cards.Count / 2 - 1) * _offset + _offset / 2) * -1;
            }

            for (int i = 0; i < Cards.Count; i++)
            {
                seatPosition = new Vector3(startPositionX + _offset * i, _positionY);

                Seats[i].SetLocalPositionValues(seatPosition, Quaternion.identity.eulerAngles);
            }
        }

        private void InitSeats()
        {
            foreach (DiscoverSeat seat in Seats)
            {
                seat.Init(this);
            }
        }

        #region AutomaticFillComponents

        protected virtual void DefineAllComponents()
        {
            DefineSeats();
        }

        [ContextMenu(nameof(DefineSeats))]
        private void DefineSeats()
        {
            AutomaticFillComponents.DefineComponent(this, ref Seats);
        }

        #endregion
    }
}
