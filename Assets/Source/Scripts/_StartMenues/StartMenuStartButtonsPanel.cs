using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StartMenues
{
    public class StartMenuStartButtonsPanel : StartMenuButtonsPanel, IFocusWatcher//, IAutomaticFillComponents
    {
        //[SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private StartMenuButton _playButton;
        [SerializeField] private StartMenuButton _campaignButton;
        [SerializeField] private StartMenuButton _collectionButton;
        [SerializeField] private StartMenuButton _achievementsButton;
        [SerializeField] private StartMenuButton _rulesButton;
        [SerializeField] private StartMenuButton _settingsButton;
        [SerializeField] private StartMenuButton _exitButton;

        private List<StartMenuButton> _fightMenuButtons;

        //private bool _isComplete;
        private StartMenuButton _currentFocusedButton;

        public override bool? IsActive { get; protected set; } = null;
        //public bool IsComplete => _isComplete;

        public void Init(Action onSettingsButtonClick, Action onRulesButtonClick)
        {
            //_isComplete = true;
            //_fadablePanel.Init();

            _fightMenuButtons = new List<StartMenuButton>()
            {
                _playButton,
                _campaignButton,
                _collectionButton,
                _achievementsButton,
                _rulesButton,
                _settingsButton,
                _exitButton
            };

            _playButton.Init(this, null);
            _campaignButton.Init(this, null);
            _collectionButton.Init(this, null);
            _achievementsButton.Init(this, null);
            _rulesButton.Init(this, onRulesButtonClick);
            _settingsButton.Init(this, onSettingsButtonClick);
            _exitButton.Init(this, () =>
            {
                #if UNITY_EDITOR
                {
                    EditorApplication.isPlaying = false;
                }
                #else
                {
                    Application.Quit();
                }
                #endif
            });
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

        public void SetFocusedButton(ConfirmableFocusableButton focusedButton)
        {
            if (_currentFocusedButton == focusedButton)
                return;

            if (_fightMenuButtons.Contains(focusedButton) == false)
                throw new System.Exception("Ну и какого хера ты пытаешься зафокусить неподвластную тебе кнопку???");

            UnfocuseButton();

            foreach (StartMenuButton fightMenuButton in _fightMenuButtons)
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

            foreach (StartMenuButton fightMenuButton in _fightMenuButtons)
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

            foreach (StartMenuButton fightMenuButton in _fightMenuButtons)
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