using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;

namespace GameFields.Persons.EnemyProcessImitations
{
    public abstract class DragAndDropBehaviour : ICompletable, IActivatable
    {
        //protected const float SelectYDirection = 1;
        //protected const float UnselectYDirection = -1;

        //protected readonly EnemyDragAndDropImitationData Data;
        //protected readonly CardDragAndDropImitationActions CardImitationActions;
        //protected readonly Hand Hand;
        protected readonly CancellationToken Token;

        private bool _isComplete;

        public DragAndDropBehaviour(CancellationToken fightToken)
        {
            _isComplete = false;
            Token = fightToken;
        }

        public bool IsComplete => _isComplete;

        public void Activate()
        {
            _isComplete = false;

            Activating(Token).Forget();
        }

        private async UniTask Activating(CancellationToken token)
        {
            await OnActivating(token);

            _isComplete = true;
        }

        protected abstract UniTask OnActivating(CancellationToken token);
    }
}