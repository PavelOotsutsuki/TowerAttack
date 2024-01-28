using Cards;
using GameFields.Persons;
using GameFields.Persons.PersonAnimators;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private EnemyDragAndDropAnimator _enemyDragAndDropAnimator;

        public void Init(ICardDropPlaceImitation cardDropPlaceImitation, IEndTurnHandler endTurnHandler, ICardDragImitationListener cardDragImitationListener, CanvasScaler canvasScaler)
        {
            _enemyDragAndDropAnimator.Init(cardDropPlaceImitation, endTurnHandler, cardDragImitationListener, canvasScaler);
        }

        public void StartDragAndDropAnimation(CardImitationActions cardImitationActions)
        {
            _enemyDragAndDropAnimator.StartDragAndDropAnimation(cardImitationActions);
        }
    }
}
