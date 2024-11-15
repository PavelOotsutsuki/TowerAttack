using System;
using TMPro;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumber : SelectableButton
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Animator _animator;

        private Action<bool> _clickCallback;

        public void Init(int number, Vector3 position, Vector2 size, Action<bool> clickCallback)
        {
            base.Init();

            _rectTransform.sizeDelta = size;
            _rectTransform.SetLocalPositionAndRotation(position, Quaternion.identity);
            _text.text = number.ToString();

            //_animator.gameObject.SetActive(false);

            _clickCallback = clickCallback;
        }

        //public override void Activate()
        //{
        //    base.Activate();
        //}

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            //_animator.Play("Cross");
            _animator.SetBool("IsActivate", true);
            //_animator.gameObject.SetActive(true);
            _clickCallback.Invoke(true);
        }

        protected override void OnExitClick()
        {
            base.OnExitClick();

            _animator.SetBool("IsActivate", false);
            //_animator.gameObject.SetActive(false);
            _clickCallback.Invoke(false);
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumber))]
        public override void DefineAllComponents()
        {
            DefineRectTransform();
            DefineText();
            DefineAnimator();

            base.DefineAllComponents();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineText))]
        private void DefineText()
        {
            AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InThisElseChildren);
        }

        [ContextMenu(nameof(DefineAnimator))]
        private void DefineAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _animator, ComponentLocationTypes.InChildren);
        }

        #endregion

        //public override void OnPointerClick(PointerEventData eventData)
        //{
        //    base.OnPointerClick(eventData);

        //    _attackButton.Activate();
        //}

        //public void Unsubscribe()
        //{
        //    _button.onClick.RemoveListener(_attackButton.Activate);
        //}

        //private void Subscribe()
        //{
        //    _button.onClick.AddListener(_attackButton.Activate);
        //}
    }
}
