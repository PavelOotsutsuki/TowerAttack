using UnityEngine;
using Cards;
using Tools;

namespace GameFields.Tables
{
    internal class TableSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        //internal CardCharacter CardCharacter { get; private set; }
        private CardCharacter _cardCharacter;

        internal void Init()
        {
            _cardCharacter = null;
        }

        internal void SetCardCharacter(CardCharacter cardCharacter)
        {
            Vector2 cardCharacterPosition = new Vector2(0, 0);
            //CardCharacter = Instantiate(cardCharacter, _rectTransform);
            _cardCharacter = cardCharacter;
            _cardCharacter.transform.SetParent(_rectTransform);
            _cardCharacter.transform.localPosition = cardCharacterPosition;
            _cardCharacter.Activate();
        }

        internal bool IsEmpty()
        {
            return _cardCharacter == null;
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

