using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Decks;
using GameFields.DiscardPiles;
using GameFields.Persons;
using GameFields.Persons.Fires;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using Tools.Utils;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

namespace GameFields
{
    public class CardLocationViewRoot
    {
        private readonly ICardWatcher _allCardsWatcher;
        private readonly IDeckView _deckView;
        //private readonly ICardView _handPlayer;
        //private readonly ICardView _handAI;
        //private readonly ICardView _discardPile;
        ////private readonly Tower _towerPlayer; //delete?
        ////private readonly Tower _towerAI; //delete?
        //private readonly ICardView _fireRoot;

        private readonly Dictionary<ViewType, ICardView> _views;
        //private readonly List<Hand> _hands;

        public CardLocationViewRoot(ICardWatcher allCardsWatcher, IDeckView deckView, HandPlayer handPlayer, HandAI handAI,
            DiscardPile discardPile, FireRoot fireRoot, Table tablePlayer, Table tableAI)
        {
            _allCardsWatcher = allCardsWatcher;
            _deckView = deckView;

            _views = new Dictionary<ViewType, ICardView>()
            {
                { ViewType.Deck, deckView },
                { ViewType.DiscardPile, discardPile },
                { ViewType.FireRoot, fireRoot },
                { ViewType.HandAI, handAI },
                { ViewType.HandPlayer, handPlayer },
                { ViewType.TableAI, tableAI },
                { ViewType.TablePlayer, tablePlayer }
            }; 

            //_handPlayer = handPlayer;
            //_handAI = handAI;
            //_discardPile = discardPile;
            ////_towerPlayer = towerPlayer;
            ////_towerAI = towerAI;
            //_fireRoot = fireRoot;

            //_hands = new List<Hand>()
            //{
            //    _handPlayer,
            //    _handAI
            //};
        }

        //public bool TryViewRandomCardsFromHand(out IReadOnlyList<Card> cards, IPersonObject sender, int countCards)
        //{
        //    ICardView hand;

        //    if (sender is IPlayerObject)
        //    {
        //        hand = _handPlayer;
        //    }
        //    else if (sender is IEnemyAIObject) 
        //    {
        //        hand = _handAI;
        //    }
        //    else
        //    {
        //        throw new System.Exception("Неизвестный " + typeof(IPersonObject) + ": " + sender.ToString());
        //    }

        //    return TryView(out cards, countCards, hand);
        //}

        //public bool TryViewRandomCardsFromDeck(out IReadOnlyList<Card> cards, int countCards)
        //{
        //    return TryView(out cards, countCards, _deckView);
        //}
        public IEnumerable<Card> GetAllCards(ViewType viewType)
        {
            return _views[viewType].AllCards;
        }

        public Card ViewRandomCardFromAllCards(IEnumerable<int> exceptions, IEnumerable<ViewType> noContains = null)
        {
            IReadOnlyList<Card> cards = _allCardsWatcher.Cards;

            Func<int, bool> viewTypesContains = (int number) =>
            {
                if (noContains == null)
                    return false;

                foreach (ViewType viewType in noContains)
                {
                    ICardView target = _views[viewType];

                    if (target.Contains(number))
                        return true;
                }

                return false;
            };

            cards = Utils.Shuffle(cards);

            for (int i = 0; i < cards.Count; i++)
            {
                if (exceptions.Contains(cards[i].ViewData.Number) == false && viewTypesContains.Invoke(cards[i].ViewData.Number) == false)
                {
                    return cards[i];
                }
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (exceptions.Contains(cards[i].ViewData.Number) == false)
                {
                    return cards[i];
                }
            }

            return cards[0];
            //while (exceptions.Contains(cards[randomIndex].ViewConfig.Number))
            //{
            //    randomIndex = Random.Range(0, cards.Count);
            //}

            //return cards[randomIndex];

            //List<int> existingIndices = new List<int>();
            //List<Card> result = new List<Card>();

            //for (int i = 0; i < count; i++)
            //{
            //    int randomIndex = Random.Range(0, _cards.Count);

            //    while (existingIndices.Contains(randomIndex))
            //    {
            //        randomIndex = Random.Range(0, _cards.Count);
            //    }

            //    result.Add(_cards[randomIndex]);
            //}

            //return result;
        }

        public bool TryViewDeckLastCards(out IReadOnlyList<Card> cards, int countCards)
        {
            cards = null;

            if (_deckView.IsHasCards(1) == false)
                return false;

            List<Card> currentCards = new List<Card>();

            for (int i = 0; i < countCards; i++)
            {
                if (_deckView.IsHasCards(i + 1))
                    currentCards.Add(_deckView.ViewCardFromEndDeck(i));
            }
            
            cards = currentCards;
            return true;
        }

        public bool TryViewDeckTopCards(out IReadOnlyList<Card> cards, int countCards)
        {
            cards = null;

            if (_deckView.IsHasCards(countCards) == false)
                return false;

            List<Card> currentCards = new List<Card>();

            for (int i = 0; i < countCards; i++)
            {
                currentCards.Add(_deckView.ViewCardFromTopDeck(i));
            }

            cards = currentCards;
            return true;
        }

        // Пытается найти определенное кол-во карт. Если не находит, находит сколько есть. Если ничего нет - false
        public bool TryView(out IReadOnlyList<Card> cards, int countCards, ViewType viewType, IEnumerable<int> exceptions = null)
        {
            exceptions ??= new List<int>();

            ICardView target = _views[viewType];

            cards = null;
            int realCountCards;

            for (realCountCards = countCards; realCountCards > 0; realCountCards--)
            {
                if (target.IsHasCards(realCountCards, exceptions))
                    break;
            }

            if (realCountCards == 0)
                return false;

            cards = target.ViewRandomCards(realCountCards, exceptions);
            return true;
        }

    }
}
