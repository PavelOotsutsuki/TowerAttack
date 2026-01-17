using System;
using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menues
{
    public class GoBackOnMainPanelButton : Button, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _scaleFactor = 1.2f;
        [SerializeField] private float _focusInDuration = 1f;
        [SerializeField] private float _focusOutDuration = 0.5f;

        private Vector3 _defaultScale;
        private Movement _movement;
        private ReadOnlyTransform _ROTransform;
        private Action _onEnterClick;

        public void Init(Action onEnterClick)
        {
            _onEnterClick = onEnterClick;

            _movement = new Movement(_transform);
            _ROTransform = new ReadOnlyTransform(_transform);
            _defaultScale = _ROTransform.GetLocalScale();
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);

            OnClick();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            OnClick();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            OnEnter();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            OnExit();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            OnEnter();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            OnExit();
        }

        private void OnClick()
        {
            OnExit();
            _onEnterClick?.Invoke();
        }

        private void OnEnter()
        {
            _movement.Stop();
            _movement.MoveLocalSmoothly(_ROTransform.GetLocalPosition(), _ROTransform.GetRotationVector(), _focusInDuration, _defaultScale * _scaleFactor);
        }

        private void OnExit()
        {
            _movement.Stop();
            _movement.MoveLocalSmoothly(_ROTransform.GetLocalPosition(), _ROTransform.GetRotationVector(), _focusOutDuration, _defaultScale);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(GoBackOnMainPanelButton))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}