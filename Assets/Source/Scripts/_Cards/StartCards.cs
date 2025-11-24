using System.Collections.Generic;

namespace Cards
{
    public class StartCards
    {
        private readonly List<CardName> _startCardNames;

        public StartCards(StartCardsType startCardsType)
        {
            _startCardNames = new List<CardName>();

            switch (startCardsType)
            {
                case StartCardsType.DefaultFiftyCards:
                    CreateDefaultFiftyCards();
                    break;
                default:
                    throw new System.Exception($"Неизвестный StartCardsType: {startCardsType}");
            }
        }

        public IReadOnlyList<CardName> StartCardNames => _startCardNames;

        private void CreateDefaultFiftyCards()
        {
            for (int i = 1; i < 46; i++)
            {
                CardName cardName = (CardName)i;
                _startCardNames.Add(cardName);
            }

            _startCardNames.Add((CardName)48);
        }
    }
}