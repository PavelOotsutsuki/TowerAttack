using System.Collections;
using System.Collections.Generic;
using GameFields.InputSettings;
using GameFields.Persons;
using GameFields.Persons.SelectMenues.Attacks;
using GameFields.Persons.SelectMenues.Choices;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.Persons.SelectMenues
{
    public class SelectMenuPlayer : SelectMenu, IInputLogicObject, IQPressHandler, IEnterPressHandler
    {
        [SerializeField] private SelectNumberPanelPlayer _selectNumberPanelPlayer;
        [SerializeField] private SelectButton _selectButton;
        [SerializeField] private SelectModeButton _selectModeButton;

        private int _currentNeedForActivate;
        private InputRoot _inputRoot;

        public void Init(ICardNumberKeeper cardNumberKeeper, ISelectResultHandler attackResultHandler, int[] cardNumbers,
            SelectNumbersList selectedNumbers, ConfirmableNumbers confirmableNumbers, InputRoot inputRoot)
        {
            _selectButton.Init(this, inputRoot);
            _selectNumberPanelPlayer.Init(_selectButton, cardNumberKeeper, cardNumbers, selectedNumbers, confirmableNumbers);
            _selectModeButton.Init(_selectNumberPanelPlayer);

            _inputRoot = inputRoot;

            SelectMenuLabelTextLogic selectMenuLabelTextLogic = new DefaultSelectMenuLabelTextLogic(GetNeedForActivate);

            base.Init(attackResultHandler, _selectNumberPanelPlayer, selectMenuLabelTextLogic);
        }

        //public IPointerClickHandler SelectModeButton => _selectModeButton;
        //public IPointerClickHandler SelectButton => _selectButton;

        public override void Activate(SelectMenuActivateData activateData)
        {
            if (IsActive == true)
                return;

            _inputRoot.SetInputType(this);

            _currentNeedForActivate = activateData.NeedSelect;

            base.Activate(activateData);

            _selectModeButton.Activate();
        }

        void IEnterPressHandler.OnEnter()
        {
            _selectButton.OnPointerClick(null);
        }

        void IQPressHandler.OnQ()
        {
            _selectModeButton.OnPointerClick(null);
        }

        protected override List<ICompletable> FillCompletableElements()
        {
            List<ICompletable> completableElements = base.FillCompletableElements();

            completableElements.Add(_selectButton);
            completableElements.Add(_selectModeButton);

            return completableElements;
        }

        protected override IEnumerator OnDeactivating()
        {
            _selectButton.Deactivate();
            _selectModeButton.Deactivate();
            _selectNumberPanelPlayer.Deactivate();

            yield return new WaitUntil(() => _selectNumberPanelPlayer.IsCompleteNumbersHide);
        }

        private int GetNeedForActivate() => _currentNeedForActivate;

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectMenuPlayer))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineSelectNumberPanel(),
                DefineSelectButton(),
                DefineSelectModeButton()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineSelectButton))]
        private ComponentAttachInfo DefineSelectButton()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _selectButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSelectModeButton))]
        private ComponentAttachInfo DefineSelectModeButton()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _selectModeButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSelectNumberPanel))]
        private ComponentAttachInfo DefineSelectNumberPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _selectNumberPanelPlayer, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
