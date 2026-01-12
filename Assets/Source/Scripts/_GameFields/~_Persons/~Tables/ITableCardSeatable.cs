using Cards;
using Cards.DependencyInterlayers;
using GameFields.Persons;

namespace GameFields.Persons.Tables
{
    public interface ITableCardSeatable : ICardDropPlace
    {
        public void SeatCard(PersonEffect personEffect);
    }
}