using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers.Brothers
{
    public class BrothersEffectHandler
    {
        private readonly RechangeFeatureRuleController _ruleController;
        private readonly IEnumerable<ICardFeatureRechangablePlace> _rechangables;

        private int _extraCount;

        public BrothersEffectHandler(RechangeFeatureRuleController ruleController, IEnumerable<ICardFeatureRechangablePlace> rechangables)
        {
            _ruleController = ruleController;
            _rechangables = rechangables;

            _extraCount = 0;
        }

        public int ExtraCount => _extraCount;
        //public int UpgradeStepCount => _upgradeStepCount;

        public void Upgrade(int startValue, int increaseValue)
        {
            _extraCount += increaseValue;

            List<TagValuePair> tagValuePairs = new List<TagValuePair>
            {
                new TagValuePair("CARDS", startValue + _extraCount)
            };

            _ruleController.Add(CardCapability.BrothersBonds, tagValuePairs);

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
    }
}
