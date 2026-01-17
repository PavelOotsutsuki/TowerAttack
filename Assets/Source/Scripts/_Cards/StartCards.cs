using System.Collections.Generic;
using Cards.Views;

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

            for (int i = 1; i <= 50; i++)
            {
                CardName cardName = (CardName)i;
                _startCardNames.Add(cardName);
            }


            //for (int i = 1; i < 100; i++)
            //{
            //    CardName cardName = (CardName)28;
            //    _startCardNames.Add(cardName);
            //}

            //for (int i = 1; i < 50; i++)
            //{
            //    CardName cardName = (CardName)50;
            //    _startCardNames.Add(cardName);
            //}

            //for (int i = 1; i < 50; i++)
            //{
            //    CardName cardName = (CardName)45;
            //    _startCardNames.Add(cardName);
            //}

            //for (int i = 1; i < 10; i++)
            //{
            //    CardName cardName = (CardName)44;
            //    _startCardNames.Add(cardName);
            //}

            //for (int i = 1; i < 10; i++)
            //{
            //    CardName cardName = (CardName)23;
            //    _startCardNames.Add(cardName);
            //}

            //for (int i = 1; i < 10; i++)
            //{
            //    CardName cardName = (CardName)36;
            //    _startCardNames.Add(cardName);
            //}

            //for (int i = 1; i < 10; i++)
            //{
            //    CardName cardName = (CardName)18;
            //    _startCardNames.Add(cardName);
            //}

            //for (int i = 1; i < 20; i++)
            //{
            //    CardName cardName = (CardName)40;
            //    _startCardNames.Add(cardName);
            //}
        }
    }
}