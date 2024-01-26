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
        [SerializeField] private float _cardViewTime = 1f;
        [SerializeField] private float _cardViewDelayMin = 2f;
        [SerializeField] private float _cardViewDelayMax = 4f;
        [SerializeField] private float _cardTranslateInDropPlaceTime = 0.5f;
        [SerializeField] private float _cardReturnInHandTime = 0.5f;
        [SerializeField] private float _endTurnDelay = 2f;
        [SerializeField] private int _maxCountRepeat = 1;

        private ICardDropPlaceImitation _cardDropPlaceImitation;
        private IEndTurnHandler _endTurnHandler;
        private CanvasScaler _canvasScaler;
        private ICardDragImitationListener _cardDragImitationListener;

        internal void Init(ICardDropPlaceImitation cardDropPlaceImitation, IEndTurnHandler endTurnHandler, ICardDragImitationListener cardDragImitationListener, CanvasScaler canvasScaler)
        {
            _cardDragImitationListener = cardDragImitationListener;
            _cardDropPlaceImitation = cardDropPlaceImitation;
            _endTurnHandler = endTurnHandler;
            _canvasScaler = canvasScaler;
        }

        internal void StartDragAndDropAnimation(Card card)
        {
            int logicNumber = Random.Range(1, CountLogics + 1);

            if (logicNumber == 1)
            {
                StartCoroutine(DragAndDropBehaviour1(card));
            }
        }

        private IEnumerator DragAndDropBehaviour1(Card card)
        {
            float screenFactor = Screen.height / _canvasScaler.referenceResolution.y;

            float startDelay = Random.Range(_startDelayMin, _startDelayMax);
            float countRepeat = Random.Range(0, _maxCountRepeat + 1);

            yield return new WaitForSeconds(startDelay);

            for (int i = 0; i < countRepeat + 1; i++)
            {
                float cardViewDelay = Random.Range(_cardViewDelayMin, _cardViewDelayMax);

                card.PlaySelectCardAnimation(screenFactor, _cardViewTime);
                yield return new WaitForSeconds(_cardViewTime + cardViewDelay);

                if (i != countRepeat)
                {
                    card.PlayUnselectCardAnimation(screenFactor, _cardViewTime);
                    yield return new WaitForSeconds(_cardViewTime);
                }
            }

            card.PlayCardAnimation(_cardDropPlaceImitation.GetCentralСoordinates(), _cardTranslateInDropPlaceTime);
            _cardDragImitationListener.OnCardDrag(card);
            yield return new WaitForSeconds(_cardTranslateInDropPlaceTime);

            if (_cardDropPlaceImitation.TryGetCard(card) == false)
            {
                _cardDragImitationListener.OnCardDrop();
                card.PlayReturnInHandAnimation(_cardReturnInHandTime);
                yield return new WaitForSeconds(_cardReturnInHandTime);
            }

            yield return new WaitForSeconds(_endTurnDelay);

            _endTurnHandler.OnEndTurn();
        }
    }
}
