using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools
{
    public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Transform _targetTransform;
        private IDragAndDropActions _dragAndDropActions;
        private Transform _container;

        private Transform _defaultParent;

        public void Init(Transform targetTransform, IDragAndDropActions dragAndDropActions, Transform container)
        {
            _container = container;
            _targetTransform = targetTransform;
            _dragAndDropActions = dragAndDropActions;
            //_isForciblyDrag = false;
            //IsDragable = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //_currentEventData = eventData;

            //if (IsDragable)
            //{
            //    return;
            //}

            //IsDragable = true;
            //_isForciblyDrag = false;

            _defaultParent = _targetTransform.parent;
            _targetTransform.SetParent(_container);
            _dragAndDropActions.StartDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            //_currentEventData = eventData;

            //if (IsDragable == false)
            //{
            //    return;
            //}

            _targetTransform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //if (_isForciblyDrag)
            //{
            //    IsDragable = false;
            //    _isForciblyDrag = false;

            //    return;
            //}

            //if (EventSystem.current.TryGetComponentInRaycasts(eventData, out T dropPlace))
            //{
            //if (_cardDragAndDropActions.TryDrop(cardDropPlace))
            //{
            //    IsDragable = false;
            //    _cardDragAndDropActions.PlayCard();
            //    return;
            //}
            //}

            _targetTransform.parent = _defaultParent;

            if (_dragAndDropActions.TryDrop())
            {
                _dragAndDropActions.Drop();
            }

            //enabled = false;
            _dragAndDropActions.StartEndDragActions();

            //StartEndDragActions();
        }
    }
}
