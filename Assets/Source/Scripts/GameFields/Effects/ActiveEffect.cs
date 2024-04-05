using System.Collections;
using System.Collections.Generic;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class ActiveEffect
    {
        private readonly IPerson _person;
        private readonly Effect _effect;

        public ActiveEffect(IPerson person, Effect effect)
        {
            _person = person;
            _effect = effect;
        }

        public int CountTurns => _effect.CountTurns;

        public void DecreaseEffectCounter(IPerson activePerson)
        {
            if (activePerson == _person)
            {
                _effect.DecreaseCounter();
            }
        }
    }
}
