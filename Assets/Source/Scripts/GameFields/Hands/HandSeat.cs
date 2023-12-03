using UnityEngine;
using Cards;
using Tools;
using DG.Tweening;

namespace GameFields.Hands
{
    public class HandSeat : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private Card _card;
        //public Card Card { get; private set; }

        public void Init()
        {
            //Card = null;
        }

        public void SetCard(Card card, float duration)
        {
            _card = card;
            _card.BindSeat(_transform, duration);
            //Card.transform.localPosition = cardLocalPosition;
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
