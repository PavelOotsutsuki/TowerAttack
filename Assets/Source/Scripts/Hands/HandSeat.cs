using UnityEngine;
using Cards;
using Tools;
using DG.Tweening;

namespace Hands
{
    public class HandSeat : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private Card _card;

        public void Init()
        {
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

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Disactivate()
        {
            gameObject.SetActive(false);
        }

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation, float duration)
        {
            _transform.DOLocalMove(position, duration);
            _transform.DOLocalRotate(rotation, duration);
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