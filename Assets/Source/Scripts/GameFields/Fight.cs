using GameFields.Persons;
using System.Collections;
using UnityEngine;
using GameFields.EndTurnButtons;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Tables;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IPersonSideListener
    {
        private readonly int _maxTurns = 100;

        private Player _player;
        private EnemyAI _enemy;
        private EndTurnButton _endTurnButton;

        private Person _activePerson;
        private Person _deactivePerson;
        private int _turnNumber;
        private bool _isComlpete;
        private EndFightResults _endFightResults;
       
        public Fight(Player player, EnemyAI enemy, EndTurnButton endTurnButton)
        {
            _isComlpete = false;
            _turnNumber = 1;

            _player = player;
            _enemy = enemy;
            _endTurnButton = endTurnButton;
        }

        public Person ActivePerson => _activePerson;
        public Person DeactivePerson => _deactivePerson;
        public bool IsComplete => _isComlpete;
        public EndFightResults EndFightResults => _endFightResults;

        public void OnEndTurn()
        {
            _turnNumber++;

            DiscardCards();
            CheckEndFight();
            SwitchPerson();
        }

        public void Start()
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
                _endFightResults = EndFightResults.Draw;
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

            EnemyTurnProcessing().ToUniTask();
        }

        private IEnumerator EnemyTurnProcessing()
        {
            yield return new WaitUntil(() => _enemy.IsImitationComplete);

            OnEndTurn();
        }
    }
}
