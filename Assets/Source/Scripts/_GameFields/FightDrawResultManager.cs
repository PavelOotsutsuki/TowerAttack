using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.CardTransits;
using GameFields.Decks;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.Hands;

namespace GameFields
{
    public class FightDrawResultManager
    {
        private ICardView _handAI;
        private ICardView _handPlayer;
        private ICardView _deck;
        private PersonEffectsHandlerRoot _personEffectsHandlerRoot;

        public bool IsDrawResult
        {
            get
            {
                // Ищем все карты которые можем разыграть 
                IEnumerable<Card> handAICards = _handAI.AllCards.Where(c => c.CardName != CardName.CursedMark);
                IEnumerable<Card> handPlayerCards = _handPlayer.AllCards.Where(c => c.CardName != CardName.CursedMark);
                IEnumerable<Card> deckCards = _deck.AllCards;

                bool hasCards;

                if ((handAICards.Count() + handPlayerCards.Count() + deckCards.Count()) == 0)
                {
                    hasCards = false;
                }
                else
                {
                    hasCards = true;
                }

                return hasCards == false && _personEffectsHandlerRoot.IsActiveLengthyEffectCapableWinAlone() == false;
            }
        }

        public FightDrawResultManager(HandAI handAI, HandPlayer handPlayer, Deck deck, PersonEffectsHandlerRoot personEffectsHandlerRoot)
        {
            _handAI = handAI;
            _handPlayer = handPlayer;
            _deck = deck;
            _personEffectsHandlerRoot = personEffectsHandlerRoot;
        }
    }
}