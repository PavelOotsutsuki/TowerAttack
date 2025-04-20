using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Persons.Towers;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackMenuImitation_Old : AttackMenu_OLD
    {
        [SerializeField] private AttackNumberPanelEnemyAI _attackNumberPanelEnemyAI;
        [SerializeField] private AttackMenuImitationData _data;

        public void Init(ICardNumberKeeper cardNumberKeeper, ISelectResultHandler attackResultHandler, int countNumbers,
            ConfirmableNumbers confirmableNumbers)
        {
            _attackNumberPanelEnemyAI.Init(cardNumberKeeper, countNumbers, confirmableNumbers);

            base.Init(attackResultHandler, _data, _attackNumberPanelEnemyAI);
        }

        public override void Activate(AttackMenuActivateData activateData)
        {
            base.Activate(activateData);

            StartCoroutine(WaitingUntilDeactivate());
        }

        protected override IEnumerator OnDeactivating()
        {
            yield return new WaitForSeconds(_data.DelayAfterChoiceNumberDone);

            _attackNumberPanelEnemyAI.Deactivate();
        }

        private IEnumerator WaitingUntilDeactivate()
        {
            yield return new WaitUntil(() => _attackNumberPanelEnemyAI.IsComplete);

            Deactivate();
        }


        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackMenuImitation))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAttackNumberPanel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineAttackNumberPanel))]
        private ComponentAttachInfo DefineAttackNumberPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _attackNumberPanelEnemyAI, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}