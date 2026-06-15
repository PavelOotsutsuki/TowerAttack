using System.Threading;
using GameFields.EndTurnButtons;
using GameFields.Persons.EffectHandlers;

namespace GameFields.Persons
{
    public class EndTurnProcessingCreator
    {
        private readonly IEndTurnButtonStateWatcher _endTurnButtonStateWatcher;
        private readonly InteractionActivator _interactionActivator;
        private readonly PersonEffectsHandler _personEffectsHandler;

        public EndTurnProcessingCreator(IEndTurnButtonStateWatcher endTurnButtonStateWatcher, InteractionActivator interactionActivator, PersonEffectsHandler personEffectsHandler)
        {
            _endTurnButtonStateWatcher = endTurnButtonStateWatcher;
            _interactionActivator = interactionActivator;
            _personEffectsHandler = personEffectsHandler;
        }

        internal EndTurnProcessing Create(CancellationToken turnToken) => new EndTurnProcessing(_endTurnButtonStateWatcher, _interactionActivator, _personEffectsHandler, turnToken);
    }
}