using Servers;

namespace GameFields.Persons.SelectMenues
{
    public class CursedSelectNumbersList : SelectNumbersList
    {
        public CursedSelectNumbersList(FightProcessDBManager fightProcessDBManager, bool? isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        { }

        protected override SelectNumbersListType SetSelectNumbersListType() => SelectNumbersListType.Curse;
    }
}