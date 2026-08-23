using Servers;

namespace GameFields.Persons.SelectMenues
{
    public class ChoicedSelectNumbersList : SelectNumbersList
    {
        public ChoicedSelectNumbersList(FightProcessDBManager fightProcessDBManager, bool? isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        { }

        protected override SelectNumbersListType SetSelectNumbersListType() => SelectNumbersListType.Choice;
    }
}