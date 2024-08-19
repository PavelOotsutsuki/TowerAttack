using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using UnityEngine;

namespace GameFields.Persons
{
    public class PlayCardManager
    {
        //private IUnbindCardManager _unbindCardManager;
        //private Table _table;
        //private PlayedCards _playedCards;

        //public PlayCardManager(IUnbindCardManager unbindCardManager, Table table)
        //{
        //    _playedCards = new PlayedCards();
        //    _unbindCardManager = unbindCardManager;
        //    _table = table;
        //}

        //public void Play()
        //{

        //}
    }

//using Cards;

//namespace GameFields.Persons.Tables
//    {
//        public class PlayedCardPair
//        {
//            private int _countTurnsOnTable;

//            public PlayedCardPair(CardCharacter cardCharacter, Card card, int countTurnsOnTable)
//            {
//                CardCharacter = cardCharacter;
//                Card = card;
//                _countTurnsOnTable = countTurnsOnTable;
//            }

//            public Card Card { get; private set; }
//            public CardCharacter CardCharacter { get; private set; }

//            public void DecreaseCardCounter()
//            {
//                _countTurnsOnTable--;
//            }

//            public bool TryDiscard(out Card card)
//            {
//                if (_countTurnsOnTable <= 0)
//                {
//                    card = Card;
//                    return true;
//                }

//                card = null;
//                return false;
//            }

//        }
//    }
//using System.Collections.Generic;
//using Cards;

//namespace GameFields.Persons.Tables
//    {
//        public class PlayedCards
//        {
//            private List<PlayedCardPair> _playedCardPairs;

//            public PlayedCards()
//            {
//                _playedCardPairs = new List<PlayedCardPair>();
//            }

//            //public Card GetCard(CardCharacter cardCharacter)
//            //{
//            //    foreach (PlayedCardPair playedCardPair in _playedCardPairs)
//            //    {
//            //        if (playedCardPair.CardCharacter == cardCharacter)
//            //        {
//            //            return playedCardPair.Card;
//            //        }
//            //    }

//            //    return null;
//            //}

//            public List<Card> GetDiscardCards()
//            {
//                List<Card> discardCards = new List<Card>();

//                foreach (PlayedCardPair playedCardPair in _playedCardPairs)
//                {
//                    playedCardPair.DecreaseCardCounter();

//                    if (playedCardPair.TryDiscard(out Card card))
//                    {
//                        discardCards.Add(card);
//                        _playedCardPairs.Remove(playedCardPair);
//                    }
//                }

//                return discardCards;
//            }

//            public void Add(CardCharacter cardCharacter, Card card, int countTurnsOnTable)
//            {
//                _playedCardPairs.Add(new PlayedCardPair(cardCharacter, card, countTurnsOnTable));
//            }
//        }
//    }

}
