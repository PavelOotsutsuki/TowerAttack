using System.Collections.Generic;
using Cards.Views;
using Servers;

namespace GameFields.Persons.EffectHandlers
{
    public class GnomeEffectHandler : EffectHandler
    {
        private readonly RechangeFeatureRuleController _ruleController;
        private readonly IEnumerable<ICardFeatureRechangablePlace> _rechangables;

        private readonly int _upgradeStepCount;
        private readonly int _startCount; 

        private int _gnomeCounterNumbers;
        private int _gnomeCounterUse;

        public GnomeEffectHandler(RechangeFeatureRuleController ruleController, IEnumerable<ICardFeatureRechangablePlace> rechangables,
            FightProcessDBManager fightProcessDBManager, bool isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        {
            _ruleController = ruleController;
            _rechangables = rechangables;

            _upgradeStepCount = 2;

            _startCount = 3;
            _gnomeCounterNumbers = 0;
            _gnomeCounterUse = 1;
        }

        public int GnomeCounterNumbers => _startCount + _gnomeCounterNumbers;
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

            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, _upgradeStepCount.ToString(), "UPGRADE", GetType().Name);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, _gnomeCounterNumbers.ToString(), "ALL _gnomeCounterNumbers", GetType().Name);
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
            countNumbers = GnomeCounterNumbers;

            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, null, "ACTIVATE", GetType().Name);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, _gnomeCounterUse.ToString(), "ALL _gnomeCounterUse", GetType().Name);

            return true;
        }
    }
}