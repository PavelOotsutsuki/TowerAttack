using GameFields.DiscardPiles;
using GameFields.Persons.Commons;
using GameFields.Persons.Towers;
using Zenject;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackResultHandlerPlayer : AttackResultHandler, IPlayerObject
    {
        public AttackResultHandlerPlayer(DiscardPile discardPile, SignalBus bus, IBoomTower tower,
            IAttackCardKeeper attackCardKeeper, AttackResultHandlerData data) :
            base(discardPile, bus, tower, attackCardKeeper, data)
        { }
    }
}