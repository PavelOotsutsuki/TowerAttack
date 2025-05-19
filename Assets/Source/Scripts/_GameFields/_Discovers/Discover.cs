using System;
using System.Collections.Generic;
using Cards;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public abstract class Discover : MonoBehaviour, IDiscoverChoiceHandler, IActivatable<DiscoverActivateData>, IAutomaticFillComponents
    {
        [SerializeField] protected DiscoverSeat[] Seats;
        [SerializeField] private float _offset = 400f;
        [SerializeField] private float _positionY = 0f;

        protected IReadOnlyList<Card> Cards;

        private DiscoverResult _currentResult;

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

            _currentResult.SetResult(card);

            Deactivate();
        }

        public virtual void Activate(DiscoverActivateData data)
        {
            Cards = data.Cards;
            _currentResult = data.DiscoverResult;

            SortDiscoverSeats();

            gameObject.SetActive(true);

            for (int i = 0; i < Cards.Count; i++)
            {
                Seats[i].SetCard(Cards[i]);
            }
        }

        protected virtual void Deactivate()
        {
            gameObject.SetActive(false);
            _currentResult.SetComplete();
            _currentResult = null;
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
        [ContextMenu(nameof(DefineAllComponents) + nameof(Discover))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineSeats()
            };

            return list;
        }

        [ContextMenu(nameof(DefineSeats))]
        private ComponentAttachInfo DefineSeats()
        {
           return AutomaticFillComponents.DefineComponent(this, ref Seats);
        }

        #endregion
    }
}