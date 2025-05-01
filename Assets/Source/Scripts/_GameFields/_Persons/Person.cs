using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.SelectMenues.Attacks;
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
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Persons.SelectMenues.Choices;
using GameFields.InformationLabels;

namespace GameFields.Persons
{
    public abstract class Person : ITurnStep, IDrawCardManager, ITowerTransitCheck, ITowerTransitSet, /*IHandTransitGetLast,*/ IHandTransitSet, IHandTransitTryGet, IHandTransitGetAll
    {
        private readonly CardPlayingZone _playingZone;
        private readonly DrawCardRoot _drawCardRoot;
        private readonly Tower _tower;
        private readonly Stack<PersonStep> _personSteps;
        private readonly Discover _discover;
        private readonly Hand _hand;
        private readonly ISelectMenuActivator _attackMenu;
        private readonly ISelectMenuActivator _choiceMenu;

        //private readonly PersonStep _lastStep;

        //protected readonly PersonStep TurnProcess;
        protected readonly StartTurnDraw StartTurnDraw;

        protected readonly SignalBus Bus;
        protected readonly InteractionActivator InteractionActivator;

        private PersonStep _currentStep;

        protected Person(CardPlayingZone playingZone, DrawCardRoot drawCardRoot, Tower tower,
            StartTurnDraw startTurnDraw, Discover discover, SignalBus bus, /*PersonStep lastStep,*/
            Hand hand, ISelectMenuActivator attackMenu, InteractionActivator gameFieldObjectsActivator,
            ISelectMenuActivator selectMenu)
        {
            _hand = hand;
            Bus = bus;
            _playingZone = playingZone;
            _tower = tower;
            _drawCardRoot = drawCardRoot;
            StartTurnDraw = startTurnDraw;
            //TurnProcess = turnProcess;
            _discover = discover;
            _attackMenu = attackMenu;
            _choiceMenu = selectMenu;
            //_lastStep = lastStep;
            InteractionActivator = gameFieldObjectsActivator;

            _personSteps = new Stack<PersonStep>();
            //Bus.Subscribe<StartEffectSignal>(SetCardEffectProcess);
        }

        ~Person()
        {
            //Bus.Unsubscribe<StartEffectSignal>(SetCardEffectProcess);
        }

        public bool IsComplete { get; private set; }

        public void StartStep()
        {
            IsComplete = false;

            _personSteps.Clear();

            _hand.OnStartTurn();
            OnStartStep();
            InitSteps();

            _currentStep = _personSteps.Pop();

            ProcessingTurn().ToUniTask();
        }

        public void FinishTurn()
        {
            _hand.OnFinishTurn();

            IReadOnlyList<Card> discardedCards = _playingZone.UpdateCards();

            if (discardedCards.Count > 0)
                Bus.Fire(new DiscardCardsSignal(discardedCards));
        }

        public void DiscoverCards(IReadOnlyList<Card> cards, string activateMessage, Action<Card> callback)
        {
            if (cards is null)
            {
                return;
            }

            if (cards.Count > _discover.MaxSeats)
            {
                return;
            }

            DiscoverActivateData discoverActivateData = new DiscoverActivateData(cards, activateMessage, callback);

            _discover.Activate(discoverActivateData);
        }

        public void AttackActivate()
        {
            SelectMenuActivateData data = new SelectMenuActivateData(1);

            _attackMenu.Activate(data);
        }

        //public void ChoiceActivate(string message, int countNumbers)
        //{
        //    ChoiceResultHandler choiceResultHandler = new ChoiceResultHandler(_informationLabel, message);
        //    ChoiceMenuActivateData data = new ChoiceMenuActivateData(countNumbers, choiceResultHandler);

        //    _choiceMenu.Activate(data);
        //}

        public void ChoiceActivate(int countNumbers, Action callback)
        {
            SelectMenuActivateData data = new SelectMenuActivateData(countNumbers);

            _choiceMenu.Activate(data);

            WaitingToInvoke(_choiceMenu, callback).ToUniTask(); ;
        }

        private IEnumerator WaitingToInvoke(ICompletable completable, Action callback)
        {
            yield return new WaitUntil(() => completable.IsComplete);

            callback.Invoke();
        }

        //public void AttackDeactivate()
        //{
        //    _attackMenu.Deactivate();
        //}

        protected abstract void OnStartStep();

        protected void PushStep(PersonStep turnStep) => _personSteps.Push(turnStep);

        protected abstract void InitSteps();
        //{
        //    EnqueueStep(_startTurnDraw);
        //    EnqueueStep(_turnProcess);
        //    EnqueueStep(_cardEffectProcessing);
        //}

        private IEnumerator ProcessingTurn()
        {
            while (IsComplete == false)
            {
                _currentStep.StartStep();
                //Debug.Log(_currentStep.ToString() + ": " + this.ToString());
                yield return new WaitUntil(() => _currentStep.IsComplete);

                NextStep();
            }
        }

        private void NextStep()
        {
            //if (_hand.CountCards == 0)
            //{
            //    _personSteps.Clear();
            //    _currentStep = _lastStep;
            //    return;
            //}

            if (_personSteps.Count > 0 )
            {
                _currentStep = _personSteps.Pop();
            }
            else
            {
                IsComplete = true;
            }
        }

        public abstract void StartEffect(Effect effect);

        //public void StartEffect(Effect effect)
        //{
        //    CardEffectProcessing.SetEffect(effect);

        //    if (TurnProcess is TurnProcessing)
        //    {
        //        ((TurnProcessing)TurnProcess).Completed();
        //    }
        //}

        //private void SetCardEffectProcess(StartEffectSignal signal)
        //{
        //    CardEffectProcessing.SetEffect(signal.Effect);

        //    if (TurnProcess is TurnProcessing)
        //    {
        //        ((TurnProcessing)TurnProcess).Completed();
        //    }
        //}

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

        public void ActivateSlimeEffect(int countTurns)
        {
            _hand.ActivateSlimeEffect(countTurns);
        }

        public void ActivateFireDraw(int countTurns)
        {
            StartTurnDraw.SetFireMode(countTurns);
        }

        //public void DeactivateSlimeEffect()
        //{

        //}
    }
}