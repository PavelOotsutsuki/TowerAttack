using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Tools.Extensions;
using Cards.DependencyInterlayers;

namespace Cards.Insides
{
    internal class CardDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Coroutine _viewCardAfterDropInWork;
        private bool _isForciblyDrag;
        private bool _isNotDraggable;
        private Transform _cardTransform;

        private CardDragAndDropActions _cardDragAndDropActions;

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
                return;
            }

            _currentEventData = eventData;

            if (IsDragable)
            {
                return;
            }

            IsDragable = true;
            _isForciblyDrag = false;

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
            if (IsDragable == false)
                return;

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

            if (_cardDragAndDropActions.IsPlayable() && EventSystem.current.TryGetComponentInRaycasts(eventData, out ICardDropPlace cardDropPlace))
            {
                if (_cardDragAndDropActions.CanDrop(cardDropPlace))
                {
                    IsDragable = false;
                    _cardDragAndDropActions.PlayCard();
                    return;
                }
            }

            if (_cardDragAndDropActions.IsAttackable() && EventSystem.current.TryGetComponentInRaycasts(eventData, out IPlayerAttackable cardAttackZone))
            {
                IsDragable = false;
                enabled = false;
                _cardDragAndDropActions.Attack(cardAttackZone);
                return;
            }

            if (_cardDragAndDropActions.IsForgable() && EventSystem.current.TryGetComponentInRaycasts(eventData, out IForging forgingZone))
            {
                IsDragable = false;
                enabled = false;
                _cardDragAndDropActions.StartForging(forgingZone);
                return;
            }

            if (_cardDragAndDropActions.IsHandTransferable() && EventSystem.current.TryGetComponentInRaycasts(eventData, out IHandTransferable handTransferZone))
            {
                IsDragable = false;
                enabled = false;
                _cardDragAndDropActions.StartHandTransfing(handTransferZone);
                return;
            }

            enabled = false;
            _cardDragAndDropActions.StartEndDrag();

            StartEndDragActions();
        }

        private void StartEndDragActions()
        {
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