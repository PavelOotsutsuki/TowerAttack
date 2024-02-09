using System.Collections;
using System.Collections.Generic;
using Cards;
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

        public void StartDragAndDropAnimation(Card card)
        {
            _enemyDragAndDropAnimator.StartDragAndDropAnimation(card);
        }
    }
}
