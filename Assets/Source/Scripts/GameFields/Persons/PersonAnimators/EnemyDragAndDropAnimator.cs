using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.PersonAnimators
{
    public class EnemyDragAndDropAnimator : MonoBehaviour
    {
        private const int CountLogics = 1;

        [SerializeField] private float _startDelayMin = 1f;
        [SerializeField] private float _startDelayMax = 2f;
        [SerializeField] private float _cardViewTime = 0.5f;
        [SerializeField] private float _cardViewDelayMin = 2f;
        [SerializeField] private float _cardViewDelayMax = 4f;
        [SerializeField] private float _cardTranslateTime = 0.5f;

        private ICardDropPlaceImitation _cardDropPlaceImitation;

        internal void Init(ICardDropPlaceImitation cardDropPlaceImitation)
        {
            _cardDropPlaceImitation = cardDropPlaceImitation;
        }

        internal void StartDragAndDropAnimation(Card drawnCard)
        {
            int logicNumber = Random.Range(1, CountLogics);

            if (logicNumber == 1)
            {
                StartCoroutine(DragAndDropBehaviour1(drawnCard));
            }
        }

        private IEnumerator DragAndDropBehaviour1(Card drawnCard)
        {
            float startDelay = Random.Range(_startDelayMin, _startDelayMax);
            bool isWithRepeat = System.Convert.ToBoolean(Random.Range(0, 1));
            float cardViewDelay = Random.Range(_cardViewDelayMin, _cardViewDelayMax);

            yield return new WaitForSeconds(startDelay);

            drawnCard.PlaySelectCardAnimation();

            yield return new WaitForSeconds(_cardViewTime + cardViewDelay);

            if (isWithRepeat)
            {
                float cardViewDelay2 = Random.Range(_cardViewDelayMin, _cardViewDelayMax);

                drawnCard.PlayUnselectCardAnimation();

                yield return new WaitForSeconds(_cardViewTime);

                drawnCard.PlaySelectCardAnimation();

                yield return new WaitForSeconds(_cardViewTime + cardViewDelay2);
            }

            drawnCard.PlayCardAnimation();

            yield return new WaitForSeconds(_cardTranslateTime);


            _cardDropPlaceImitation.GetCard(drawnCard);
        }
    }
}
