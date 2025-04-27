using System;
using System.Collections.Generic;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectNumber : SelectableButton, ISelectNumber, IActivatable<SelectNumberActivateData>
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Color _errorColor;
        [SerializeField] private Color _successColor;
        [SerializeField] private SelectNumberAnimator _animator;

        private Action<bool> _clickCallback;
        private Color _choiceColor;

        //private Color? _blockColor;
        private Dictionary<NumberAnimationType, Color> _blockColors;

        public int Number { get; private set; }
        public float AnimationDuration => _animator.AnimationDuration;

        public void Init(int number, Vector3 position, Vector2 size, Action<bool> clickCallback)
        {
            base.Init();

            Number = number;

            _choiceColor = _errorColor;
            //_blockColor = NormalColor;

            _blockColors = new Dictionary<NumberAnimationType, Color>()
            {
                { NumberAnimationType.Error, _errorColor },
                { NumberAnimationType.Success, _successColor },
                { NumberAnimationType.Choice, _choiceColor }
            };

            _rectTransform.sizeDelta = size;
            _rectTransform.SetLocalPositionAndRotation(position, Quaternion.identity);
            _text.text = Number.ToString();

            _animator.Init();

            _clickCallback = clickCallback;
        }

        public void Activate(SelectNumberActivateData data)
        {
            base.Activate();

            if (data.NumberAnimationType is not null)
            {
                SetDisableView(_blockColors[data.NumberAnimationType.Value]);
            }

            _animator.Activate(data.SelectNumberAnimatorActivateData);
        }

        public override void Deactivate()
        {
            base.Deactivate();

            _animator.Deactivate();
        }

        public void SetChoice(NumberAnimationType numberAnimationType)
        {
            SetDisableView(_blockColors[numberAnimationType]);

            _animator.PlayAnimation(numberAnimationType);

            //switch (numberAnimationType)
            //{
            //    case NumberAnimationType.Success:
            //        break;
            //    case NumberAnimationType.Error:
            //        break;
            //    case NumberAnimationType.Choice:
            //        break;
            //    default:
            //        throw new NullReferenceException("Неизвестный NumberAnimationType: " + numberAnimationType);
            //}
        }

        //public void SuccessChoice()
        //{
        //    SetDisableView(_successColor);
        //    //_confirmableNumbers.Add(this);

        //    _animator.PlaySuccessAnimation();
        //}

        //public void ErrorChoice()
        //{
        //    SetDisableView(_errorColor);
        //    //_confirmableNumbers.Add(this);

        //    _animator.PlayErrorAnimation();
        //}

        private void SetDisableView(Color color)
        {
            //_blockColor = color;

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

        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectNumber))]
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