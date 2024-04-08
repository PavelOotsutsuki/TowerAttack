using Tools;
using UnityEngine;

namespace GameFields.Persons.PersonAnimators
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        [SerializeField] private int _countDrawCards = 1;
        [SerializeField] private float _drawCardsDelay = 0.5f;

        public int CountDrawCards => _countDrawCards;
        public float DrawCardsDelay => _drawCardsDelay;

        public void Init(IEndTurnHandler endTurnHandler, CardDragAndDropImitationActions cardImitationActions)
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
