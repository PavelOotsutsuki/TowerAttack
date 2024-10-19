using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.AttackMenues;
using GameFields.Persons.CardTransits;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using UnityEngine;
using Zenject;

namespace GameFields.Persons
{
    public abstract class Person : ITurnStep, IDrawCardManager, ITowerTransitCheck, ITowerTransitSet, /*IHandTransitGetLast,*/ IHandTransitSet, IHandTransitTryGet, IHandTransitGetAll
    {
        private readonly IPersonStep _turnProcess;
        private readonly CardPlayingZone _playingZone;
        private readonly DrawCardRoot _drawCardRoot;
        private readonly StartTurnDraw _startTurnDraw;
        private readonly Tower _tower;
        private readonly Queue<IPersonStep> _personSteps;
        private readonly Discover _discover;
        private readonly Hand _hand;
        private readonly AttackMenu _attackMenu;

        private IPersonStep _currentStep;

        protected readonly SignalBus Bus;

        protected Person(CardPlayingZone playingZone, DrawCardRoot drawCardRoot, Tower tower,
            StartTurnDraw startTurnDraw, IPersonStep turnProcess, Discover discover, SignalBus bus,
            Hand hand, AttackMenu attackMenu)
        {
            _hand = hand;
            Bus = bus;
            _playingZone = playingZone;
            _tower = tower;
            _drawCardRoot = drawCardRoot;
            _startTurnDraw = startTurnDraw;
            _turnProcess = turnProcess;
            _discover = discover;
            _attackMenu = attackMenu;

            _personSteps = new Queue<IPersonStep>();

            _discover.Deactivate();
        }

        public bool IsComplete { get; private set; }

        public void StartStep()
        {
            IsComplete = false;

            _personSteps.Clear();

            OnStartStep();
            InitSteps();

            _currentStep = _personSteps.Dequeue();

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
            }

            _discover.Activate(cards, activateMessage, callback);
        }

        public void AttackActivate()
        {
            _attackMenu.Activate();
        }

        public void AttackDeactivate()
        {
            _attackMenu.Deactivate();
        }

        protected abstract void OnStartStep();

        protected void EnqueueStep(IPersonStep turnStep) => _personSteps.Enqueue(turnStep);

        private void InitSteps()
        {
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
            if (_personSteps.Count > 0)
            {
                _currentStep = _personSteps.Dequeue();
            }
            else
            {
                IsComplete = true;
            }
        }

        List<Card> IDrawCardManager.DrawCards(int countCards, Action callback)
        { 
            return _drawCardRoot.DrawCards(countCards, callback);
        }

        bool ITowerTransitCheck.IsFill => _tower.HasFreeSeat == false;

        void ITowerTransitSet.Set(Card card)
        {
            _tower.SeatCard(card);
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