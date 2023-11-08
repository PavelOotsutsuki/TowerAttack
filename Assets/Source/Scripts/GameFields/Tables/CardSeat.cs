using UnityEngine;
using Cards;
using Tools;

namespace GameFields.Tables
{
    internal class CardSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        internal CardCharacter CardCharacter { get; private set; }

        internal void Init()
        {
            CardCharacter = null;
        }

        internal void SetCardCharacter(CardCharacter cardCharacter)
        {
            Vector2 cardCharacterPosition = new Vector2(0, 0);
            CardCharacter = Instantiate(cardCharacter, _rectTransform);
            CardCharacter.transform.localPosition = cardCharacterPosition;
            CardCharacter.Activate();
        }

        internal bool IsEmpty()
        {
            return CardCharacter == null;
        }

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
    }
}

