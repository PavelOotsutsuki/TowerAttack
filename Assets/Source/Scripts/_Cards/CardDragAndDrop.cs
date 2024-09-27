using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Tools;

namespace Cards
{
    internal class CardDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private float _returnInHandSpeed = 0.5f;

        private Coroutine _viewCardAfterDropInWork;
        private bool _isForciblyDrag;
        private bool _isNotDraggable;
        private Transform _cardTransform;

        private CardDragAndDropActions _cardDragAndDropActions;
        private Transform _container;
        private Transform _defaultParent;

        private PointerEventData _currentEventData;

        public bool IsDragable { get; private set; }

        internal void Init(Transform cardTransform, CardDragAndDropActions cardDragAndDropActions, Transform container)
        {
            _container = container;
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
            
            _currentEventData?.Reset();
            _isForciblyDrag = true;

            StartEndDragActions();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_cardDragAndDropActions.IsCanDrag() == false)
            {
                _isNotDraggable = true;
                _currentEventData?.Reset();
                return;
            }

            _currentEventData = eventData;

            if (IsDragable)
            {
                return;
            }

            IsDragable = true;
            _isForciblyDrag = false;

            _defaultParent = _cardTransform.parent;
            _cardTransform.SetParent(_container);
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
                if (_cardDragAndDropActions.IsCanDrop(cardDropPlace))
                {
                    IsDragable = false;
                    _cardDragAndDropActions.PlayCard(cardDropPlace);
                    return;
                }
            }

            enabled = false;
            _cardDragAndDropActions.StartEndDrag();

            StartEndDragActions();
        }

        private void StartEndDragActions()
        {
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

            IsDragable = false;

            EventSystem.current.TryGetComponentInRaycasts(eventData, out CardDragAndDrop cardDragAndDrop);

            _cardDragAndDropActions.OnReturnInHand(cardDragAndDrop == this);
        }
    }
}
