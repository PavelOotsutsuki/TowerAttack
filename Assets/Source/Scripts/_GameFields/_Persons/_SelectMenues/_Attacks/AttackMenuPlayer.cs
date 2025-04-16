using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Towers;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackMenuPlayer : AttackMenu
    {
        [SerializeField] private AttackNumberPanelPlayer _attackNumberPanelPlayer;
        [SerializeField] private AttackMenuPlayerData _data;
        [SerializeField] private AttackButton _attackButton;

        public void Init(ICardNumberKeeper cardNumberKeeper, IAttackResultHandler attackResultHandler, int countNumbers,
            ConfirmableNumbers confirmableNumbers)
        {
            _attackButton.Init(this);
            _attackNumberPanelPlayer.Init(_attackButton, cardNumberKeeper, countNumbers, confirmableNumbers);

            base.Init(attackResultHandler, _data, _attackNumberPanelPlayer);
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
            _attackNumberPanelPlayer.Deactivate();

            yield return new WaitUntil(() => _attackNumberPanelPlayer.IsCompleteNumbersHide);
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
            return AutomaticFillComponents.DefineComponent(this, ref _attackNumberPanelPlayer, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}