using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Persons;
using Hands;
using Tables;
using Cards;

namespace GameFields.FightProcess
{
    internal class Fight : IPlayCardManager, IEndTurnHandler
    {
        private Player _player;
        private EnemyAI _enemy;
        private HandPlayer _handPlayer;
        private HandAI _handAI;
        private TablePlayer _tablePlayer;
        private TableAI _tableAI;
        private Deck _deck;
        private DrawCardAnimator _drawCardAnimator;

        private Person _activePerson;
        private Hand _activeHand;
        private Table _activeTable;

        public Fight(Player player, EnemyAI enemy, HandPlayer handPlayer, HandAI handAI, TablePlayer tablePlayer, TableAI tableAI, Deck deck, DrawCardAnimator drawCardAnimator)
        {
            _player = player;
            _enemy = enemy;
            _handPlayer = handPlayer;
            _handAI = handAI;
            _tablePlayer = tablePlayer;
            _tableAI = tableAI;
            _deck = deck;
            _drawCardAnimator = drawCardAnimator;

            SetPlayerTurn();
        }

        public void PlayCard(Card card)
        {
            _activeHand.RemoveCard(card);
        }

        public void OnEndTurn()
        {
            SwitchPerson();
            StartTurn();
        }

        private void SwitchPerson()
        {
            if (_activePerson == _player)
            {
                SetEnemyTurn();
            }
            else
            {
                SetPlayerTurn();
            }

            ActivateTable(_activeTable);
        }

        private void SetPlayerTurn()
        {
            _activePerson = _player;
            _activeHand = _handPlayer;
            _activeTable = _tablePlayer;
        }

        private void SetEnemyTurn()
        {
            _activePerson = _enemy;
            _activeHand = _handAI;
            _activeTable = _tableAI;
        }

        private void StartTurn()
        {
            for (int i = 0; i < _activePerson.CountDrawCards; i++)
            {
                DrawCards();
            }
        }

        private void ActivateTable(Table activeTable)
        {
            DeactivateTables();

            activeTable.Activate();
        }

        private void DeactivateTables()
        {
            _tablePlayer.Deactivate();
            _tableAI.Deactivate();
        }

        private void DrawCards()
        {
            if (_deck.TryTakeCard(out Card drawnCard))
            {
                drawnCard.AddToHand(_activeHand);
                _drawCardAnimator.Init(_activeHand, drawnCard);
            }
        }
    }
}
