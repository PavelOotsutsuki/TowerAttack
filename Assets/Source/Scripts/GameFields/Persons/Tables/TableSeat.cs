using UnityEngine;
using Cards;
using Tools;
using GameFields.Effects;
//using System;

namespace GameFields.Persons.Tables
{
    internal class TableSeat : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        private CardCharacter _cardCharacter;
        //private EffectRoot _effectRoot;
        //private Effect _effect;

        //internal bool IsEmpty => _cardCharacter == null;
        //internal bool IsDiscarded => _effect.CountTurns <= 0;
        //internal CardCharacter CardCharacter => _cardCharacter;

        //internal void Init(EffectRoot effectRoot)
        //{
        //    _effectRoot = effectRoot;
        //}

        //public void DecreaseCounter()
        //{
        //    if (IsEmpty)
        //    {
        //        return;
        //    }

        //    _effect.DecreaseCounter();
        //}

        internal void SetCharacter(CardCharacter cardCharacter)
        {
            _cardCharacter = cardCharacter;
            //_cardCharacter.transform.SetParent(_rectTransform);
            //_cardCharacter.transform.localPosition = cardCharacterPosition;
            _cardCharacter.Activate(_rectTransform);
            //_effect = _effectRoot.PlayEffect(_cardCharacter.Effect);
        }

        //internal bool TryDiscardCardCharacter(out CardCharacter cardCharacter)
        //{
        //    cardCharacter = null;

        //    if (IsEmpty())
        //    {
        //        return false;
        //    }

        //    _effect.DecreaseCounter();

        //    if (_effect.CountTurns > 0)
        //    {
        //        return false;
        //    }

        //    cardCharacter = _cardCharacter;
        //    _cardCharacter = null;
        //    return true;
        //}

        internal void DiscardCharacter()
        {
            _cardCharacter = null;
        }

        internal bool IsEqualCharacter(CardCharacter character)
        {
            return character == _cardCharacter;
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

