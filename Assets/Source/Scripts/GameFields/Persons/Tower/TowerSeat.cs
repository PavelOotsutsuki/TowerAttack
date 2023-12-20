using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using Tools;
using UnityEngine.EventSystems;

namespace GameFields.Persons.Towers
{
    internal class TowerSeat : MonoBehaviour, ICardDropPlace
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Min(0f)] private float _duration;

        private Card _card;

        public void Init()
        {
            Activate();
        }

        //public void OnDrop(PointerEventData eventData)
        //{
        //    //if (EventSystem.current.TryGetComponentInRaycasts(eventData, out Card card))
        //    //{
        //    Debug.Log("Сработал");

        //    if (eventData.pointerDrag.TryGetComponent(out Card card))
        //    {
        //        Debug.Log("еще раз сработал");
        //        SetCard(card, _duration);
        //        Deactivate();
        //    }
        //    //}
        //}

        public void GetCard(Card card)
        {
            SetCard(card, _duration);

            Deactivate();
        }

        public void SetCard(Card card, float duration)
        {
            _card = card;
            _card.BindSeat(_transform, duration);
        }

        public bool IsCardEqual(Card card)
        {
            return _card == card;
        }

        private void Activate()
        {
            _canvasGroup.blocksRaycasts = true;
        }

        private void Deactivate()
        {
            _canvasGroup.blocksRaycasts = false;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTransform();
        }

        [ContextMenu(nameof(DefineTransform))]
        private void DefineTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
    }
}
