using GameFields.Persons;

namespace GameFields
{
    public interface IStartTowerCardSelectionListener: IDrawCardManager
    {
        public bool IsTowerFilled { get; }

        public void StartTowerCardSelection(int drawCardsCount);
    }
}
