using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.InputSettings;
using GameFields.Persons.ConfirmableNumbersView;
using GameFields.Persons.Towers;
using Tools;
using Tools.InputSettings;
using Tools.UI.UIHelpers;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    public abstract class SelectMenuPlayer : SelectMenu, IInputLogicObject, IQPressHandler, IEnterPressHandler, IPlayerObject
    {
        [SerializeField] private SelectNumberPanelPlayer _selectNumberPanelPlayer;
        [SerializeField] private SelectButton _selectButton;
        [SerializeField] private SelectModeButton _selectModeButton;

        private int _currentNeedForActivate;
        private GameFieldInputRoot _inputRoot;

        public void Init(ICardNumberKeeper cardNumberKeeper, ISelectResultHandler selectResultHandler, int[] cardNumbers,
            SelectNumbersList selectedNumbers, ConfirmableNumbers confirmableNumbers, GameFieldInputRoot inputRoot,
            LastSelectedNumbersWatcher lastSelectedNumbersWatcher, UIHelperDescription UIHelperDescription, CancellationToken fightToken,
            CancellationToken gameFieldToken)
        {
            _selectButton.Init(this, inputRoot, fightToken);
            _selectNumberPanelPlayer.Init(_selectButton, cardNumberKeeper, cardNumbers, selectedNumbers, confirmableNumbers,
                lastSelectedNumbersWatcher, fightToken);
            _selectModeButton.Init(_selectNumberPanelPlayer, UIHelperDescription, gameFieldToken);

            _inputRoot = inputRoot;

            SelectMenuLabelTextLogic selectMenuLabelTextLogic = new DefaultSelectMenuLabelTextLogic(GetNeedForActivate);

            base.Init(selectResultHandler, _selectNumberPanelPlayer, selectMenuLabelTextLogic, fightToken);
        }

        //public IPointerClickHandler SelectModeButton => _selectModeButton;
        //public IPointerClickHandler SelectButton => _selectButton;

        public override void Activate(SelectMenuActivateData activateData)
        {
            if (FightToken.IsCancellationRequested)
                return;

            if (IsActive == true)
                return;

            _inputRoot.SetInputType(this);

            _currentNeedForActivate = activateData.NeedSelect;

            base.Activate(activateData);

            _selectModeButton.Activate(new CancellationTokenData(CurrentCTS.Token));
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

        protected override async UniTask OnDeactivating(CancellationToken token)
        {
            _selectButton.Deactivate();
            _selectModeButton.Deactivate(new CancellationTokenData(token));
            _selectNumberPanelPlayer.Deactivate();

           await UniTask.WaitUntil(() => _selectNumberPanelPlayer.IsCompleteNumbersHide, cancellationToken: token);
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