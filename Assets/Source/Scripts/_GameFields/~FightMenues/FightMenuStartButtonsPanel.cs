using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using Menues;
using ModestTree;
using Tools;
using Tools.UI;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.FightMenues
{
    public class FightMenuStartButtonsPanel : CustomFocusMenuButtonsPanel//, IAutomaticFillComponents
    {
        //[SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private FightMenuButton _resumeButton;
        [SerializeField] private FightMenuButton _rulesButton;
        [SerializeField] private FightMenuButton _settingsButton;
        [SerializeField] private FightMenuButton _capitulateButton;
        [SerializeField] private FightMenuButton _exitButton;

        private List<FightMenuButton> _fightMenuButtons;

        //private bool _isComplete;
        private FightMenuButton _currentFocusedButton;

        //public override bool? IsActive { get; protected set; } = null;
        //public bool IsComplete => _isComplete;

        public void Init(LoseActions playerLoseActions, IDeactivatable fightMenuDeactivator, Action onSettingsButtonClick,
            Action onRulesButtonClick)
        {
            //_isComplete = true;
            //_fadablePanel.Init();

            _fightMenuButtons = new List<FightMenuButton>()
            {
                _resumeButton,
                _rulesButton,
                _settingsButton,
                _capitulateButton,
                _exitButton
            };

            _resumeButton.Init(this, () => fightMenuDeactivator.Deactivate());
            _rulesButton.Init(this, onRulesButtonClick);
            _settingsButton.Init(this, onSettingsButtonClick);
            _capitulateButton.Init(this, () =>
            {
                fightMenuDeactivator.Deactivate();
                playerLoseActions.Activate();
            });
            _exitButton.Init(this, Utils.Quit);
        }

        public override void OnEnterPress()
        {
            _currentFocusedButton?.OnPointerClick(null);
        }

        public override void OnDownArrow()
        {
            if (_currentFocusedButton == null)
                return;

            int index = _fightMenuButtons.IndexOf(_currentFocusedButton);

            index++;

            if (index == _fightMenuButtons.Count)
                index = 0;

            _fightMenuButtons[index].OnPointerEnter(null);
        }

        public override void OnUpArrow()
        {
            if (_currentFocusedButton == null)
                return;

            int index = _fightMenuButtons.IndexOf(_currentFocusedButton);

            index--;

            if (index < 0)
                index = _fightMenuButtons.Count - 1;

            _fightMenuButtons[index].OnPointerEnter(null);
        }

        public override void SetFocused(ConfirmableFocusableButton focusedButton)
        {
            if (_currentFocusedButton == focusedButton)
                return;

            if (_fightMenuButtons.Contains(focusedButton) == false)
                throw new Exception("Ну и какого хера ты пытаешься зафокусить неподвластную тебе кнопку???");

            UnfocuseButton();

            foreach (FightMenuButton fightMenuButton in _fightMenuButtons)
            {
                if (fightMenuButton == focusedButton)
                {
                    _currentFocusedButton = fightMenuButton;
                    _currentFocusedButton.PointerDisableSettingsRoot.OnPointerExit.Disable();
                    return;
                }
            }
        }

        public override void Activate()
        {
            if (IsActive == true)
                return;

            base.Activate();

            //_isComplete = false;

            foreach (FightMenuButton fightMenuButton in _fightMenuButtons)
            {
                fightMenuButton.Activate();
            }

            EventSystem.current.SetSelectedGameObject(null);
            _fightMenuButtons[0].OnPointerEnter(null);
            //SetFocusedButton(_fightMenuButtons[0], true);

            //Activating().ToUniTask();
        }

        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            base.Deactivate();

            //_isComplete = false;

            foreach (FightMenuButton fightMenuButton in _fightMenuButtons)
            {
                fightMenuButton.Deactivate();
            }

            //Deactivating().ToUniTask();
        }

        private void UnfocuseButton()
        {
            if (_currentFocusedButton != null)
            {
                _currentFocusedButton.PointerDisableSettingsRoot.OnPointerExit.Enable();
                _currentFocusedButton.OnPointerExit(null);
                _currentFocusedButton = null;
            }
        }

        public override void OnLeftArrow()
        {
            throw new NotImplementedException();
        }

        public override void OnRightArrow()
        {
            throw new NotImplementedException();
        }

        //private IEnumerator Activating()
        //{
        //    _fadablePanel.Show();

        //    yield return new WaitUntil(() => _fadablePanel.IsComplete);

        //    _isComplete = true;
        //}

        //private IEnumerator Deactivating()
        //{
        //    _fadablePanel.Hide();

        //    yield return new WaitUntil(() => _fadablePanel.IsComplete);

        //    _isComplete = true;
        //}

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents) + nameof(FightMenuButtonsPanel))]
        //public virtual List<ComponentAttachInfo> DefineAllComponents()
        //{
        //    List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
        //    {
        //        DefineFadablePanel()
        //    };

        //    return list;
        //}

        //[ContextMenu(nameof(DefineFadablePanel))]
        //private ComponentAttachInfo DefineFadablePanel()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        //}
        //#endregion
    }
}