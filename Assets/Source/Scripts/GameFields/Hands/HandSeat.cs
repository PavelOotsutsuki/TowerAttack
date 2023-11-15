using UnityEngine;
using Cards;
using Tools;

namespace GameFields.Hands
{
    public class HandSeat : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        public Card Card { get; private set; }

        public void Init()
        {
            Card = null;
        }

        public void SetCard(Card card, float duration)
        {
            Card = card;
            Debug.Log(Card == null);
            Vector2 cardLocalPosition = new Vector2(0, 0);
            Card.transform.SetParent(_transform);
            //Card.transform.localPosition = cardLocalPosition;
            Card.TranslateLocalInto(cardLocalPosition, Quaternion.identity.eulerAngles, duration);
        }

        public void SetLocalPositionValues(Vector3 position, Vector3 rotation)
        {
            _transform.localPosition = position;
            _transform.rotation = Quaternion.Euler(rotation);
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
