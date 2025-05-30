using GameFields.Persons.Commons;
using GameFields.Signals;

namespace GameFields.Persons.Towers
{
    public class CardAttackZoneEnemyAI : CardAttackZone, IEnemyAIObject
    {
        protected override void AttackProcessingActivate()
        {
            Bus.Fire(new AttackSignalEnemyAI(this));
        }
    }
}