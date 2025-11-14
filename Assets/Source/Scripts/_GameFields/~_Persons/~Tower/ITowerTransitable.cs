using Cards;

namespace GameFields.Persons.Towers
{
    public interface ITowerTransitable : ICardSeatable
    {
        bool TryTakeAwayCard(out Card card);
    }
}