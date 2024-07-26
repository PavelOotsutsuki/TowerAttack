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
        private readonly IPersonSides _personSides;

        public EffectRoot(SignalBus bus, Deck deck, IPersonSides personSides)
        {
            _bus = bus;
            _deck = deck;
            _personSides = personSides;
            
            _bus.Subscribe<CardPlayedSignal>(OnPlayCardSignal);
        }

        ~EffectRoot()
        {
            _bus.Unsubscribe<CardPlayedSignal>(OnPlayCardSignal);
        }

        private void OnPlayCardSignal(CardPlayedSignal signal)
        {
            Effect effect = PlayEffect(signal.Character.Effect);
            
            _bus.Fire(new EffectCreatedSignal(effect));
        }

        private Effect PlayEffect(EffectType effectType)
        {
            Effect effect = effectType switch
            {
                EffectType.ZhyzhaEffect => new ZhyzhaEffect(_personSides.DeactivePerson),
                EffectType.GreedyEffect => new GreedyEffect(_personSides.ActivePerson, _personSides.DeactivePerson),
                EffectType.PatriarchCorallEffect => new PatriarchCorallEffect(_deck, _personSides.ActivePerson, _personSides.DeactivePerson),
                _ => throw new NullReferenceException("Effect is not founded"),
            };

            return effect;
        }
    }
}
