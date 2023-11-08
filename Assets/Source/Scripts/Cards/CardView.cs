using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Tools;
using DG.Tweening;
using System.Collections;

namespace Cards
{
    internal class CardView : MonoBehaviour
    {
        [SerializeField] private CardFront _cardFront;
        [SerializeField] private CardBack _cardBack;
        [SerializeField] private CardDragAndDrop _cardDragAndDrop;

        private RectTransform _cardRectTransform;
        private bool _isBackView;
        private IDragAndDropable _dragAndDropable;
        private CardMovement _cardMovement;

        public CardFront CardFront => _cardFront;

        public void Init(CardSO cardSO, RectTransform cartRectTransform, CardDescription cardDescription, BigCard bigCard, CardMovement cardMovement, IDragAndDropable dragAndDropable, bool isBackView)
        {
            _dragAndDropable = dragAndDropable;
            _cardRectTransform = cartRectTransform;
            _cardFront.Init(cardSO, _cardRectTransform, cardDescription, bigCard);
            //_cardDragAndDrop.Init(_cardRectTransform, _cardFront);
            //_cardBack.Init(cardMovement, _cardRectTransform);
            _cardMovement = cardMovement;
            _isBackView = isBackView;
            SetSideView();
            _dragAndDropable.ActivateDragAndDrop(_isBackView == false);
        }

        //public void TranslateLocalInto(Vector2 positon, Vector3 rotation, float duration)
        //{
        //    _cardRectTransform.DOLocalMove(positon, duration);
        //    _cardRectTransform.DORotate(rotation, duration);
        //}

        public IEnumerator PlayDrawnCardAnimation(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        {
            InvertCardBack(cardBackDuration, cardBackRotation, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardBackDuration);

            _isBackView = false;
            SetSideView();

            InvertCardFront(cardFrontDuration, cardBackScaleFactor, indent);
            yield return new WaitForSeconds(cardFrontDuration);
        }

        private void SetSideView()
        {
            _cardBack.gameObject.SetActive(_isBackView);
            _cardFront.gameObject.SetActive(_isBackView == false);
        }

        private void InvertCardBack(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(cardBackRotation, 0f, 0f);
            float scaleX = _cardRectTransform.localScale.x * cardBackScaleFactor;
            float scaleY = _cardRectTransform.localScale.y * cardBackScaleFactor;
            float scaleZ = _cardRectTransform.localScale.z * cardBackScaleFactor;
            float downWayX = _cardRectTransform.position.x;
            float downWayY = _cardRectTransform.position.y - (_cardRectTransform.position.y - indent - _cardRectTransform.rect.height) / 2;
            float downWayZ = _cardRectTransform.position.z;
            Vector3 downWay = new Vector3(downWayX, downWayY, downWayZ);

            _cardMovement.TranslateLinear(downWay, endRotationVector, cardBackDuration, scaleX, scaleY, scaleZ);
        }

        private void InvertCardFront(float duration, float scaleFactor, float indent)
        {
            Vector3 endRotationVector = new Vector3(0f, 0f, 0f);
            float scaleX = scaleFactor;
            float scaleY = scaleFactor;
            float scaleZ = scaleFactor;
            float downWayX = _cardRectTransform.position.x;
            float downWayY = _cardRectTransform.rect.height / 2 * scaleFactor + indent;
            float downWayZ = _cardRectTransform.position.z;
            Vector3 downWay = new Vector3(downWayX, downWayY, downWayZ);

            _cardMovement.TranslateSmoothly(downWay, endRotationVector, duration, scaleX, scaleY, scaleZ);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardFront();
            DefineCardBack();
            //DefineCardDragAndDrop();
        }

        [ContextMenu(nameof(DefineCardFront))]
        private void DefineCardFront()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardFront, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardBack))]
        private void DefineCardBack()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardBack, ComponentLocationTypes.InChildren);
        }

        //[ContextMenu(nameof(DefineCardDragAndDrop))]
        //private void DefineCardDragAndDrop()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _cardDragAndDrop, ComponentLocationTypes.InChildren);
        //}
    }
}
