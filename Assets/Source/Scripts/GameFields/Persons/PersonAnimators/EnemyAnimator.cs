using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons;
using GameFields.Persons.PersonAnimators;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields
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
    }
}
