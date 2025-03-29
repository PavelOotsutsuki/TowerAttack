using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Tools.Extensions;
using Tools;

namespace Cards
{
    internal class CardDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Coroutine _viewCardAfterDropInWork;
        private bool _isForciblyDrag;
        private bool _isNotDraggable;
        private Transform _cardTransform;

        private CardDragAndDropActions _cardDragAndDropActions;
        //private Transform _defaultParent; ///IPS

        private PointerEventData _currentEventData;

        public bool IsDragable { get; private set; }

        internal void Init(Transform cardTransform, CardDragAndDropActions cardDragAndDropActions)
        {
            _cardTransform = cardTransform;
            _cardDragAndDropActions = cardDragAndDropActions;
            _isForciblyDrag = false;
            IsDragable = false;
            _isNotDraggable = false;
        }

        public void BlockDrag()
        {
            if (_currentEventData is null)
                return;

            if (_currentEventData != null)
            {
                Debug.Log("Reset!");
                _currentEventData.Reset();
            }
            //_currentEventData?.Reset();

            _isForciblyDrag = true;

            StartEndDragActions();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_cardDragAndDropActions.CanDrag() == false)
            {
                _isNotDraggable = true;

                if (_currentEventData != null)
                {
                    Debug.Log("Reset!");
                    _currentEventData.Reset();
                }
                //_currentEventData?.Reset();
                return;
            }

            _currentEventData = eventData;

            if (IsDragable)
            {
                return;
            }

            IsDragable = true;
            _isForciblyDrag = false;

            //_defaultParent = _cardTransform.parent; ///IPS
            //_cardTransform.SetParent(_container); ///IPS
            _cardDragAndDropActions.StartDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _currentEventData = eventData;

            if (IsDragable == false)
            {
                return;
            }
            
            _cardTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isNotDraggable)
            {
                _isNotDraggable = false;
                return;
            }

            if (_isForciblyDrag)
            {
                IsDragable = false;
                _isForciblyDrag = false;

                return;
            }

            if (EventSystem.current.TryGetComponentInRaycasts(eventData, out ICardDropPlace cardDropPlace))
            {
                if (_cardDragAndDropActions.CanDrop(cardDropPlace))
                {
                    IsDragable = false;
                    _cardDragAndDropActions.PlayCard(cardDropPlace);
                    return;
                }
            }

            if (EventSystem.current.TryGetComponentInRaycasts(eventData, out IAttackable cardAttackZone))
            {

                IsDragable = false;
                enabled = false;
                _cardDragAndDropActions.Attack(cardAttackZone);
                return;
            }

            enabled = false;
            _cardDragAndDropActions.StartEndDrag();

            StartEndDragActions();
        }

        private void StartEndDragActions()
        {
            //_cardTransform.SetParent(_defaultParent); ///IPS

            if (_viewCardAfterDropInWork != null)
            {
                StopCoroutine(_viewCardAfterDropInWork);
            }

            _viewCardAfterDropInWork = StartCoroutine(ViewCardAfterDrop(_cardDragAndDropActions.ReturnInHandDuration, _currentEventData));
            _cardDragAndDropActions.ReturnInHand(_cardDragAndDropActions.ReturnInHandDuration);
        }

        private IEnumerator ViewCardAfterDrop(float endDuration, PointerEventData eventData)
        {
            yield return new WaitForSeconds(endDuration);

            IsDragable = false;

            EventSystem.current.TryGetComponentInRaycasts(eventData, out CardDragAndDrop cardDragAndDrop);

            _cardDragAndDropActions.OnReturnInHand(cardDragAndDrop == this);
        }
    }
}