using UnityEngine;
using Cards;
using System;
using GameFields.DiscardPiles;

namespace GameFields.Effects
{
    public class EffectRoot
    {
        private readonly Deck _deck;
        private readonly MonoBehaviour _coroutineContainer;
        private DiscardPile _discardPile;
        private IPersonSideListener _personSideListener;
        //private List<ActiveEffect> _activeEffects;

        public EffectRoot(Deck deck, DiscardPile discardPile, IPersonSideListener personSideListener, MonoBehaviour coroutineContainer)
        {
            //_activeEffects = new List<ActiveEffect>();

            _deck = deck;
            _discardPile = discardPile;
            _personSideListener = personSideListener;
            _coroutineContainer = coroutineContainer;
        }

        public Effect PlayEffect(EffectType effectType)
        {
            Effect effect = effectType switch
            {
                EffectType.ZhyzhaEffect => new ZhyzhaEffect(_personSideListener.DeactivePerson),
                EffectType.GreedyEffect => new GreedyEffect(_personSideListener.ActivePerson, _personSideListener.DeactivePerson),
                EffectType.PatriarchCorallEffect => new PatriarchCorallEffect(_deck, _personSideListener.ActivePerson,
                    _personSideListener.DeactivePerson, _coroutineContainer),
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
