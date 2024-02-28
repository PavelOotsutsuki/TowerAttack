using GameFields.Persons;
using Cards;
using System.Collections;
using UnityEngine;
using GameFields.EndTurnButtons;
using GameFields.DiscardPiles;
using Tools;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using GameFields.FirstTurns;
using System;
using UnityEngine.Events;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IStartFightListener
    {
        private readonly int _maxTurns = 100;

        private Player _player;
        private EnemyAI _enemy;
        private Deck _deck;
        private EndTurnButton _endTurnButton;
        private DiscardPile _discardPile;
        private FightAnimator _fightAnimator;
        private FirstTurn _firstTurn;

        private IPerson _activePerson;
        private int _turnNumber;

        public Fight(Player player, EnemyAI enemy, Deck deck, DiscardPile discardPile, EndTurnButton endTurnButton, FightAnimator fightAnimator, FirstTurn firstTurn)
        {
            _turnNumber = 1;

            _player = player;
            _enemy = enemy;
            _deck = deck;
            _discardPile = discardPile;
            _endTurnButton = endTurnButton;
            _fightAnimator = fightAnimator;
            _firstTurn = firstTurn;

            //StartFirstTurn().ToUniTask();
            //StartFirstTurn();
        }

        //public void Init(Player player, EnemyAI enemy, Deck deck, DiscardPile discardPile, EndTurnButton endTurnButton, FightAnimator fightAnimator)
        //{
        //    _turnNumber = 1;

        //    _player = player;
        //    _enemy = enemy;
        //    _deck = deck;
        //    _discardPile = discardPile;
        //    _endTurnButton = endTurnButton;
        //    _fightAnimator = fightAnimator;

        //    SetPlayerTurn();
        //}

        public void OnEndTurn()
        {
            _turnNumber++;

            DiscardCards();
            CheckEndFight();
            SwitchPerson();
            StartTurn();
        }

        public void StartFight()
        {
            //_firstTurn.Deactivate().ToUniTask();
            _firstTurn.Deactivate();

            SetPlayerTurn();
        }

        public void StartFirstTurn()
        {
            //yield return _firstTurn.Activate();
            //Card[] firstTurnCards = new Card[_firstTurnCardsCount * 2];

            //for (int i = 0; i < _firstTurnCardsCount * 2; i++)
            //{
            //    if (_deck.TryTakeCard(out Card drawnCard))
            //    {
            //        firstTurnCards[i] = drawnCard;
            //    }
            //    else
            //    {
            //        Debug.LogError("Недостаточно карт в колоде");
            //    }
            //}
            //if (_deck.IsHasCards(_firstTurnCardsCount * 2) == false)
            //{
            //    throw new ArgumentOutOfRangeException("Недостаточно карт в колоде");
            //}

            //Func<int, Card[]> takeCardsAction = _deck.TakeCards;

            //Card[] playerCards = new Card[_firstTurnCardsCount];
            //Card[] enemyCards = new Card[_firstTurnCardsCount];

            //for (int i = 0; i < _firstTurnCardsCount * 2; i++)
            //{
            //    if (_deck.TryTakeCard(out Card drawnCard))
            //    {
            //        if(i % 2 == 0)
            //        {
            //            playerCards[i / 2] = drawnCard;
            //        }
            //        else
            //        {
            //            enemyCards[i / 2] = drawnCard;
            //        }
            //    }
            //    else
            //    {
            //        Debug.LogError("Недостаточно карт в колоде");
            //    }
            //}

            ////yield return _firstTurn.Activate(firstTurnCards);
            _firstTurn.Activate(_player, _enemy, _deck);
        }

        private void DiscardCards()
        {
            _fightAnimator.DiscardCards(_activePerson.GetDiscardCards());
        }

        private void CheckEndFight()
        {
            if (_turnNumber >= _maxTurns)
            {
                EndFight();
            }
        }

        private void SwitchPerson()
        {
            if (_activePerson is Player)
            {
                SetEnemyTurn();
            }
            else
            {
                SetPlayerTurn();
            }
        }

        private void SetPlayerTurn()
        {
            _activePerson = _player;

            _player.ActivateDropPlaces();
        }

        private void SetEnemyTurn()
        {
            _activePerson = _enemy;

            _player.DeactivateDropPlaces();
        }

        private void StartTurn()
        {
            DrawningCard().ToUniTask();
        }

        private void EndFight()
        {
            Debug.Log("БОЙ ОКОНЧЕН! НИЧЬЯ!");
        }

        private IEnumerator DrawningCard()
        {
            WaitForSeconds delay = new WaitForSeconds(_activePerson.DrawCardsDelay);
            Card drawnCard;

            for (int i = 0; i < _activePerson.CountDrawCards; i++)
            {
                if (_deck.IsHasCards(1))
                {
                    drawnCard = _deck.TakeTopCard();
                    _activePerson.DrawCard(drawnCard);
                    yield return delay;
                }
            }

            if (_activePerson is EnemyAI)
            {
                _enemy.PlayDragAndDropImitation();
            }
            else
            {
                _endTurnButton.SetActiveSide();
            }
        }
    }
}
