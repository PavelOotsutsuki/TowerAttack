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
        private CanvasScaler _canvasScaler;

        internal void Init(ICardDropPlaceImitation cardDropPlaceImitation, CanvasScaler canvasScaler)
        {
            _cardDropPlaceImitation = cardDropPlaceImitation;
            _canvasScaler = canvasScaler;
        }

        internal void StartDragAndDropAnimation(Card card)
        {
            int logicNumber = Random.Range(1, CountLogics);

            if (logicNumber == 1)
            {
                StartCoroutine(DragAndDropBehaviour1(card));
            }
        }

        private IEnumerator DragAndDropBehaviour1(Card card)
        {
            float screenFactor = Screen.height / _canvasScaler.referenceResolution.y;

            float startDelay = Random.Range(_startDelayMin, _startDelayMax);
            bool isWithRepeat = System.Convert.ToBoolean(Random.Range(0, 1));
            float cardViewDelay = Random.Range(_cardViewDelayMin, _cardViewDelayMax);

            yield return new WaitForSeconds(startDelay);

            card.PlaySelectCardAnimation(screenFactor, _cardViewTime);

            yield return new WaitForSeconds(_cardViewTime + cardViewDelay);

            if (isWithRepeat)
            {
                float cardViewDelay2 = Random.Range(_cardViewDelayMin, _cardViewDelayMax);

                card.PlayUnselectCardAnimation(screenFactor, _cardViewTime);

                yield return new WaitForSeconds(_cardViewTime);

                card.PlaySelectCardAnimation(screenFactor, _cardViewTime);

                yield return new WaitForSeconds(_cardViewTime + cardViewDelay2);
            }

            card.PlayCardAnimation();

            yield return new WaitForSeconds(_cardTranslateTime);


            _cardDropPlaceImitation.GetCard(card);
        }
    }
}
