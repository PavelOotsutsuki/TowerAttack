using GameFields.DiscardPiles;
using GameFields.Persons;
using GameFields.Persons.Towers;
using Zenject;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackResultHandlerPlayer : AttackResultHandler, IEnemyAIObject
    {
        public AttackResultHandlerPlayer(DiscardPile discardPile, LoseActions loseActions,
            IAttackCardKeeper attackCardKeeper, AttackResultHandlerData data) :
            base(discardPile, loseActions, attackCardKeeper, data)
        { }
    }
}