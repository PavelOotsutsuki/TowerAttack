using System.Threading;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.EnemyProcessImitations;

namespace GameFields.Persons
{
    public class OnBeforeEndTurnProcessingCreator
    {
        private readonly InteractionActivator _interactionActivator;
        private readonly PersonEffectsHandler _personEffectsHandler;

        public OnBeforeEndTurnProcessingCreator(InteractionActivator interactionActivator, PersonEffectsHandler personEffectsHandler)
        {
            _interactionActivator = interactionActivator;
            _personEffectsHandler = personEffectsHandler;
        }

        internal OnBeforeEndTurnProcessing Create(CancellationToken turnToken) => new OnBeforeEndTurnProcessing(_interactionActivator, _personEffectsHandler, turnToken);
    }
}