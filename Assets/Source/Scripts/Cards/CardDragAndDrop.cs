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
        private bool _isForciblyDrag;
        private Transform _cardTransform;

        private CardDragAndDropActions _cardDragAndDropActions;
        private Transform _container;
        private Transform _defaultParent;

        internal void Init(Transform cardTransform, CardDragAndDropActions cardDragAndDropActions, Transform container)
        {
            _container = container;
            _cardTransform = cardTransform;
            _cardDragAndDropActions = cardDragAndDropActions;
            _isDrag = false;
            _isAlreadyDrag = false;
            _isForciblyDrag = false;
        }

        public void BlockDrag()
        {
            _isDrag = false;

            _cardTransform.SetParent(_defaultParent);
            _cardDragAndDropActions.ReturnInHand(_returnInHandSpeed);
            _isForciblyDrag = true;
        }

        //public void BlockDrag()
        //{
        //    Debug.Log("222222. _isDrag = " + _isDrag + ", _isAlreadyDrag = " + _isAlreadyDrag);

        //    if (_isDrag == false || _isAlreadyDrag == true)
        //    {
        //        return;
        //    }

        //    OnEndDrag(_eventData);
        //}

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isDrag)
            {
                _isAlreadyDrag = true;
                return;
            }

            _isDrag = true;
            _isForciblyDrag = false;

            _defaultParent = _cardTransform.parent;
            _cardTransform.SetParent(_container);
            _cardDragAndDropActions.StartDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDrag == false || _isAlreadyDrag == true)
            {
                return;
            }

            //if (_isForciblyDrag)
            //{
            //    Debug.Log(eventData.position);
            //    eventData.position = _cardTransform.position;
            //    return;
            //}

            _cardTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isForciblyDrag)
            {
                //eventData.position = _cardTransform.position;
                _isDrag = false;
                this.enabled = false;

                return;
            }
            //_isForciblyDrag = false;

            if (_isAlreadyDrag)
            {
                _isAlreadyDrag = false;
                return;
            }

            if (EventSystem.current.TryGetComponentInRaycasts(eventData, out ICardDropPlace cardDropPlace))
            {
                if (_cardDragAndDropActions.TryDrop(cardDropPlace))
                {
                    _isDrag = false;
                    _cardDragAndDropActions.PlayCard();
                    return;
                }
            }

            _cardDragAndDropActions.EndDrag();
            _cardTransform.SetParent(_defaultParent);

            if (_viewCardAfterDropInWork != null)
            {
                StopCoroutine(_viewCardAfterDropInWork);
            }

            _viewCardAfterDropInWork = StartCoroutine(ViewCardAfterDrop(_returnInHandSpeed, eventData));
            _cardDragAndDropActions.ReturnInHand(_returnInHandSpeed);
        }

        private IEnumerator ViewCardAfterDrop(float endDuration, PointerEventData eventData)
        {
            yield return new WaitForSeconds(endDuration);

            _isDrag = false;

            EventSystem.current.TryGetComponentInRaycasts(eventData, out CardDragAndDrop cardDragAndDrop);

            _cardDragAndDropActions.OnReturnInHand(cardDragAndDrop == this);
        }
    }
}
