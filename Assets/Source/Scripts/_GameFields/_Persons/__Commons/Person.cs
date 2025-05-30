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
using GameFields.Effects;
using GameFields.Persons.EffectHandlers;

namespace GameFields.Persons.Commons
{
    public abstract class Person : ITurnStep, IDrawCardManager
    {
        private readonly CardPlayingZone _playingZone;
        private readonly DrawCardRoot _drawCardRoot;
        private readonly Tower _tower;
        private readonly Stack<PersonStep> _personSteps;
        private readonly Discover _discover;
        private readonly Hand _hand;
        private readonly ISelectMenuActivator _attackMenu;
        private readonly ISelectMenuActivator _choiceMenu;
        private readonly ISelectMenuActivator _choiceMenuImitation;

        //private readonly PersonStep _lastStep;

        //protected readonly PersonStep TurnProcess;
        protected readonly StartTurnDraw StartTurnDraw;

        protected readonly SignalBus Bus;
        protected readonly InteractionActivator InteractionActivator;

        private readonly PersonEffectsHandler _personEffectsHandler;

        private PersonStep _currentStep;

        protected Person(CardPlayingZone playingZone, DrawCardRoot drawCardRoot, Tower tower,
            StartTurnDraw startTurnDraw, Discover discover, SignalBus bus, /*PersonStep lastStep,*/
            Hand hand, ISelectMenuActivator attackMenu, InteractionActivator gameFieldObjectsActivator,
            ISelectMenuActivator choiceMenu, ISelectMenuActivator choiceMenuImitation, PersonEffectsHandler personEffectsHandler)
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
            _choiceMenu = choiceMenu;
            _choiceMenuImitation = choiceMenuImitation;
            //_lastStep = lastStep;
            InteractionActivator = gameFieldObjectsActivator;

            _personEffectsHandler = personEffectsHandler;

            _personSteps = new Stack<PersonStep>();

            LastEffect = ScriptableObject.CreateInstance<CardEffectConfig>();
            //Bus.Subscribe<StartEffectSignal>(SetCardEffectProcess);
        }

        ~Person()
        {
            //Bus.Unsubscribe<StartEffectSignal>(SetCardEffectProcess);
        }

        public CardEffectConfig LastEffect { get; private set; }
        public bool IsComplete { get; private set; }
        public PersonEffectsHandler PersonEffectsHandler => _personEffectsHandler;

        public void StartStep()
        {
            IsComplete = false;

            _personSteps.Clear();

            //_hand.OnStartTurn();
            OnStartStep();
            InitSteps();

            _currentStep = _personSteps.Pop();

            ProcessingTurn().ToUniTask();
        }

        public void FinishTurn()
        {
            //_hand.OnFinishTurn();
            _personEffectsHandler.OnEndTurn();

            IReadOnlyList<Card> discardedCards = _playingZone.UpdateCards();

            if (discardedCards.Count > 0)
                Bus.Fire(new DiscardCardsSignal(discardedCards));
        }

        public void DiscoverCards(IReadOnlyList<Card> cards, string activateMessage, DiscoverResult discoverResult)
        {
            if (cards is null)
            {
                return;
            }

            if (cards.Count > _discover.MaxSeats)
            {
                return;
            }

            DiscoverActivateData discoverActivateData = new DiscoverActivateData(cards, activateMessage, discoverResult);

            _discover.Activate(discoverActivateData);
        }

        public void AttackActivate(int countNumbers = 1, Action callback = null, RestrictionType? restrictionType = null)
        {
            ActivateSelectMenu(_attackMenu, countNumbers, callback, restrictionType);
        }

        public void ChoiceActivate(int countNumbers, Action callback = null, RestrictionType? restrictionType = null)
        {
            ActivateSelectMenu(_choiceMenu, countNumbers, callback, restrictionType);
        }

        public void ChoiceImitationActivate(int countNumbers, Action callback = null, RestrictionType? restrictionType = null)
        {
            ActivateSelectMenu(_choiceMenuImitation, countNumbers, callback, restrictionType);
        }

        private void ActivateSelectMenu(ISelectMenuActivator selectMenu, int countNumbers, Action callback, RestrictionType? restrictionType)
        {
            SelectMenuActivateData data = new SelectMenuActivateData(countNumbers, restrictionType);

            selectMenu.Activate(data);

            if (callback != null)
                WaitingToInvoke(selectMenu, callback).ToUniTask();
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

        public virtual void StartEffect(Effect effect, CardEffectConfig effectData)
        {
            LastEffect = effectData;
        }

        public abstract void StartAction(ICompletable completable);

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

        Card IDrawCardManager.DrawCard(Card card, Action callback)
        {
            return _drawCardRoot.DrawCard(card, callback);
        }

        //bool ITowerTransitCheck.IsFill => _tower.HasFreeSeat == false;

        //void ITowerTransitSet.Set(Card card)
        //{
        //    _tower.SeatCard(card);
        //}

        //bool IHandTransitTryGet.TryGet(Card card)
        //{
        //    return _hand.TryTakeAwayCard(card);
        //}

        //void IHandTransitSet.Set(Card card)
        //{
        //    _hand.SeatCard(card);
        //}

        //Card IHandTransitGetLast.Get()
        //{
        //    return _hand.GetLastCard();
        //}

        //List<Card> IHandTransitGetAll.Get()
        //{
        //    List<Card> cards;

        //    if (_hand.TryTakeAwayAllCards(out cards) == false)
        //    {
        //        cards = null;
        //    }

        //    return cards;
        //}

        public void ActivateSlimeEffect(int countTurns)
        {
            _personEffectsHandler.SlimeEffectHandler.Activate(countTurns);
        }

        public abstract void ActivateSharpSnakeEffect(Action callback);

        public void ActivateFireDraw(int countTurns)
        {
            StartTurnDraw.SetFireMode(countTurns);
        }

        //public void DeactivateSlimeEffect()
        //{

        //}
    }
}