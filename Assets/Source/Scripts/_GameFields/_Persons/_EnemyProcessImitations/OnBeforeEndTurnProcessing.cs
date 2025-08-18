using GameFields.Persons.Commons;
using GameFields.Persons.EffectHandlers;

namespace GameFields.Persons.EnemyProcessImitations
{
    public class OnBeforeEndTurnProcessing : PersonStep
    {
        private readonly PersonEffectsHandler _personEffectsHandler;

        private bool _isComplete;

        public OnBeforeEndTurnProcessing(InteractionActivator interactionActivator, PersonEffectsHandler personEffectsHandler)
            : base(interactionActivator)
        {
            _isComplete = false;
            _personEffectsHandler = personEffectsHandler;
        }

        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;

            _personEffectsHandler.BeforeEndTurn(() => _isComplete = true);
        }
    }
}