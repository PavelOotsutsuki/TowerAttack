using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Tools;

namespace Cards
{
    internal class CardDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private float _returnInHandSpeed;

        private Coroutine _viewCardAfterDropInWork;
        private bool _isDrag;
        private bool _isAlreadyDrag;
        private Vector2 _dragPosition;
        private Vector3 _dragRotation;
        private int _siblingIndex;
        private Transform _cardTransform;

        private CardDragAndDropActions _cardDragAndDropActions;

        internal void Init(Transform cardTransform, CardDragAndDropActions cardDragAndDropActions)
        {
            _cardTransform = cardTransform;
            _cardDragAndDropActions = cardDragAndDropActions;
            _isDrag = false;
            _isAlreadyDrag = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isDrag)
            {
                _isAlreadyDrag = true;
                return;
            }

            _cardDragAndDropActions.StartDrag();

            ActivateStartDragOptions();

            DefineDragTransformValues();
            DefineDragSiblingIndex();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDrag == false)
            {
                return;
            }

            _cardTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isAlreadyDrag)
            {
                _isAlreadyDrag = false;
                return;
            }

            _cardTransform.SetSiblingIndex(_siblingIndex);

            if (_viewCardAfterDropInWork != null)
            {
                StopCoroutine(_viewCardAfterDropInWork);
            }

            _viewCardAfterDropInWork = StartCoroutine(ViewCardAfterDrop(_returnInHandSpeed, eventData));
            _cardDragAndDropActions.ReturnInHand(_dragPosition, _dragRotation, _returnInHandSpeed);
        }

        private void ActivateStartDragOptions()
        {
            _isDrag = true;
            _cardDragAndDropActions.BlockReview();
        }

        private IEnumerator ViewCardAfterDrop(float endDuration, PointerEventData eventData)
        {
            for (float duration = 0; duration < endDuration; duration += Time.deltaTime)
            {
                yield return true;
            }

            _isDrag = false;

            EventSystem.current.TryGetComponentInRaycasts(eventData, out CardDragAndDrop cardDragAndDrop);
            
            _cardDragAndDropActions.FinishDrag(cardDragAndDrop == this);
        }

        private void DefineDragTransformValues()
        {
            _dragPosition = _cardTransform.position;
            _dragRotation = _cardTransform.rotation.eulerAngles;
            _cardTransform.rotation = Quaternion.identity;
        }

        private void DefineDragSiblingIndex()
        {
            _siblingIndex = _cardTransform.GetSiblingIndex();
            _cardTransform.SetAsLastSibling();
        }
    }
}
