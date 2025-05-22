using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace GameFields.Persons.EffectCounters
{
    public class GnomeEffectCounter
    {
        private readonly int _upgradeStepCount;

        private int _gnomeCounterNumbers;
        private int _gnomeCounterUse;

        public GnomeEffectCounter()
        {
            _upgradeStepCount = 2;

            _gnomeCounterNumbers = 3;
            _gnomeCounterUse = 1;
        }

        public void Upgrade(Card card)
        {
            _gnomeCounterNumbers += _upgradeStepCount;

            List<TagValuePair> tagValuePairs = new List<TagValuePair>
            {
                new TagValuePair("NUMBERS", _gnomeCounterNumbers)
            };

            card.RechangeFeature(tagValuePairs);
        }

        public bool TryActivate(out int countNumbers)
        {
            countNumbers = 0;

            if (_gnomeCounterUse <= 0)
                return false;

            _gnomeCounterUse--;
            countNumbers = _gnomeCounterNumbers;

            return true;
        }
    }
}
