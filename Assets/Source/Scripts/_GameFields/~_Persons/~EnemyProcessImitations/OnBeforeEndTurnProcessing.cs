using System.Threading;
using GameFields.Persons.EffectHandlers;
using Tools.InputSettings;

namespace GameFields.Persons.EnemyProcessImitations
{
    internal class OnBeforeEndTurnProcessing : PersonStep, IInputLogicObject
    {
        private readonly PersonEffectsHandler _personEffectsHandler;

        private bool _isComplete;

        public OnBeforeEndTurnProcessing(InteractionActivator interactionActivator, PersonEffectsHandler personEffectsHandler, CancellationToken turnToken)
            : base(interactionActivator, turnToken)
        {
            _isComplete = false;
            _personEffectsHandler = personEffectsHandler;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;

            _personEffectsHandler.BeforeEndTurn(() => _isComplete = true, Token);
        }
    }
}