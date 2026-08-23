using System.Collections.Generic;
using Cards.Views;
using Servers;

namespace GameFields.Persons.EffectHandlers.Brothers
{
    public class BrothersEffectHandler : EffectHandler
    {
        private const int StartCount = 0;

        private readonly RechangeFeatureRuleController _ruleController;
        private readonly IEnumerable<ICardFeatureRechangablePlace> _rechangables;

        private int _extraCount;

        public BrothersEffectHandler(RechangeFeatureRuleController ruleController, IEnumerable<ICardFeatureRechangablePlace> rechangables,
            FightProcessDBManager fightProcessDBManager, bool isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        {
            _ruleController = ruleController;
            _rechangables = rechangables;

            _extraCount = 0;
        }

        public int ExtraCount => _extraCount;
        //public int UpgradeStepCount => _upgradeStepCount;

        public void Upgrade(int increaseValue)
        {
            _extraCount += increaseValue;

            List<TagValuePair> tagValuePairs = new List<TagValuePair>
            {
                new TagValuePair("CARDS", _extraCount + StartCount),
                new TagValuePair("TIME", _extraCount + StartCount),
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

            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, increaseValue.ToString(), "UPGRADE", GetType().Name);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, _extraCount.ToString(), "ALL _extraCount", GetType().Name);
        }
    }
}