using System.Threading;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.EnemyProcessImitations;
using GameFields.Persons.Hands;

namespace GameFields.Persons
{
    public class EnemyDragAndDropImitationCreator
    {
        private readonly CardDragAndDropImitationActions _cardImitationActions;
        private readonly EnemyDragAndDropImitationData _data;
        private readonly InteractionActivator _interactionActivator;
        private readonly SkipTurnChecker _skipTurnChecker;
        private readonly IDrawnCardWatcher _drawnCardWatcher;
        private readonly Hand _hand;
        private readonly IAIThinkLogic _mainAIThinkLogic;
        private readonly GnomeEffectHandler _gnomeEffectHandler;

        public EnemyDragAndDropImitationCreator(CardDragAndDropImitationActions cardImitationActions, EnemyDragAndDropImitationData data,
            InteractionActivator interactionActivator, SkipTurnChecker skipTurnChecker, IDrawnCardWatcher drawnCardWatcher, Hand hand,
            IAIThinkLogic mainAIThinkLogic, GnomeEffectHandler gnomeEffectHandler)
        {
            _cardImitationActions = cardImitationActions;
            _data = data;
            _interactionActivator = interactionActivator;
            _skipTurnChecker = skipTurnChecker;
            _drawnCardWatcher = drawnCardWatcher;
            _hand = hand;
            _mainAIThinkLogic = mainAIThinkLogic;
            _gnomeEffectHandler = gnomeEffectHandler;
        }

        internal EnemyDragAndDropImitation Create(CancellationToken turnToken) => new EnemyDragAndDropImitation(
                _cardImitationActions, _data, _interactionActivator, _skipTurnChecker, _drawnCardWatcher, _hand, _mainAIThinkLogic, _gnomeEffectHandler, turnToken
            );
    }
}