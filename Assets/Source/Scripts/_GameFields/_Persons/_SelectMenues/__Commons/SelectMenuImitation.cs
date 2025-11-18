using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Commons;
using GameFields.Persons.Towers;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Commons
{
    public class SelectMenuImitation : SelectMenu
    {
        [SerializeField] private SelectNumberPanelImitation _selectNumberPanelImitation;
        [SerializeField] private SelectMenuImitationData _data;

        public void Init(ICardNumberKeeper cardNumberKeeper, ISelectResultHandler selectResultHandler, int[] cardNumbers,
            SelectNumbersList selectedNumbers, ConfirmableNumbers confirmableNumbers)
        {
            _selectNumberPanelImitation.Init(cardNumberKeeper, cardNumbers, selectedNumbers, confirmableNumbers);

            SelectMenuLabelTextLogic selectMenuLabelTextLogic = new EnemySelectMenuLabelTextLogic(_data.SelectMenuLabelText);

            base.Init(selectResultHandler, _selectNumberPanelImitation, selectMenuLabelTextLogic);
        }

        public override void Activate(SelectMenuActivateData activateData)
        {
            base.Activate(activateData);

            StartCoroutine(WaitingUntilDeactivate());
        }

        protected override IEnumerator OnDeactivating()
        {
            yield return new WaitForSeconds(_data.DelayAfterChoiceNumberDone);

            _selectNumberPanelImitation.Deactivate();
        }

        private IEnumerator WaitingUntilDeactivate()
        {
            yield return new WaitUntil(() => _selectNumberPanelImitation.IsComplete);

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
            return AutomaticFillComponents.DefineComponent(this, ref _selectNumberPanelImitation, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}