using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Discarding;
using GameFields.Effects;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using StateMachine;
using UnityEngine;
using Zenject;

namespace GameFields.Persons
{
    public abstract class Person : IStartTowerCardSelectionListener, IStateMachineState
    {
        private readonly ITurnStep _turnProcess;
        private readonly CardPlayingZone _playingZone;
        private readonly DrawCardRoot _drawCardRoot;
        private readonly StartTurnDraw _startTurnDraw;
        private readonly Tower _tower;
        private readonly SignalBus _bus;
        private readonly List<Effect> _appliedEffects;
        private readonly PlayedCards _playedCards;
        private readonly Queue<ITurnStep> _turnSteps;
        
        private ITurnStep _currentStep;

        protected Person(CardPlayingZone playingZone, DrawCardRoot drawCardRoot, Tower tower,
            StartTurnDraw startTurnDraw, ITurnStep turnProcess, SignalBus bus)
        {
            _playingZone = playingZone;
            _tower = tower;
            _drawCardRoot = drawCardRoot;
            _startTurnDraw = startTurnDraw;
            _turnProcess = turnProcess;
            _bus = bus;
            
            _appliedEffects = new List<Effect>();
            _playedCards = new PlayedCards();
            _turnSteps = new Queue<ITurnStep>();
            
            _playingZone.Played += PlayingZoneOnPlayed;
            _bus.Subscribe<EffectCreatedSignal>(OnEffectCreatedSignal);
        }

        ~Person()
        {
            _playingZone.Played -= PlayingZoneOnPlayed;
            _bus.Unsubscribe<EffectCreatedSignal>(OnEffectCreatedSignal);
        }

        private void PlayingZoneOnPlayed(Card card, CardCharacter character)
        {
            _playedCards.Add(character, card);
            _bus.Fire(new CardPlayedSignal(character));
        }

        private void OnEffectCreatedSignal(EffectCreatedSignal signal)
        {
            if (_playedCards.HasCharacter(signal.Character))
                _playedCards.BindEffect(signal.Character, signal.Effect);
        }

        public bool IsComplete { get; private set; }
        public bool IsTowerFilled => _tower.IsTowerFill;

        public void StartStep()
        {
            IsComplete = false;
            
            InitTurnSteps();
            OnStartStep();

            _currentStep = _turnSteps.Dequeue();

            ProcessingTurn().ToUniTask();
        }

        private void InitTurnSteps()
        {
            _turnSteps.Clear();
            
            EnqueueStep(_startTurnDraw);
            EnqueueStep(_turnProcess);
        }

        public List<Card> DrawCards(int countCards, Action callback = null)
            => _drawCardRoot.DrawCards(countCards, callback);

        public void ApplyEffect(Effect effect) => _appliedEffects.Add(effect);

        public void FinishTurn()
        {
            Debug.Log($"{ToString()} finishing turn");
            DecreaseEffectsCounters();
            _bus.Fire(new DiscardCardsSignal(GetDiscardedCards()));
        }

        protected abstract void OnStartStep();

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

        private void EnqueueStep(ITurnStep turnStep) => _turnSteps.Enqueue(turnStep);

        private void DecreaseEffectsCounters()
        {
            foreach (Effect effect in _appliedEffects)
                if (effect.CountTurns > 0)
                    effect.DecreaseCounter();
        }

        private List<Card> GetDiscardedCards()
        {
            List<Card> cards = _playedCards.GetDiscardCards();
            List<CardCharacter> characters = cards.Select(card => _playedCards.GetCharacterByCard(card)).ToList();
            
            _playingZone.FreeSeatsByCharacters(characters);
            _playedCards.RemoveCard(cards);

            return cards;
        }
    }
}