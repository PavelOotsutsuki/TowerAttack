namespace GameFields.Persons.EffectHandlers.Brothers
{
    public class BrothersEffectHandlerRoot
    {
        private readonly BrothersEffectHandler _playerBrothersEffectHandler;
        private readonly BrothersEffectHandler _enemyBrothersEffectHandler;

        public BrothersEffectHandlerRoot(BrothersEffectHandler playerBrothersEffectHandler,
            BrothersEffectHandler enemyBrothersEffectHandler)
        {
            _playerBrothersEffectHandler = playerBrothersEffectHandler;
            _enemyBrothersEffectHandler = enemyBrothersEffectHandler;
        }

        public void Upgrade(int startValue, int increaseValue)
        {
            _playerBrothersEffectHandler.Upgrade(startValue, increaseValue);
            _enemyBrothersEffectHandler.Upgrade(startValue, increaseValue);
        }
    }
}