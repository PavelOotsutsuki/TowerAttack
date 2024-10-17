using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using Tools.UI.Buttons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            _attackButton.Activate();
        }

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
