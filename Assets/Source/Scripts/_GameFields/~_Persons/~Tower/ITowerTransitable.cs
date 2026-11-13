using Cards;

namespace GameFields.Persons.Towers
{
    public interface ITowerTransitable : ITowerCardSeatable
    {
        bool TryTakeAwayCard(out Card card);
    }
}