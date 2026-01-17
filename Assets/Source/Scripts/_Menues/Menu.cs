using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.InputSettings;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Menues
{
    public abstract class Menu : MonoBehaviour, IWorkable, ICompletable, IMenuInputActivateWatcher, IAutomaticFillComponents
    {
        [SerializeField] private MenuLabel _fightMenuLabel;
        [SerializeField] private MenuPanel _fightMenuPanel;
        [SerializeField] private MenuButtonsPanelRoot _menuButtonsPanelRoot;
        [SerializeField] private CanvasGroup _canvasGroup;

        //private InputRoot _inputRoot;

        private bool _isComplete;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete => _isComplete;

        public IFocusedButtonEnterHandler CurrentMenuButtonInputHandler => _menuButtonsPanelRoot.CurrentFightMenuButtonInputHandler;

        protected void Init()
        {
            gameObject.SetActive(false);
            _isComplete = true;
            IsActive = false;
            _canvasGroup.blocksRaycasts = true;

            //_inputRoot = inputRoot;

            _fightMenuLabel.Init();
            _fightMenuPanel.Init();
            //_menuButtonsPanelRoot.Init(playerLoseActions, this, cardVolume, musicVolume, cardCapabilityDescription);
        }

        public void Activate()
        {
            if (IsActive == true || _isComplete == false)
                return;

            //_inputRoot.Pause();
            OnActivateInput();

            _isComplete = false;
            IsActive = true;

            gameObject.SetActive(true);

            Activating().ToUniTask();
        }

        protected abstract void OnActivateInput();

        public void Deactivate()
        {
            if (IsActive == false || _isComplete == false)
                return;

            IsActive = false;
            _isComplete = false;
            //_inputRoot.Pause();
            //_inputRoot.DeactivateFightMenu();

            OnDeactivateInput();

            Deactivating().ToUniTask();
        }

        protected abstract void OnDeactivateInput();

        private IEnumerator Activating()
        {
            _fightMenuLabel.Show();
            _fightMenuPanel.Show();
            _menuButtonsPanelRoot.Activate();

            yield return new WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && _menuButtonsPanelRoot.IsComplete);

            //_inputRoot.ActivateFightMenu();
            OnActivatingInput();

            _isComplete = true;
        }

        protected abstract void OnActivatingInput();

        private IEnumerator Deactivating()
        {
            _fightMenuLabel.Hide();
            _fightMenuPanel.Hide();
            _menuButtonsPanelRoot.Deactivate();

            yield return new WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && _menuButtonsPanelRoot.IsComplete);

            gameObject.SetActive(false);
            //_inputRoot.DeactivateFightMenu();
            OnDeactivatingInput();

            _isComplete = true;
        }

        protected abstract void OnDeactivatingInput();

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Menu))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFightMenuLabel(),
                DefineFightMenuPanel(),
                DefineFightMenuButtonsPanel(),
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFightMenuLabel))]
        private ComponentAttachInfo DefineFightMenuLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFightMenuPanel))]
        private ComponentAttachInfo DefineFightMenuPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFightMenuButtonsPanel))]
        private ComponentAttachInfo DefineFightMenuButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _menuButtonsPanelRoot, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}