using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers.Slimes
{
    public class SlimeEffectHandler : ITurnSkipper, ILengthyEffectHandler
    {
        private readonly ISlimeEffectWorker _slimeEffectWorker;
        //private readonly List<Card> _turnCardsFromDeck;
        private readonly IDrawnCardClearable _turnDrawnCards;

        private readonly List<Card> _effectedCards;

        public SlimeEffectHandler(ISlimeEffectWorker slimeEffectWorker, IDrawnCardClearable turnDrawnCards)
        {
            _slimeEffectWorker = slimeEffectWorker;
            //_turnCardsFromDeck = new List<Card>();
            _turnDrawnCards = turnDrawnCards;
            _effectedCards = new List<Card>();
        }

        public bool CanSkip => _turnDrawnCards.IsVoid() && _effectedCards.Count > 0;
        //public IEnumerable<Card> TurnCardsFromDeck => _turnCardsFromDeck;

        public void Activate(Card card)
        {
            if (_effectedCards.Contains(card))
                return;

            _effectedCards.Add(card);
            _slimeEffectWorker.Activate();
        }

        public void EndEffect(Card card)
        {
            if (_effectedCards.Contains(card) == false)
                return;

            _effectedCards.Remove(card);

            if (_effectedCards.Count == 0)
                _slimeEffectWorker.Deactivate();
        }

        //void ITurnDrawCardWatcher.SetCard(Card card)
        //{
        //    _turnCardsFromDeck.Add(card);
        //}

        public void OnStartTurn()
        {
            //if (_countTurns > 0)
            //{
            //    _countTurns--;

            //    if (_countTurns == 0)
            //    {
            //        _slimeEffectWorker.Deactivate();
            //    }
            //}

            _turnDrawnCards.Clear();
        }
    }
}