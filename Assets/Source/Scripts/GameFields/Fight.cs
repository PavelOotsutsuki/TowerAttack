using GameFields.Persons;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IPersonSideListener, IFightStep
    {
        private const int MaxTurns = 100;

        private readonly Player _player;
        private readonly EnemyAI _enemy;
        private readonly FightResult _fightResult;

        private int _turnNumber;
        
        public Fight(Player player, EnemyAI enemy, FightResult fightResult)
        {
            _player = player;
            _enemy = enemy;
            _fightResult = fightResult;

            _turnNumber = 1;

            IsComplete = false;
        }

        public bool IsComplete { get; private set; }
        public Person ActivePerson { get; private set; }
        public Person DeactivePerson { get; private set; }

        private bool TurnsIsOut => _turnNumber >= MaxTurns;

        public void OnEndTurn()
        {
            _turnNumber++;

            ActivePerson.FinishTurn();
            
            if (TurnsIsOut)
            {
                _fightResult.SetDraw();
                IsComplete = true;
            }
            
            SwitchPerson();
            StartTurn();
        }

        public void StartStep()
        {
            ActivePerson = _player;
            DeactivePerson = _enemy;

            StartTurn();
        }

        private void SwitchPerson()
        {
            Person temp = ActivePerson;
            ActivePerson = DeactivePerson;
            DeactivePerson = temp;
        }

        private void StartTurn()
        {
            ActivePerson.StartStep();

            TurnProcessing().ToUniTask();
        }

        private IEnumerator TurnProcessing()
        {
            yield return new WaitUntil(() => ActivePerson.IsComplete);

            OnEndTurn();
        }
    }
}
