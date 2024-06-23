using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Tools;

namespace Cards
{
    internal class CardDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private float _returnInHandDuration = 0.5f;

        private bool _disabled;
        private Coroutine _viewCardAfterDropInWork;
        private Transform _container;
        private Transform _cardTransform;
        private Transform _defaultParent;
        private PointerEventData _currentEventData;
        private CardDragAndDropActions _cardDragAndDropActions;

        public bool InDrag { get; private set; }

        internal void Init(Transform cardTransform, CardDragAndDropActions cardDragAndDropActions, Transform container)
        {
            _disabled = true;
            _container = container;
            _cardTransform = cardTransform;
            _cardDragAndDropActions = cardDragAndDropActions;
            InDrag = false;
        }

        public void Enable() => _disabled = false;

        public void Disable() => _disabled = true;

        public void ForceBlockDrag()
        {
            Disable();

            if (_currentEventData is null)
                return;
            
            ResetDragObject();

            StartEndDragActions();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_disabled)
            {
                Debug.Log($"{nameof(OnBeginDrag)} for {name}. Dragged object: {eventData.pointerDrag.name}");
                ResetDragObject();
                return;
            }
            
            Debug.Log($"{nameof(OnBeginDrag)} for {name}");
            _currentEventData = eventData;

            if (InDrag)
            {
                return;
            }

            InDrag = true;

            _cardTransform.SetParent(_container);
            _cardDragAndDropActions.StartDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(_disabled)
            {
                Debug.Log($"{nameof(OnDrag)} for {name}. Dragged object: {eventData.pointerDrag.name}");
                ResetDragObject();
                return;
            }
            
            Debug.Log($"{nameof(OnDrag)} for {name}");
            _currentEventData = eventData;

            if (InDrag == false)
            {
                return;
            }

            _cardTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_disabled)
            {
                ResetDragObject();
                return;
            }
            
            Debug.Log($"{nameof(OnEndDrag)} for {name}");
            if (EventSystem.current.TryGetComponentInRaycasts(eventData, out ICardDropPlace cardDropPlace))
            {
                if (_cardDragAndDropActions.TryDrop(cardDropPlace))
                {
                    InDrag = false;
                    _cardDragAndDropActions.PlayCard();
                    return;
                }
            }

            _cardDragAndDropActions.EndDrag();

            StartEndDragActions();
        }

        private void ResetDragObject()
        {
            if (_currentEventData.pointerDrag == gameObject)
                _currentEventData.pointerDrag = null;
        }

        private void StartEndDragActions()
        {
            // if (_defaultParent == null)
            //     return;

            _cardTransform.SetParent(_defaultParent);

            if (_viewCardAfterDropInWork != null)
            {
                StopCoroutine(_viewCardAfterDropInWork);
            }

            _viewCardAfterDropInWork = StartCoroutine(ViewCardAfterDrop(_returnInHandDuration, _currentEventData));
            _cardDragAndDropActions.ReturnInHand(_returnInHandDuration);
        }

        private IEnumerator ViewCardAfterDrop(float endDuration, PointerEventData eventData)
        {
            yield return new WaitForSeconds(endDuration);

            InDrag = false;

            EventSystem.current.TryGetComponentInRaycasts(eventData, out CardDragAndDrop cardDragAndDrop);

            _cardDragAndDropActions.OnReturnInHand(cardDragAndDrop == this);
        }

        public void BindParent(Transform container)
        {
            _defaultParent = container;
            transform.SetParent(_defaultParent);
        }
    }
}
