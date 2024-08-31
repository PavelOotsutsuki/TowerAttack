using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public class DiscoverSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private Card _card;
        private Movement _seatMovement;

        internal bool IsEmpty => _card == null;

        public void Init()
        {
            _seatMovement = new Movement(_rectTransform);
            Reset();
        }

        internal void SetCard(Card card)
        {
            _card = card;
            //_card.Discover();
            //_card.Transform.SetParent(_rectTransform);
            //_card.Transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
            _card.Discover(_rectTransform);
            //cardMovement.MoveLocalInstantly(Vector2.zero, Quaternion.identity.eulerAngles);
            //cardMovement.MoveLocalInstantly(Vector2.zero, Quaternion.identity.eulerAngles);
            //IncreaseCard(discoverMovement);
        }

        internal void Reset() => _card = null;

        internal bool IsCardEqual(Card card) => card == _card;

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration = 0f)
        {
            if (Mathf.Approximately(duration, 0f))
            {
                _seatMovement.MoveLocalInstantly(position, rotation);
            }
            else
            {
                _seatMovement.MoveLocalSmoothly(position, rotation, duration);
            }
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
