using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
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

        private bool _isComplete;

        public DragAndDropBehaviour()
        {
            _isComplete = false;
        }

        public bool IsComplete => _isComplete;

        public void Activate()
        {
            _isComplete = false;

            Activating().ToUniTask();
        }

        private IEnumerator Activating()
        {
            yield return OnActivating();

            _isComplete = true;
        }

        protected abstract IEnumerator OnActivating();
    }
}