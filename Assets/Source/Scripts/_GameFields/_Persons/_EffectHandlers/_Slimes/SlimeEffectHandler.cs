using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers.Slimes
{
    public class SlimeEffectHandler : ITurnSkipper
    {
        private readonly ISlimeEffectWorker _slimeEffectWorker;
        //private readonly List<Card> _turnCardsFromDeck;
        private readonly IDrawnCardClearable _turnDrawnCards;

        private int _countTurns;

        public SlimeEffectHandler(ISlimeEffectWorker slimeEffectWorker, IDrawnCardClearable turnDrawnCards)
        {
            _slimeEffectWorker = slimeEffectWorker;
            //_turnCardsFromDeck = new List<Card>();
            _turnDrawnCards = turnDrawnCards;

            _countTurns = 0;
        }

        public bool CanSkip => _turnDrawnCards.IsVoid() && _countTurns > 0;
        //public IEnumerable<Card> TurnCardsFromDeck => _turnCardsFromDeck;

        public void Activate(int countTurns)
        {
            _countTurns = countTurns
                + 1; // +1 чтобы нейтрализовать эффект "в начале хода"

            _slimeEffectWorker.Activate();
        }

        //void ITurnDrawCardWatcher.SetCard(Card card)
        //{
        //    _turnCardsFromDeck.Add(card);
        //}

        public void OnStartTurn()
        {
            if (_countTurns > 0)
            {
                _countTurns--;

                if (_countTurns == 0)
                {
                    _slimeEffectWorker.Deactivate();
                }
            }

            _turnDrawnCards.Clear();
        }
    }
}