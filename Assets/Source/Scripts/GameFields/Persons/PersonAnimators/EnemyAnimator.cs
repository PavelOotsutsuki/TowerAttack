using Tools;
using UnityEngine;

namespace GameFields.Persons.PersonAnimators
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;

        public void Init(IEndTurnHandler endTurnHandler, CardImitationActions cardImitationActions)
        {
            _enemyDragAndDropImitation.Init(endTurnHandler, cardImitationActions);
        }

        public void StartDragAndDropAnimation()
        {
            _enemyDragAndDropImitation.StartDragAndDropAnimation();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineEnemyDragAndDropImitation();
        }

        [ContextMenu(nameof(DefineEnemyDragAndDropImitation))]
        private void DefineEnemyDragAndDropImitation()
        {
            AutomaticFillComponents.DefineComponent(this, ref _enemyDragAndDropImitation, ComponentLocationTypes.InThis);
        }
    }
}
