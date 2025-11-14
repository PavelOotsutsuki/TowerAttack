using Cards;

namespace GameFields.Persons.EffectHandlers
{
    public class PersonEffectsHandlerRoot
    {
        private readonly PersonEffectsHandler _enemyEffectsHandler;
        private readonly PersonEffectsHandler _playerEffectsHandler;

        public PersonEffectsHandlerRoot(PersonEffectsHandler enemyEffectsHandler, PersonEffectsHandler playerEffectsHandler)
        {
            _enemyEffectsHandler = enemyEffectsHandler;
            _playerEffectsHandler = playerEffectsHandler;
        }

        public void EndEffect(Card card)
        {
            _enemyEffectsHandler.EndEffect(card);
            _playerEffectsHandler.EndEffect(card);
        }
    }
}