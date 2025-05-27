using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFields.Persons.Common;

namespace GameFields.Persons.EffectCounters
{
    public class PersonEffectsCounter
    {
        private readonly GnomeEffectCounter _gnomeEffectCounter;

        public PersonEffectsCounter(GnomeEffectCounter gnomeEffectCounter)
        {
            _gnomeEffectCounter = gnomeEffectCounter;
        }

        public GnomeEffectCounter GnomeEffectCounter => _gnomeEffectCounter;
    }
}
