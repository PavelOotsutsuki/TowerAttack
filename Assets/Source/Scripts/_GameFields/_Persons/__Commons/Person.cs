using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
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
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.LookCardMenues;

namespace GameFields.Persons.Commons
{
    public abstract class Person : ITurnStep, IDrawCardManager, IPersonObject
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
        private readonly ILookCardMenu _lookCardMenu;
        private readonly IBoomTower _boomTower;
        private readonly ICardNumberKeeper _cardNumberKeeper;
        private readonly SkipTurnView _skipTurnView;
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
            ISelectMenuActivator choiceMenu, ISelectMenuActivator choiceMenuImitation, PersonEffectsHandler personEffectsHandler,
            ILookCardMenu lookCardMenu, SkipTurnView skipTurnView)
        {
            _hand = hand;
            Bus = bus;
            _playingZone = playingZone;
            _tower = tower;
            _boomTower = tower;
            _cardNumberKeeper = tower;
            _drawCardRoot = drawCardRoot;
            StartTurnDraw = startTurnDraw;
            //TurnProcess = turnProcess;
            _discover = discover;
            _attackMenu = attackMenu;
            _choiceMenu = choiceMenu;
            _choiceMenuImitation = choiceMenuImitation;
            _lookCardMenu = lookCardMenu;
            //_loseActions = loseActions;
            _skipTurnView = skipTurnView;
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
        //public PersonEffectsHandler PersonEffectsHandler => _personEffectsHandler;
        public bool IsDoubleEffect => _personEffectsHandler.DoubleEffectHandler.IsActive;
        public bool TryUseScarecrowEffect => _personEffectsHandler.ScarecrowEffectHandler.TryUse();
        public bool IsWiseEffectActive => _personEffectsHandler.WiseMonkEffectHandler.IsActive;

        public void StartStep()
        {
            IsComplete = false;

            _personSteps.Clear();

            //_hand.OnStartTurn();
            //OnStartStep();

            if (_personEffectsHandler.SkipTurnEffectHandler.IsActive)
            {
                InitSkipSteps();
            }
            else
            {
                InitCommonSteps();
            }

            _personEffectsHandler.OnStartTurn();
            _currentStep = _personSteps.Pop();

            ProcessingTurn().ToUniTask();
        }

        protected void AddStartTurnDrawStep()
        {
            PushStep(StartTurnDraw);
            //_personEffectsHandler.OnStartTurn();
        }

        public void FinishTurn()
        {
            //_hand.OnFinishTurn();
            //_personEffectsHandler.OnEndTurn();

            //IReadOnlyList<Card> discardedCards = _playingZone.DiscardCards();
            _playingZone.DiscardCards();

            //if (discardedCards.Count > 0)
            //    Bus.Fire(new DiscardCardsSignal(discardedCards));
        }

        public void DiscoverCards(IReadOnlyList<IDiscoverable> cards, string activateMessage, DiscoverResult discoverResult)
        {
            if (cards is null)
            {
                return;
            }

            if (cards.Count > _discover.MaxSeats)
            {
                throw new Exception("Пытаемся закинуть в discover больше карт чем можем");
                //return;
            }

            DiscoverActivateData discoverActivateData = new DiscoverActivateData(cards, activateMessage, discoverResult);

            _discover.Activate(discoverActivateData);
        }

        public void LookCards(LookCardMenuActivateData activateData, Action callback = null)
        {
            _lookCardMenu.Activate(activateData);

            WaitingToInvoke(_lookCardMenu, callback).ToUniTask();
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

        //public void Capitulate()
        //{
        //    _loseActions.Activate();
        //}

        public bool IsSuccessChoiceTowerNumber(int number)
        {
            return _cardNumberKeeper.Card.IsSuccessChoice(number);
        }

        //private IEnumerator WaitUntilCapitulate()
        //{
        //    yield return new WaitForSeconds(5f);

        //    Bus.Fire(new PersonWinSignal(this));
        //}


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

            callback?.Invoke();
        }

        //public void AttackDeactivate()
        //{
        //    _attackMenu.Deactivate();
        //}

        //protected abstract void OnStartStep();

        protected void PushStep(PersonStep turnStep) => _personSteps.Push(turnStep);

        protected abstract void InitCommonSteps();

        private void InitSkipSteps()
        {
            PushStep(_skipTurnView);
        }
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

        //public void StartEffect(Effect effect, CardEffectConfig effectData)
        public void StartEffect(PersonEffect personEffect, bool isRememberEffect = true)
        {
            if (isRememberEffect)
                LastEffect = personEffect.CardEffectConfig;

            //Debug.Log("StartEffect");
            StartAction(personEffect.Effect);

            _playingZone.SeatCard(personEffect);
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

        public void ActivateJusticeBullEffect()
        {
            _personEffectsHandler.JusticeBullEffectHandler.Activate();
        }

        public void ActivateSkipTurns(Card card)
        {
            _personEffectsHandler.SkipTurnEffectHandler.Activate(card);
        }

        public bool TryActivateGnomeEffect(out int countTurns)
        {
            return _personEffectsHandler.GnomeEffectCounter.TryActivate(out countTurns);
        }

        public int BrothersCounter => _personEffectsHandler.BrothersEffectHandler.ExtraCount;

        public void UpgradeBrothers(int increaseValue)
        {
            _personEffectsHandler.BrothersEffectHandler.Upgrade(increaseValue);
        }

        public void ActivateDoubleEffect(Card card)
        {
            _personEffectsHandler.DoubleEffectHandler.Activate(card);
        }

        public void AddCurse(Card card)
        {
            _personEffectsHandler.CurseEffectHandler.Activate(card);
        }

        public void ActivateSlimeEffect(Card card)
        {
            _personEffectsHandler.SlimeEffectHandler.Activate(card);
        }

        //public abstract void ActivateSharpSnakeEffect(Action callback);

        public void ActivateFireDraw(Card card)
        {
            _personEffectsHandler.FireEffectHandler.Activate(card);
        }

        public void ActivateFateInevitability(Card card, int countTurns)
        {
            _personEffectsHandler.FateInevitabilityHandler.Activate(card, countTurns);
        }

        public void ActivateScarecrowEffect(int countTurns, Card card)
        {
            _personEffectsHandler.ScarecrowEffectHandler.Activate(countTurns, card);
        }

        public void ActivateWiseMonkEffect(Card card)
        {
            _personEffectsHandler.WiseMonkEffectHandler.Activate(card);
        }

        //public void DeactivateSlimeEffect()
        //{

        //}
    }
}