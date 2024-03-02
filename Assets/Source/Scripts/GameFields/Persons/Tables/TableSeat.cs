using UnityEngine;
using Cards;
using Tools;

namespace GameFields.Persons.Tables
{
    internal class TableSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private CardCharacter _cardCharacter;
        private CardEffects _cardEffects;

        internal void Init(CardEffects cardEffects)
        {
            _cardEffects = cardEffects;
        }

        internal void SetCardCharacter(CardCharacter cardCharacter)
        {
            EffectType effectType;
            Vector2 cardCharacterPosition = new Vector2(0, 0);

            _cardCharacter = cardCharacter;
            _cardCharacter.transform.SetParent(_rectTransform);
            _cardCharacter.transform.localPosition = cardCharacterPosition;
            effectType = _cardCharacter.Activate();

            _cardEffects.PlayEffect(effectType);
        }

        internal bool TryDiscardCardCharacter(out CardCharacter cardCharacter)
        {
            cardCharacter = null;

            if (IsEmpty())
            {
                return false;
            }

            cardCharacter = _cardCharacter;
            _cardCharacter = null;
            return true;
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

