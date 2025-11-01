using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Commons;
using ModestTree;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.FightMenues
{
    [RequireComponent(typeof(FadablePanel))]
    public class FightMenuButtonsPanel : MonoBehaviour, IWorkable, ICompletable, IFocusWatcher,
        IFocusedButtonEnterHandler, IAutomaticFillComponents
    {
        [SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private FightMenuButton _capitulateButton;
        [SerializeField] private FightMenuButton _soundSettingsButton;
        [SerializeField] private FightMenuButton _exitButton;

        private List<FightMenuButton> _fightMenuButtons;

        private bool _isComplete;
        private FightMenuButton _currentFocusedButton;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete => _isComplete;

        public void Init(LoseActions playerLoseActions, IDeactivatable fightMenuDeactivator)
        {
            _isComplete = true;
            _fadablePanel.Init();

            _fightMenuButtons = new List<FightMenuButton>()
            {
                _capitulateButton,
                _soundSettingsButton,
                _exitButton
            };

            _capitulateButton.Init(this, () =>
            {
                fightMenuDeactivator.Deactivate();
                playerLoseActions.Activate();
            });
            _soundSettingsButton.Init(this, null);
            _exitButton.Init(this, () => fightMenuDeactivator.Deactivate());
        }

        public void OnEnterPress()
        {
            _currentFocusedButton?.OnPointerClick(null);
        }

        public void OnDownArrow()
        {
            if (_currentFocusedButton == null)
                return;

            int index = _fightMenuButtons.IndexOf(_currentFocusedButton);

            index++;

            if (index == _fightMenuButtons.Count)
                index = 0;

            _fightMenuButtons[index].OnPointerEnter(null);
        }

        public void OnUpArrow()
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

        public void Activate()
        {
            if (IsActive == true || _isComplete == false)
                return;

            IsActive = true;
            _isComplete = false;

            foreach (FightMenuButton fightMenuButton in _fightMenuButtons)
            {
                fightMenuButton.Activate();
            }

            _fightMenuButtons[0].OnPointerEnter(null);
            //SetFocusedButton(_fightMenuButtons[0], true);

            Activating().ToUniTask();
        }

        public void Deactivate()
        {
            if (IsActive == false || _isComplete == false)
                return;

            IsActive = false;
            _isComplete = false;

            foreach (FightMenuButton fightMenuButton in _fightMenuButtons)
            {
                fightMenuButton.Deactivate();
            }

            Deactivating().ToUniTask();
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

        private IEnumerator Activating()
        {
            _fadablePanel.Show();

            yield return new WaitUntil(() => _fadablePanel.IsComplete);

            _isComplete = true;
        }

        private IEnumerator Deactivating()
        {
            _fadablePanel.Hide();

            yield return new WaitUntil(() => _fadablePanel.IsComplete);

            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FightMenuButtonsPanel))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadablePanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}