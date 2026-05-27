using Tools;

namespace GameFields.Persons
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

        public void Capitulate(Person person, CancellationTokenData cancellationTokenData)
        {
            switch (person)
            {
                case EnemyAI:
                    _enemyLoseActions.Activate(cancellationTokenData);
                    break;
                case Player:
                    _playerLoseActions.Activate(cancellationTokenData);
                    break;
                default:
                    throw new System.Exception("Неизвестный тип персонажа");
            }
        }
    }
}