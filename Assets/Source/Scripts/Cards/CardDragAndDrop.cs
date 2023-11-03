using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Cards
{
    internal class CardDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private float _returnInHandSpeed;

        private Transform _cardTransform;
        private CardFront _cardFront;
        private Vector2 _dragPosition;
        private Vector3 _dragRotation;
        private int _siblingIndex;
        private Coroutine _viewCardAfterDropInWork;
        private bool _isDrag;
        private bool _isAlreadyDrag;

        internal void Init(RectTransform cardTransform, CardFront cardFront)
        {
            _isDrag = false;
            _isAlreadyDrag = false;
            _cardFront = cardFront;

            _cardTransform = cardTransform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isDrag)
            {
                _isAlreadyDrag = true;
                return;
            }

            _cardFront.OnPointerExit(eventData);
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

            _cardFront.DisableRaycasts();
            _cardTransform.SetSiblingIndex(_siblingIndex);

            if (_viewCardAfterDropInWork != null)
            {
                StopCoroutine(_viewCardAfterDropInWork);
            }

            _viewCardAfterDropInWork = StartCoroutine(ViewCardAfterDrop(_returnInHandSpeed, eventData));
            _cardFront.TranslateInto(_dragPosition, _dragRotation, _returnInHandSpeed);
            _cardFront.EnableRaycasts();
        }

        private void ActivateStartDragOptions()
        {
            _isDrag = true;
            _cardFront.Block();
        }

        private void ActivateEndDragOptions()
        {
            _isDrag = false;
            _cardFront.Unblock();
        }

        private IEnumerator ViewCardAfterDrop(float endDuration, PointerEventData eventData)
        {
            for (float duration = 0; duration < endDuration; duration += Time.deltaTime)
            {
                yield return true;
            }

            ActivateEndDragOptions();

            if (eventData.pointerEnter != null)
            {
                if (eventData.pointerEnter.TryGetComponent(out CardDragAndDrop cardDragAndDrop))
                {
                    if (cardDragAndDrop == this)
                    {
                        _cardFront.OnPointerEnter(eventData);
                    }
                }
            }
        }

        private void DefineDragSiblingIndex()
        {
            _siblingIndex = _cardTransform.GetSiblingIndex();
            _cardTransform.SetAsLastSibling();
        }

        private void DefineDragTransformValues()
        {
            _dragPosition = _cardTransform.position;
            _dragRotation = _cardTransform.rotation.eulerAngles;
            _cardTransform.rotation = Quaternion.identity;
        }
    }
}
