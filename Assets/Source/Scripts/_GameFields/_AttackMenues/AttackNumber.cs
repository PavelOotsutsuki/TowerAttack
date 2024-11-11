using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumber : SelectableButton
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _text;

        private IActivatable _attackButton;

        public void Init(int number, Vector3 position, Vector2 size, IActivatable attackButton)
        {
            base.Init();

            _rectTransform.sizeDelta = size;
            _rectTransform.SetLocalPositionAndRotation(position, Quaternion.identity);
            _text.text = number.ToString();

            _attackButton = attackButton;
        }

        //public override void Activate()
        //{
        //    base.Activate();
        //}

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            _attackButton.Activate();
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumber))]
        public override void DefineAllComponents()
        {
            DefineRectTransform();
            DefineText();
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
