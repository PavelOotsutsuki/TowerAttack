using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.Persons.Discovers
{
    internal class DiscoverSeatImitation : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private DiscoverCardImitation _discoverCard;

        private Card _card;
        private Movement _seatMovement;
        private IDiscoverChoiceHandler _discoverChoiceHandler;

        internal bool IsEmpty => _card == null;

        public void Init(IDiscoverChoiceHandler discoverChoiceHandler, float scaleFactor)
        {
            _seatMovement = new Movement(_rectTransform);
            _discoverChoiceHandler = discoverChoiceHandler;
            _discoverCard.Init(OnDiscoverCardClick, scaleFactor);
            Reset();
        }

        internal void SetCard(Card card, float waitDuration = 0f)
        {
            _card = card;
            //_card.Discover();
            //_card.Transform.SetParent(_rectTransform);
            //_card.Transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
            _discoverCard.Activate(_card.Transform.sizeDelta.y, _card.Transform.sizeDelta.x);
            //_card.Discover(_rectTransform);
            //cardMovement.MoveLocalInstantly(Vector2.zero, Quaternion.identity.eulerAngles);
            //cardMovement.MoveLocalInstantly(Vector2.zero, Quaternion.identity.eulerAngles);
            //IncreaseCard(discoverMovement);
        }

        internal void StartClickImitation()
        {
            _discoverCard.StartClickImitation();
        }

        internal void Reset()
        {
            _card = null;
            _discoverCard.Hide();
        }

        internal bool IsCardEqual(Card card) => card == _card;

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        {
            _seatMovement.MoveLocalSmoothly(position, rotation, duration);
        }

        private void OnDiscoverCardClick()
        {
            _discoverChoiceHandler.OnMakeChoice(_card);
        }

        //private void IncreaseCard(Movement discoverMovement)
        //{
        //    Vector3 position = _rectTransform.position;

        //    discoverMovement.MoveInstantly(position, Quaternion.identity.eulerAngles, Vector3.zero);
        //    discoverMovement.MoveSmoothly(position, Quaternion.identity.eulerAngles, 1f, _card.DefaultScaleVector * 2);
        //}

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}
