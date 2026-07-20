using System.Threading;
using Cysharp.Threading.Tasks;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuEnemyAI : ILookCardMenu
    {
        private readonly CancellationToken _fightToken;

        private bool _isComplete;

        public bool IsComplete => _isComplete;

        public bool? IsActive { get; private set; } = null;

        // Логики в классе нет совсем, но пока оставлю тк подозреваю что он все равно понадобится для серверной синхронизации
        public LookCardMenuEnemyAI(CancellationToken fightToken) 
        {
            _fightToken = fightToken;
        }

        public void Activate(LookCardMenuActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _isComplete = false;

            Activating(_fightToken).Forget();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
        }

        private async UniTask Activating(CancellationToken token)
        {
            await UniTask.WaitForSeconds(0.5f, cancellationToken: token);

            _isComplete = true;

            Deactivate();
        }
    }
}