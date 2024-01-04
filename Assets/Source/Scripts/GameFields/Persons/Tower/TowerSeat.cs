using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerSeat : MonoBehaviour
    {
        private const bool IsFrontCardSide = false;

        [SerializeField] private Transform _transform;
        [SerializeField, Min(0f)] private float _duration;

        private Card _card;

        public void Init()
        {
            ResetCard();
        }

        public void ResetCard()
        {
            _card = null;
        }

        public void GetCard(Card card)
        {
            SetCard(card, _duration);
        }

        public void SetCard(Card card, float duration)
        {
            _card = card;
            _card.BindSeat(_transform, IsFrontCardSide, duration);
        }

        public bool IsCardEqual(Card card)
        {
            return _card == card;
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
