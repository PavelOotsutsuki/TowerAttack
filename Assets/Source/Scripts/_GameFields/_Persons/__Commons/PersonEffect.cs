using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Effects;
using UnityEngine;

namespace GameFields.Persons.Commons
{
    public class PersonEffect
    {
        //private readonly Card _card;
        //private readonly CardEffectConfig _cardEffectConfig;

        //public CardEffectConfigPair(Card card, CardEffectConfig cardEffectConfig)
        //{
        //    _card = card;
        //    _cardEffectConfig = cardEffectConfig;
        //}

        //public Card Card => _card;
        //public CardEffectConfig CardEffectConfig => _cardEffectConfig;

        private readonly CardEffectConfigPair _cardEffectConfigPair;
        private readonly Effect _effect;

        private int _duration;

        public PersonEffect(Effect effect, CardEffectConfigPair cardEffectConfigPair)
        {
            _cardEffectConfigPair = cardEffectConfigPair;
            _effect = effect;

            _duration = _effect.Duration;
        }

        public Card Card => _cardEffectConfigPair.Card;
        public CardEffectConfig CardEffectConfig => _cardEffectConfigPair.CardEffectConfig;
        public Effect Effect => _effect;

        public void Discard()
        {
            _duration = 0;

            TryDiscard();
        }

        public void DecreaseCounter()
        {
            _duration--;
        }

        public bool TryDiscard()
        {
            if (_duration <= 0)
            {
                _effect?.End();
                return true;
            }

            DecreaseCounter();
            return false;
        }
    }
}
