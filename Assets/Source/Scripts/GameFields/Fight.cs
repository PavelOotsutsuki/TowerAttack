using GameFields.Persons;
using Cards;
using System.Collections;
using UnityEngine;

namespace GameFields
{
    internal class Fight : MonoBehaviour, IEndTurnHandler
    {
        private readonly int _maxTurns = 100;

        private Player _player;
        private EnemyAI _enemy;
        private Deck _deck;

        private IPerson _activePerson;
        private int _turnNumber;

        //public Fight(Player player, EnemyAI enemy, Deck deck)
        //{
        //    _turnNumber = 1;

        //    _player = player;
        //    _enemy = enemy;
        //    _deck = deck;

        //    SetPlayerTurn();
        //}

        public void Init(Player player, EnemyAI enemy, Deck deck)
        {
            _turnNumber = 1;

            _player = player;
            _enemy = enemy;
            _deck = deck;

            SetPlayerTurn();
        }

        public void OnEndTurn()
        {
            _turnNumber++;

            CheckEndFight();
            SwitchPerson();
            StartTurn();
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
            if (_activePerson == (IPerson)_player)
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

            ActivateTable();
        }

        private void SetEnemyTurn()
        {
            _activePerson = _enemy;

            DeactivateTable();
        }

        private void StartTurn()
        {
            StartCoroutine(DrawningCards());
        }

        private void ActivateTable()
        {
            _player.ActivateTable();
        }

        private void DeactivateTable()
        {
            _player.DeactivateTable();
        }

        private void EndFight()
        {

        }

        private void DrawCards()
        {
            if (_deck.TryTakeCard(out Card drawnCard))
            {
                _activePerson.DrawCard(drawnCard);
            }
        }

        private IEnumerator DrawningCards()
        {
            WaitForSeconds delay = new WaitForSeconds(_activePerson.DrawCardsDelay);

            for (int i = 0; i < _activePerson.CountDrawCards; i++)
            {
                DrawCards();
                yield return delay;
            }
        }

    }
}
