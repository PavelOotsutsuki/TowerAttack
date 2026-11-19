using Servers;

namespace GameFields.Persons.SelectMenues
{
    public class AttackedSelectNumbersList : SelectNumbersList
    {
        public AttackedSelectNumbersList(FightProcessDBManager fightProcessDBManager, bool? isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        { }

        protected override SelectNumbersListType SetSelectNumbersListType() => SelectNumbersListType.Attack;
    }
}