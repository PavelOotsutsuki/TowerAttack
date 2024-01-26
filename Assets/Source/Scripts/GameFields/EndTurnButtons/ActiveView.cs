using System;
using Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameFields.EndTurnButtons
{
    public class ActiveView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        //public event Action Clicked;

        //private void OnClick()
        //{
        //    Clicked?.Invoke();
        //}

        //private void OnEnable()
        //{
        //    _button.onClick.AddListener(OnClick);
        //}

        //private void OnDisable()
        //{
        //    _button.onClick.RemoveListener(OnClick);
        //}
        public void AddOnClickListener(UnityAction action)
        {
            _button.onClick.AddListener(action);
        }

        public void RemoveOnClickListener(UnityAction action)
        {
            _button.onClick.RemoveListener(action);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineButton();
        }

        [ContextMenu(nameof(DefineButton))]
        private void DefineButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _button, ComponentLocationTypes.InThis);
        }
    }
}
