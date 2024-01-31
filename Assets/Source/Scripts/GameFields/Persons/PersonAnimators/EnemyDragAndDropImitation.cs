using System.Collections;
using Cards;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.PersonAnimators
{
    public class EnemyDragAndDropImitation : MonoBehaviour
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
        private ICardDragImitationListener _cardDragImitationListener;
        private CanvasScaler _canvasScaler;

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
            RectTransform cardRect = (RectTransform)card.transform;
            float screenFactor = Screen.height / _canvasScaler.referenceResolution.y;

            float startDelay = Random.Range(_startDelayMin, _startDelayMax);
            float countRepeat = Random.Range(0, _maxCountRepeat + 1);

            yield return new WaitForSeconds(startDelay);

            Vector3 cardTargetPosition;
            
            for (int i = 0; i < countRepeat + 1; i++)
            {
                float cardViewDelay = Random.Range(_cardViewDelayMin, _cardViewDelayMax);

                cardTargetPosition = cardRect.localPosition;
                cardTargetPosition.y += cardRect.rect.height / 2 * screenFactor;
                
                card.MoveLocalTo(cardTargetPosition, _cardViewTime);
                yield return new WaitForSeconds(_cardViewTime + cardViewDelay);

                if (i != countRepeat)
                {
                    cardTargetPosition = cardRect.localPosition;
                    cardTargetPosition.y -= cardRect.rect.height / 2 * screenFactor;
                
                    card.MoveLocalTo(cardTargetPosition, _cardViewTime);
                    yield return new WaitForSeconds(_cardViewTime);
                }
            }

            card.MoveLocalTo(_cardDropPlaceImitation.GetCentralСoordinates(), _cardTranslateInDropPlaceTime);
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
