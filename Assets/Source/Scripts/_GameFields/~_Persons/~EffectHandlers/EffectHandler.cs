using Servers;

namespace GameFields.Persons.EffectHandlers
{
    public abstract class EffectHandler
    {
        protected readonly FightProcessDBManager FightProcessDBManager;
        protected readonly bool IsPlayersObject;

        public EffectHandler(FightProcessDBManager fightProcessDBManager, bool isPlayersObject)
        {
            FightProcessDBManager = fightProcessDBManager;
            IsPlayersObject = isPlayersObject;
        }
    }
}