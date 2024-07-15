using Cards;
using System;
using GameFields.Discarding;
using Zenject;

namespace GameFields.Effects
{
    public class EffectRoot
    {
        private readonly SignalBus _bus;
        private readonly Deck _deck;
        private readonly IPersonSideListener _personSideListener;

        public EffectRoot(SignalBus bus, Deck deck, IPersonSideListener personSideListener)
        {
            _bus = bus;
            _deck = deck;
            _personSideListener = personSideListener;
            
            _bus.Subscribe<CardPlayedSignal>(OnPlayCardSignal);
        }

        ~EffectRoot()
        {
            _bus.Unsubscribe<CardPlayedSignal>(OnPlayCardSignal);
        }

        private void OnPlayCardSignal(CardPlayedSignal signal)
        {
            Effect effect = PlayEffect(signal.Character.Effect);
            
            _bus.Fire(new EffectCreatedSignal(signal.Character, effect));
        }

        private Effect PlayEffect(EffectType effectType)
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
    }
}
