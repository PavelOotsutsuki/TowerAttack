using System;
using System.Collections.Generic;
using Cards;
using GameFields.Seats;
using Tools;
using Tools.Settings;
using Tools.Utils.FillComponents;
using Tools.Utils.Screens;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public abstract class Discover : MonoBehaviour, IDiscoverChoiceHandler, IActivatable<DiscoverActivateData>, IAutomaticFillComponents
    {
        [SerializeField] protected DiscoverSeat[] Seats;
        //[SerializeField] private float _offset = 400f;
        [SerializeField] private float _positionY = 0f;
        [SerializeField] private float _cardScaleFactor = 3f;
        [SerializeField] private float _viewDuration = 0.5f;

        protected IReadOnlyList<IDiscoverable> Cards;

        private DiscoverResult _currentResult;

        public int MaxSeats => Seats.Length;

        public virtual void Init()
        {
            InitSeats();

            gameObject.SetActive(false);
        }

        public void OnMakeChoice(IDiscoverable card)
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

            foreach (DiscoverSeat seat in Seats)
            {
                seat.Reset();
            }
        }

        //private void SortDiscoverSeats()
        //{
        //    float startPositionX;
        //    Vector3 seatPosition;

        //    if (Cards.Count % 2 == 1)
        //    {
        //        startPositionX = (Cards.Count / 2 * _offset) * -1;
        //    }
        //    else
        //    {
        //        startPositionX = ((Cards.Count / 2 - 1) * _offset + _offset / 2) * -1;
        //    }

        //    for (int i = 0; i < Cards.Count; i++)
        //    {
        //        //((RectTransform)Seats[i].transform).anchoredPosition = new Vector2(400f - 400f * i, 0f);
        //        //((RectTransform)Seats[i].transform).anchoredPosition = new Vector2(-570f + 570f * i, 0f);
        //        seatPosition = new Vector3(startPositionX + _offset * i, _positionY);

        //        Seats[i].SetLocalPositionValues(seatPosition, Quaternion.identity.eulerAngles);
        //    }
        //}

        private void SortDiscoverSeats()
        {
            float oneCardWidth = GameSettings.CardSize.x * _cardScaleFactor;

            float anchorMinX;
            float anchorMinY;
            float anchorMaxX;
            float anchorMaxY;
            Vector2 anchorPosition;

            if (Cards.Count == 1)
            {
                DiscoverSeat seat = Seats[0];

                anchorMinX = 0.5f;
                anchorMinY = 0.5f;
                anchorMaxX = 0.5f;
                anchorMaxY = 0.5f;
                anchorPosition = new Vector2(0f, _positionY);

                seat.SetRectTransformValues(anchorMinX, anchorMinY, anchorMaxX, anchorMaxY, anchorPosition);
                return;
            }

            if (Cards.Count == 2)
            {
                float centerOffset = oneCardWidth / 3f; // offset в 3 раза меньше карты, чтобы выглядело норм

                anchorMinX = 0.5f;
                anchorMinY = 0.5f;
                anchorMaxX = 0.5f;
                anchorMaxY = 0.5f;

                anchorPosition = new Vector2((centerOffset + oneCardWidth) / 2f * (-1f), _positionY);
                Seats[0].SetRectTransformValues(anchorMinX, anchorMinY, anchorMaxX, anchorMaxY, anchorPosition);

                anchorPosition = new Vector2((centerOffset + oneCardWidth) / 2f, _positionY);
                Seats[1].SetRectTransformValues(anchorMinX, anchorMinY, anchorMaxX, anchorMaxY, anchorPosition);
                return;
            }

            if (Cards.Count == 3)
            {
                float canvasX = ScreenView.X();

                // Левая карта
                anchorMinX = ((canvasX - oneCardWidth) / 2f) / canvasX;
                anchorMinY = 0.5f;
                anchorMaxX = anchorMinX;
                anchorMaxY = 0.5f;

                // Скраю в 2 раза больше расстояния, чем между картами ,соответственно на сторону 3 miniOffset,
                // в сумме с 2 сторон 6 miniOffset + 3 карты * oneCardWidth
                float miniOffset = (canvasX - oneCardWidth * Cards.Count) / 6f;

                anchorPosition = new Vector2((miniOffset + oneCardWidth / 2f) * (-1f), _positionY);
                Seats[0].SetRectTransformValues(anchorMinX, anchorMinY, anchorMaxX, anchorMaxY, anchorPosition);

                //Правая карта
                anchorMinX = 1 - anchorMinX;
                anchorMaxX = anchorMinX;
                anchorPosition = new Vector2(miniOffset + oneCardWidth / 2f, _positionY);

                Seats[2].SetRectTransformValues(anchorMinX, anchorMinY, anchorMaxX, anchorMaxY, anchorPosition);

                // Центральная карта
                anchorMinX = 0.5f;
                anchorMinY = 0.5f;
                anchorMaxX = 0.5f;
                anchorMaxY = 0.5f;
                anchorPosition = new Vector2(0f, _positionY);

                Seats[1].SetRectTransformValues(anchorMinX, anchorMinY, anchorMaxX, anchorMaxY, anchorPosition);
                return;
            }
        }

        private void InitSeats()
        {
            foreach (DiscoverSeat seat in Seats)
            {
                seat.Init(this, _cardScaleFactor, _viewDuration);
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