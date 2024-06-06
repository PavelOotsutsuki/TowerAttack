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

        private PointerEventData _currentEventData;

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
            //Debug.Log(_cardTransform.gameObject.name + ": начало Block");
            //_isDrag = false;

            //_cardTransform.SetParent(_defaultParent);
            //_cardDragAndDropActions.ReturnInHand(_returnInHandSpeed);

            if (_currentEventData is not null)
            {
                _currentEventData?.Reset();
                _isForciblyDrag = true;

                StartEndDragActions();

            }
            //_isForciblyDrag = true;
            //Debug.Log(_cardTransform.gameObject.name + ": конец Block");
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
            _currentEventData = eventData;

            if (_isDrag)
            {
                _isAlreadyDrag = true;
                return;
            }

            //if (_isForciblyDrag)
            //{
            //    return;
            //}

            //Debug.Log(_cardTransform.gameObject.name + ": начало OnBeginDrag");
            _isDrag = true;
            _isForciblyDrag = false;

            _defaultParent = _cardTransform.parent;
            _cardTransform.SetParent(_container);
            _cardDragAndDropActions.StartDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _currentEventData = eventData;

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

            //Debug.Log(_cardTransform.gameObject.name + ": идет Drag");

            _cardTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log(_cardTransform.gameObject.name + ": начало OnEndDrag");

            //if (_isForciblyDrag)
            //{
            //    //eventData.position = _cardTransform.position;
            //    _isDrag = false;
            //    _isForciblyDrag = false;

            //    //this.enabled = false;
            //    //_cardTransform.gameObject.GetComponent<Card>().SetActiveInteraction(false);
            //    Debug.Log(_cardTransform.gameObject.name + ": начало OnEndDrag (_isForciblyDrag)");

            //    return;
            //}


            if (_isForciblyDrag)
            {
                _isDrag = false;
                _isForciblyDrag = false;

                return;
            }

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

            StartEndDragActions();
        }

        private void StartEndDragActions()
        {
            _cardDragAndDropActions.EndDrag();
            _cardTransform.SetParent(_defaultParent);

            if (_viewCardAfterDropInWork != null)
            {
                StopCoroutine(_viewCardAfterDropInWork);
            }

            _viewCardAfterDropInWork = StartCoroutine(ViewCardAfterDrop(_returnInHandSpeed, _currentEventData));
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
