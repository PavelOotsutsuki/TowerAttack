using System;
using System.Collections.Generic;
using TMPro;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackNumber : SelectableButton, IAttackNumber
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Color _errorColor;
        [SerializeField] private Color _successColor;
        [SerializeField] private AttackNumberAnimator _animator;

        private Action<bool> _clickCallback;

        private Color? _blockColor;

        public int Number { get; private set; }
        public float AnimationDuration => _animator.AnimationDuration;

        public void Init(int number, Vector3 position, Vector2 size, Action<bool> clickCallback)
        {
            base.Init();

            Number = number;

            _blockColor = null;
            _rectTransform.sizeDelta = size;
            _rectTransform.SetLocalPositionAndRotation(position, Quaternion.identity);
            _text.text = Number.ToString();

            _animator.Init();

            _clickCallback = clickCallback;
        }

        public override void Activate()
        {
            base.Activate();

            if (_blockColor is not null)
            {
                SetDisableView(_blockColor.Value);
            }

            _animator.Activate();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public void SuccessChoice()
        {
            SetDisableView(_successColor);
            //_confirmableNumbers.Add(this);

            _animator.PlaySuccessAnimation();
        }

        public void ErrorChoice()
        {
            SetDisableView(_errorColor);
            //_confirmableNumbers.Add(this);

            _animator.PlayErrorAnimation();
        }

        private void SetDisableView(Color color)
        {
            _blockColor = color;

            Image.color = color;
            CanvasGroup.blocksRaycasts = false;
        }

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            _clickCallback.Invoke(true);
        }

        protected override void OnExitClick()
        {
            base.OnExitClick();

            _clickCallback.Invoke(false);
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumber))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
                DefineText(),
                DefineAttackNumberAnimator()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineText))]
        private ComponentAttachInfo DefineText()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InThisElseChildren);
        }

        [ContextMenu(nameof(DefineAttackNumberAnimator))]
        private ComponentAttachInfo DefineAttackNumberAnimator()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _animator, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}