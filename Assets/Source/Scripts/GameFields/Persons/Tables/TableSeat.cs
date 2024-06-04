using UnityEngine;
using Cards;
using Tools;
using GameFields.Effects;

namespace GameFields.Persons.Tables
{
    internal class TableSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private EffectRoot _effectRoot;
        private Effect _effect;

        internal bool IsEmpty => CardCharacter == null;
        internal bool CanBeDiscarded => _effect is {CountTurns: <= 0};
        internal CardCharacter CardCharacter { get; private set; }

        internal void Init(EffectRoot effectRoot)
        {
            _effectRoot = effectRoot;
        }

        internal void DecreaseCounter()
        {
            if (IsEmpty)
                return;

            _effect.DecreaseCounter();
        }

        internal void Discard() => CardCharacter = null;

        internal void SetCardCharacter(CardCharacter cardCharacter)
        {
            Vector2 cardCharacterPosition = new Vector2(0, 0);

            CardCharacter = cardCharacter;
            CardCharacter.Activate(_rectTransform, cardCharacterPosition);
            _effect = _effectRoot.PlayEffect(CardCharacter.Effect);
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

