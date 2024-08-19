using System.Collections;
using System.Collections.Generic;
using GameFields.DiscardPiles;
using GameFields.Effects;
using UnityEngine;

namespace GameFields
{
    public class TableManager
    {
        private EffectRoot _effectRoot;
        private DiscardPile _discardPile;
        private IPersonSideListener _personSideListener;

        public TableManager(EffectRoot effectRoot, DiscardPile discardPile, IPersonSideListener personSideListener)
        {
            _effectRoot = effectRoot;
            _discardPile = discardPile;
            _personSideListener = personSideListener;
        }
    }
}
