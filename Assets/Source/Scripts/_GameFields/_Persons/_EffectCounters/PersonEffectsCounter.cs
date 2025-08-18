using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Persons.EffectCounters
{
    public class PersonEffectsCounter
    {
        private readonly GnomeEffectCounter _gnomeEffectCounter;

        public PersonEffectsCounter()
        {
            _gnomeEffectCounter = new GnomeEffectCounter();
        }

        public GnomeEffectCounter GnomeEffectCounter => _gnomeEffectCounter;
    }
}
