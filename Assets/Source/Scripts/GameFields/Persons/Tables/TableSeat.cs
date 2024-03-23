using UnityEngine;
using Cards;
using Tools;
using GameFields.Effects;

namespace GameFields.Persons.Tables
{
    internal class TableSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private CardCharacter _cardCharacter;
        private EffectRoot _effectRoot;
        private Effect _effect;

        internal void Init(EffectRoot effectRoot)
        {
            _effectRoot = effectRoot;
        }

        internal void SetCardCharacter(CardCharacter cardCharacter)
        {
            Vector2 cardCharacterPosition = new Vector2(0, 0);

            _cardCharacter = cardCharacter;
            _cardCharacter.transform.SetParent(_rectTransform);
            _cardCharacter.transform.localPosition = cardCharacterPosition;
            _cardCharacter.Activate();
            _effect = _effectRoot.PlayEffect(_cardCharacter.Effect);
        }

        internal bool TryDiscardCardCharacter(out CardCharacter cardCharacter)
        {
            cardCharacter = null;

            if (IsEmpty())
            {
                return false;
            }

            _effect.DecreaseCounter();

            if (_effect.CountTurns > 0)
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

