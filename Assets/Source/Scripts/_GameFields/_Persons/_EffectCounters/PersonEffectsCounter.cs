using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFields.Persons.Common;

namespace GameFields.Persons.EffectCounters
{
    public class PersonEffectsCounter
    {
        private readonly GnomeEffectCounter _gnomeEffectCounter;

        public PersonEffectsCounter(RechangeFeatureRuleController ruleController, IEnumerable<ICardFeatureRechangable> rechangables)
        {
            _gnomeEffectCounter = new GnomeEffectCounter(ruleController, rechangables);
        }

        public GnomeEffectCounter GnomeEffectCounter => _gnomeEffectCounter;
    }
}
