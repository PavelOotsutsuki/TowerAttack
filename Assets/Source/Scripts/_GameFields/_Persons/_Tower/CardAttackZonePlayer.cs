using GameFields.Signals;

namespace GameFields.Persons.Towers
{
    public class CardAttackZonePlayer : CardAttackZone, IPlayerObject
    {
        protected override void AttackProcessingActivate()
        {
            Bus.Fire(new AttackSignalPlayer(this));
        }
    }
}