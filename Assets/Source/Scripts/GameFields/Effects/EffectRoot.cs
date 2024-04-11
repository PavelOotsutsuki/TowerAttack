using GameFields.Persons;
using Tools;
using UnityEngine;
using Cards;
using System;
using GameFields.DiscardPiles;

namespace GameFields.Effects
{
    public class EffectRoot
    {
        private Deck _deck;
        private DiscardPile _discardPile;
        private IPersonSideListener _personSideListener;
        //private List<ActiveEffect> _activeEffects;

        public EffectRoot(Deck deck, DiscardPile discardPile, IPersonSideListener personSideListener)
        {
            //_activeEffects = new List<ActiveEffect>();

            _deck = deck;
            _discardPile = discardPile;
            _personSideListener = personSideListener;
        }

        public Effect PlayEffect(EffectType effectType)
        {
            Effect effect = effectType switch
            {
                EffectType.ZhyzhaEffect => new ZhyzhaEffect(_personSideListener.DeactivePerson),
                EffectType.GreedyEffect => new GreedyEffect(_personSideListener.ActivePerson, _personSideListener.DeactivePerson),
                EffectType.PatriarchCorallEffect => new PatriarchCorallEffect(_deck, _personSideListener.ActivePerson, _personSideListener.DeactivePerson),
                _ => throw new NullReferenceException("Effect is not founded"),
            };

            return effect;
        }

        //public void DecreaseEffectCounter()
        //{
        //    foreach (ActiveEffect activeEffect in _activeEffects)
        //    {
        //        activeEffect.DecreaseEffectCounter(_personSideListener.ActivePerson);

        //        if (activeEffect.CountTurns <= 0)
        //        {
        //            _activeEffects.Remove(activeEffect);
        //        }
        //    }
        //}
    }
}
