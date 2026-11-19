using GameFields.DiscardPiles;
using Servers;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackResultHandlerPlayer : AttackResultHandler//, IEnemyAIObject
    {
        public AttackResultHandlerPlayer(DiscardPile discardPile, LoseActions loseActions,
            IAttackCardKeeper attackCardKeeper, AttackResultHandlerData data, FightProcessDBManager fightProcessDBManager) :
            base(discardPile, loseActions, attackCardKeeper, data, fightProcessDBManager)
        { }

        protected override bool? IsPlayersAction => true;
        protected override string GetName() => nameof(AttackResultHandlerPlayer);
    }
}