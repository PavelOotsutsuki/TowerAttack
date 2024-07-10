using Cards;

namespace GameFields.Persons.Tables
{
    public class PlayedCardPair
    {
        private int _countTurnsOnTable;

        public PlayedCardPair(CardCharacter cardCharacter, Card card, int countTurnsOnTable)
        {
            CardCharacter = cardCharacter;
            Card = card;
            _countTurnsOnTable = countTurnsOnTable;
        }

        public Card Card { get; private set; }
        public CardCharacter CardCharacter { get; private set; }

        public void DecreaseCardCounter()
        {
            _countTurnsOnTable--;
        }

        public bool TryDiscard(out Card card)
        {
            if (_countTurnsOnTable <= 0)
            {
                card = Card;
                return true;
            }

            card = null;
            return false;
        }

    }
}
