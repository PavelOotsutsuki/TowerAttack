using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.CardTransits;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Signals;
using StateMachine;
using UnityEngine;
using Zenject;

namespace GameFields.Persons
{
    public abstract class Person : IStateMachineState, IDrawCardManager, ITowerTransitCheck, ITowerTransitTrySet, /*IHandTransitGetLast,*/ IHandTransitSet, IHandTransitTryGet, IHandTransitGetAll
    {
        private readonly ITurnStep _turnProcess;
        private readonly CardPlayingZone _playingZone;
        private readonly DrawCardRoot _drawCardRoot;
        private readonly StartTurnDraw _startTurnDraw;
        private readonly Tower _tower;
        private readonly Queue<ITurnStep> _turnSteps;
        private readonly Discover _discover;
        private readonly Hand _hand;

        private ITurnStep _currentStep;

        protected readonly SignalBus Bus;

        protected Person(CardPlayingZone playingZone, DrawCardRoot drawCardRoot, Tower tower,
            StartTurnDraw startTurnDraw, ITurnStep turnProcess, Discover discover, SignalBus bus, Hand hand)
        {
            _hand = hand;
            Bus = bus;
            _playingZone = playingZone;
            _tower = tower;
            _drawCardRoot = drawCardRoot;
            _startTurnDraw = startTurnDraw;
            _turnProcess = turnProcess;
            _discover = discover;

            _turnSteps = new Queue<ITurnStep>();

            _discover.Deactivate();
        }

        public bool IsComplete { get; private set; }

        public void StartStep()
        {
            IsComplete = false;
            
            InitTurnSteps();
            OnStartStep();

            _currentStep = _turnSteps.Dequeue();

            ProcessingTurn().ToUniTask();
        }

        public void FinishTurn()
        {
            IReadOnlyList<Card> discardedCards = _playingZone.UpdateCards();

            if (discardedCards.Count > 0)
                Bus.Fire(new DiscardCardsSignal(discardedCards));
        }

        public void DiscoverCards(List<Card> cards, string activateMessage, Action<Card> callback)
        {
            if (cards is null)
            {
                return;
            }

            if (cards.Count > _discover.MaxSeats)
            {
                return;
                //throw new Exception("So many cards for discover: " + cards.Count + "/" + _discover.MaxSeats);
            }

            _discover.Activate(cards, activateMessage, callback);
        }

        protected abstract void OnStartStep();

        protected void EnqueueStep(ITurnStep turnStep) => _turnSteps.Enqueue(turnStep);

        private void InitTurnSteps()
        {
            _turnSteps.Clear();
            
            EnqueueStep(_startTurnDraw);
            EnqueueStep(_turnProcess);
        }

        private IEnumerator ProcessingTurn()
        {
            while (IsComplete == false)
            {
                _currentStep.StartStep();
                yield return new WaitUntil(() => _currentStep.IsComplete);

                NextStep();
            }
        }

        private void NextStep()
        {
            if (_turnSteps.Count > 0)
            {
                _currentStep = _turnSteps.Dequeue();
            }
            else
            {
                IsComplete = true;
            }
        }

        List<Card> IDrawCardManager.DrawCards(int countCards, Action callback = null)
        { 
            return _drawCardRoot.DrawCards(countCards, callback);
        }

        bool ITowerTransitCheck.IsFill => _tower.IsTowerFill;

        bool ITowerTransitTrySet.TrySet(Card card)
        {
            return _tower.TrySeatCard(card);
        }

        bool IHandTransitTryGet.TryGet(Card card)
        {
            return _hand.TryGetCard(card);
        }

        void IHandTransitSet.Set(Card card)
        {
            _hand.AddCard(card);
        }

        //Card IHandTransitGetLast.Get()
        //{
        //    return _hand.GetLastCard();
        //}

        List<Card> IHandTransitGetAll.Get()
        {
            List<Card> cards;

            if (_hand.TryGetAllCards(out cards) == false)
            {
                cards = null;
            }

            return cards;
        }
    }
}