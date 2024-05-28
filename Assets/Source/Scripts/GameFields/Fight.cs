using GameFields.Persons;
using System.Collections;
using UnityEngine;
using GameFields.EndTurnButtons;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Tables;
using System;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IPersonSideListener, IFightStep
    {
        private readonly int _maxTurns = 100;

        private static int _numbOperation;

        private Player _player;
        private EnemyAI _enemy;
        private EndTurnButton _endTurnButton;

        private int _turnNumber;

        private Person _activePerson;
        private Person _deactivePerson;
        private bool _isComlpete;
        private FightResult _fightResult;
       
        public Fight(Player player, EnemyAI enemy, EndTurnButton endTurnButton, FightResult fightResult)
        {
            _numbOperation = 0;

            _isComlpete = false;
            _turnNumber = 1;

            _fightResult = fightResult;
            _player = player;
            _enemy = enemy;
            _endTurnButton = endTurnButton;
        }

        public Person ActivePerson => _activePerson;
        public Person DeactivePerson => _deactivePerson;
        public bool IsComplete => _isComlpete;

        public void OnEndTurn()
        {
            _turnNumber++;

            DiscardCards();
            CheckEndFight();
            SwitchPerson();
            StartTurn();
        }

        //public void PrepareToStart()
        //{
        //    _isComlpete = false;
        //}

        public void StartStep()
        {
            _activePerson = _player;
            _deactivePerson = _enemy;

            StartTurn();
        }

        private void DiscardCards()
        {
            _activePerson.DiscardCards();
        }

        private void CheckEndFight()
        {
            if (_turnNumber >= _maxTurns)
            {
                _fightResult.SetDraw();
                _isComlpete = true;
            }
        }

        //private void SwitchPerson()
        //{
        //    if (_activePerson == _player)
        //    {
        //        SetEnemyTurn();
        //    }
        //    else
        //    {
        //        SetPlayerTurn();
        //    }
        //}

        //private void SetPlayerTurn()
        //{
        //    _activePerson = _player;
        //    _deactivePerson = _enemy;

        //    _activePerson.StartTurn();

        //    //_endTurnButton.SetActiveSide();
        //}

        private void SwitchPerson()
        {
            Person tempPerson = _activePerson;

            _activePerson = _deactivePerson;
            _deactivePerson = tempPerson;
        }

        private void StartTurn()
        {
            _activePerson.PrepareToStart();

            _activePerson.StartTurn();

            TurnProcessing().ToUniTask();
        }

        //private void SetEnemyTurn()
        //{
        //    _activePerson = _enemy;
        //    _deactivePerson = _player;

        //    _activePerson.StartTurn();

        //    EnemyTurnProcessing().ToUniTask();
        //}

        //private IEnumerator EnemyTurnProcessing()
        //{
        //    yield return new WaitUntil(() => _enemy.IsComplete);

        //    OnEndTurn();
        //}

        private IEnumerator TurnProcessing()
        {
            _numbOperation++;
            int numbOperation = _numbOperation;

            Debug.Log(numbOperation + ") _activePerson (TurnProcessing) start: " + _activePerson);
            yield return new WaitUntil(() => _activePerson.IsComplete);

            Debug.Log(numbOperation + ") activePerson (TurnProcessing) end: " + _activePerson);
            OnEndTurn();
        }
    }
}
