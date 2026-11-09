using GameFields.Signals;

namespace GameFields.Persons.Towers
{
    public class CardAttackZoneEnemyAI : CardAttackZone, IEnemyAIObject
    {
        protected override void AttackProcessingActivate()
        {
            Bus.Fire(new PushStepSignalEnemyAI(this));
        }

        protected override string GetName() => nameof(CardAttackZoneEnemyAI);
    }
}