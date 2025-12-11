using Cards;
using GameFields.Persons.Commons;

namespace GameFields.Persons.Tables
{
    public interface ITableCardSeatable : ICardDropPlace
    {
        public void SeatCard(PersonEffect personEffect);
    }
}