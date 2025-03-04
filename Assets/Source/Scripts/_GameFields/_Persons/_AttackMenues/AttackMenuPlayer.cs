using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Towers;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackMenuPlayer : AttackMenu
    {
        [SerializeField] private AttackNumberPanelPlayer _attackNumberPanel;
        [SerializeField] private AttackMenuPlayerData _data;
        [SerializeField] private AttackButton _attackButton;

        public void Init(ICardNumberKeeper cardNumberKeeper, IAttackResultHandler attackResultHandler, int countNumbers)
        {
            _attackButton.Init(this);
            _attackNumberPanel.Init(_attackButton, cardNumberKeeper, countNumbers);

            base.Init(attackResultHandler, _data, _attackNumberPanel);
        }

        protected override List<ICompletable> FillCompletableElements()
        {
            List<ICompletable> completableElements = base.FillCompletableElements();

            completableElements.Add(_attackButton);

            return completableElements;
        }

        protected override IEnumerator OnDeactivating()
        {
            _attackButton.Deactivate();
            _attackNumberPanel.Deactivate();

            yield return new WaitUntil(() => _attackNumberPanel.IsCompleteNumbersHide);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackMenuPlayer))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAttackNumberPanel(),
                DefineAttackButton()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineAttackButton))]
        private ComponentAttachInfo DefineAttackButton()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _attackButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackNumberPanel))]
        private ComponentAttachInfo DefineAttackNumberPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _attackNumberPanel, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}