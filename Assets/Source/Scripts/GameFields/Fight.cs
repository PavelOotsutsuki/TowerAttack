using GameFields.Persons;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Discarding;
using GameFields.Effects;
using Zenject;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IPersonSides, IFightStep
    {
        private const int MaxTurns = 100;

        private readonly Player _player;
        private readonly EnemyAI _enemy;
        private readonly FightResult _fightResult;
        private readonly SignalBus _bus;

        private int _turnNumber;

        public Fight(Player player, EnemyAI enemy, FightResult fightResult, SignalBus bus)
        {
            _player = player;
            _enemy = enemy;
            _fightResult = fightResult;
            _bus = bus;

            _turnNumber = 1;

            IsComplete = false;
            
            _bus.Subscribe<EffectCreatedSignal>(OnEffectCreatedSignal);
        }

        ~Fight()
        {
            _bus.Unsubscribe<EffectCreatedSignal>(OnEffectCreatedSignal);
        }

        private void OnEffectCreatedSignal(EffectCreatedSignal signal)
        {
            if (signal.Effect.Target == EffectTarget.Self)
                ActivePerson.ApplyEffect(signal.Effect);
            else
                DeactivePerson.ApplyEffect(signal.Effect);
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
            (ActivePerson, DeactivePerson) = (DeactivePerson, ActivePerson);
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
