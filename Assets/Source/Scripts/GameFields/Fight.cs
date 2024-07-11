using GameFields.Persons;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Discarding;
using Zenject;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IPersonSideListener, IFightStep
    {
        private const int MaxTurns = 100;

        private readonly SignalBus _bus;
        private readonly Player _player;
        private readonly EnemyAI _enemy;
        private readonly FightResult _fightResult;

        private int _turnNumber;
        
        public Fight(SignalBus bus, Player player, EnemyAI enemy, FightResult fightResult)
        {
            _bus = bus;
            _player = player;
            _enemy = enemy;
            _fightResult = fightResult;

            _turnNumber = 1;

            IsComplete = false;
        }

        public bool IsComplete { get; private set; }
        public Person ActivePerson { get; private set; }
        public Person DeactivePerson { get; private set; }

        public void OnEndTurn()
        {
            _turnNumber++;

            _bus.Fire(new DiscardCardsSignal(ActivePerson.DiscardCards()));
            
            CheckEndFight();
            SwitchPerson();
            StartTurn();
        }

        public void StartStep()
        {
            ActivePerson = _player;
            DeactivePerson = _enemy;

            StartTurn();
        }

        private void CheckEndFight()
        {
            if (_turnNumber >= MaxTurns)
            {
                _fightResult.SetDraw();
                IsComplete = true;
            }
        }

        private void SwitchPerson() => (ActivePerson, DeactivePerson) = (DeactivePerson, ActivePerson);

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
