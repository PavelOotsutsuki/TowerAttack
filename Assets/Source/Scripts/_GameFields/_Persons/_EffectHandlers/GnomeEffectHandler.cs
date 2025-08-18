using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers
{
    public class GnomeEffectHandler
    {
        private readonly RechangeFeatureRuleController _ruleController;
        private readonly IEnumerable<ICardFeatureRechangablePlace> _rechangables;

        private readonly int _upgradeStepCount;

        private int _gnomeCounterNumbers;
        private int _gnomeCounterUse;

        public GnomeEffectHandler(RechangeFeatureRuleController ruleController, IEnumerable<ICardFeatureRechangablePlace> rechangables)
        {
            _ruleController = ruleController;
            _rechangables = rechangables;

            _upgradeStepCount = 2;

            _gnomeCounterNumbers = 3;
            _gnomeCounterUse = 1;
        }

        public int GnomeCounterNumbers => _gnomeCounterNumbers;
        public int UpgradeStepCount => _upgradeStepCount;

        public void Upgrade()
        {
            _gnomeCounterNumbers += _upgradeStepCount;

            List<TagValuePair> tagValuePairs = new List<TagValuePair>
            {
                new TagValuePair("CARDS", _gnomeCounterNumbers)
            };

            _ruleController.Add(CardCapability.GnomeForging, tagValuePairs);

            if (_rechangables != null)
            {
                foreach (ICardFeatureRechangablePlace rechangable in _rechangables)
                {
                    if (rechangable.GetRechangableCards() != null)
                    {
                        foreach (IFeatureRechanger rechanger in rechangable.GetRechangableCards())
                        {
                            _ruleController.TryRenameFeature(rechanger);
                        }
                    }
                }
            }
        }

        public bool CanActivate()
        {
            if (_gnomeCounterUse <= 0)
                return false;

            return true;
        }

        public bool TryActivate(out int countNumbers)
        {
            countNumbers = 0;

            if (CanActivate() == false)
                return false;

            _gnomeCounterUse--;
            countNumbers = _gnomeCounterNumbers;

            return true;
        }
    }
}
