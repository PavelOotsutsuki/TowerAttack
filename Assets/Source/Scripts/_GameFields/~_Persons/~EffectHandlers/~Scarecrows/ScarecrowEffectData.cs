using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers.Scarecrows
{
    internal class ScarecrowEffectData
    {
        private readonly Card _card;

        private int _duration;

        public ScarecrowEffectData(Card card, int duration) 
        {
            _card = card;
            _duration = duration;
        }

        public bool NeedDelete => _duration <= 0;
        public Card Card => _card;

        public void Use()
        {
            _duration--;
        }
    }
}
