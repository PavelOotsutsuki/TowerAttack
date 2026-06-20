using System;
using System.Collections.Generic;
using Tools.UI;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI
{
    public class ConfirmableFocusableButton : ConfirmableButton
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _scaleFactor = 1.2f;
        [SerializeField] private float _focusInDuration = 1f;
        [SerializeField] private float _focusOutDuration = 0.5f;

        private Vector3 _defaultScale;
        private IFocusCustomButtonWatcher _focusWatcher;
        private Movement _movement;
        private ReadOnlyTransform _ROTransform;
        private Action _onEnterClick;

        public virtual void Init(IFocusCustomButtonWatcher focusWatcher, Action onEnterClick) 
        {
            _focusWatcher = focusWatcher;
            _onEnterClick = onEnterClick;

            _movement = new Movement(_transform);
            _ROTransform = new ReadOnlyTransform(_transform);
            _defaultScale = _ROTransform.GetLocalScale();

            base.Init();
        }

        protected override void OnEnterClick()
        {
            _onEnterClick?.Invoke();
        }

        protected override void OnEnter()
        {
            base.OnEnter();

            OnFocus();
        }

        protected override void OnExit()
        {
            base.OnExit();

            OnUnfocus();
        }

        private void OnFocus()
        {
            _focusWatcher.SetFocused(this);
            _movement.Stop();
            _movement.MoveLocalSmoothly(_ROTransform.GetLocalPosition(), _ROTransform.GetRotationVector(), _focusInDuration, _defaultScale * _scaleFactor);
        }

        private void OnUnfocus()
        {
            _movement.Stop();
            _movement.MoveLocalSmoothly(_ROTransform.GetLocalPosition(), _ROTransform.GetRotationVector(), _focusOutDuration, _defaultScale);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(ConfirmableFocusableButton))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTransform()
            };

            list.AddRange(base.DefineAllComponents());

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