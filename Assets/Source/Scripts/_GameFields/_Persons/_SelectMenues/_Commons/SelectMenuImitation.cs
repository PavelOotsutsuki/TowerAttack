using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Towers;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectMenuImitation : SelectMenu
    {
        [SerializeField] private SelectNumberPanelEnemyAI _selectNumberPanelEnemyAI;
        [SerializeField] private SelectMenuImitationData _data;

        public void Init(ICardNumberKeeper cardNumberKeeper, ISelectResultHandler attackResultHandler, int countNumbers,
            SelectNumbersList selectedNumbers)
        {
            _selectNumberPanelEnemyAI.Init(cardNumberKeeper, countNumbers, selectedNumbers);

            base.Init(attackResultHandler, _data, _selectNumberPanelEnemyAI);
        }

        public override void Activate(SelectMenuActivateData activateData)
        {
            base.Activate(activateData);

            StartCoroutine(WaitingUntilDeactivate());
        }

        protected override IEnumerator OnDeactivating()
        {
            yield return new WaitForSeconds(_data.DelayAfterChoiceNumberDone);

            _selectNumberPanelEnemyAI.Deactivate();
        }

        private IEnumerator WaitingUntilDeactivate()
        {
            yield return new WaitUntil(() => _selectNumberPanelEnemyAI.IsComplete);

            Deactivate();
        }


        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectMenuImitation))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineSelectNumberPanel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineSelectNumberPanel))]
        private ComponentAttachInfo DefineSelectNumberPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _selectNumberPanelEnemyAI, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}