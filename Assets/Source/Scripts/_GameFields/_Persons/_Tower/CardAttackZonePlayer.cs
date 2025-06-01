using Cards;
using GameFields.Persons.Commons;
using GameFields.Signals;

namespace GameFields.Persons.Towers
{
    public class CardAttackZonePlayer : CardAttackZone, IPlayerObject, IPlayerAttackable
    {
        protected override void AttackProcessingActivate()
        {
            Bus.Fire(new PushStepSignalPlayer(this));
        }
    }
}