namespace GameFields.Persons.Commons
{
    public class LoseActionsRoot
    {
        private readonly LoseActions _playerLoseActions;
        private readonly LoseActions _enemyLoseActions;

        public LoseActionsRoot(LoseActions playerLoseActions, LoseActions enemyLoseActions)
        {
            _playerLoseActions = playerLoseActions;
            _enemyLoseActions = enemyLoseActions;
        }

        public void Capitulate(Person person)
        {
            switch (person)
            {
                case EnemyAI:
                    _enemyLoseActions.Activate();
                    break;
                case Player:
                    _playerLoseActions.Activate();
                    break;
                default:
                    throw new System.Exception("Неизвестный тип персонажа");
            }
        }
    }
}