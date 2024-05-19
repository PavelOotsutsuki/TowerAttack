using GameFields.Persons;
using System.Collections;
using UnityEngine;
using GameFields.EndTurnButtons;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IPersonSideListener, IFightStep
    {
        private readonly int _maxTurns = 100;
        private readonly MonoBehaviour _coroutineContainer;

        private Player _player;
        private EnemyAI _enemy;
        private EndTurnButton _endTurnButton;

        private int _turnNumber;

        private Person _activePerson;
        private Person _deactivePerson;
        private bool _isComlpete;
        private FightResult _fightResult;
       
        public Fight(Player player, EnemyAI enemy, EndTurnButton endTurnButton, FightResult fightResult, MonoBehaviour coroutineContainer)
        {
            _isComlpete = false;
            _turnNumber = 1;
            _coroutineContainer = coroutineContainer;

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
        }

        //public void PrepareToStart()
        //{
        //    _isComlpete = false;
        //}

        public void StartStep()
        {
            SetPlayerTurn();
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
        }

        private void SetPlayerTurn()
        {
            _activePerson = _player;
            _deactivePerson = _enemy;

            _activePerson.StartTurn();

            _endTurnButton.SetActiveSide();
        }

        private void SetEnemyTurn()
        {
            _activePerson = _enemy;
            _deactivePerson = _player;

            _activePerson.StartTurn();

            _coroutineContainer.StartCoroutine(EnemyTurnProcessing());
        }

        private IEnumerator EnemyTurnProcessing()
        {
            yield return new WaitUntil(() => _enemy.IsComplete);

            OnEndTurn();
        }
    }
}
