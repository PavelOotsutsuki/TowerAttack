using GameFields.Persons;
using System.Collections;
using UnityEngine;
using GameFields.EndTurnButtons;
using Cysharp.Threading.Tasks;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IPersonSideListener, IFightStep
    {
        private readonly int _maxTurns = 100;

        private Player _player;
        private EnemyAI _enemy;

        private int _turnNumber;

        private Person _activePerson;
        private Person _deactivePerson;
        private bool _isComlpete;
        private FightResult _fightResult;
       
        public Fight(Player player, EnemyAI enemy, FightResult fightResult)
        {
            _isComlpete = false;
            _turnNumber = 1;

            _fightResult = fightResult;
            _player = player;
            _enemy = enemy;
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

        private void SwitchPerson()
        {
            Person tempPerson = _activePerson;

            _activePerson = _deactivePerson;
            _deactivePerson = tempPerson;
        }

        private void StartTurn()
        {
            _activePerson.StartStep();

            TurnProcessing().ToUniTask();
        }

        private IEnumerator TurnProcessing()
        {
            yield return new WaitUntil(() => _activePerson.IsComplete);

            OnEndTurn();
        }
    }
}
