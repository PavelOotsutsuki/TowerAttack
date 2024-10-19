using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackMenu : MonoBehaviour
    {
        [SerializeField] private AttackMenuLabel _attackMenuLabel;
        [SerializeField] private AttackMenuPanel _attackMenuPanel;
        [SerializeField] private AttackButton _attackButton;
        [SerializeField] private AttackNumberPanel _attackNumberPanel;

        public void Init()
        {
            gameObject.SetActive(false);

            _attackMenuLabel.Init();
            _attackMenuPanel.Init();
            _attackButton.Init();
            _attackNumberPanel.Init(_attackButton);
        }

        public void Activate()
        {
            gameObject.SetActive(true);

            _attackMenuLabel.Activate("Выберете кого атакуем");
            _attackMenuPanel.Activate();
            //_attackButton.Activate();
            _attackNumberPanel.Activate();
        }

        public void Deactivate()
        {
            _attackMenuLabel.Deactivate();
            _attackNumberPanel.Deactivate();
            _attackButton.Deactivate();
            _attackMenuPanel.Deactivate(() => gameObject.SetActive(false));
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineAttackMenuLabel();
            DefineAttackMenuPanel();
            DefineAttackButton();
            DefineAttackNumberPanel();
        }

        [ContextMenu(nameof(DefineAttackMenuLabel))]
        private void DefineAttackMenuLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackMenuPanel))]
        private void DefineAttackMenuPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackMenuPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackButton))]
        private void DefineAttackButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackNumberPanel))]
        private void DefineAttackNumberPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackNumberPanel, ComponentLocationTypes.InChildren);
        }

        #endregion 
    }
}
