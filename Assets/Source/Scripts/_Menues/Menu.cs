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
        [SerializeField] private CanvasGroup _canvasGroup;

        //private InputRoot _inputRoot;
        protected MenuButtonsPanelRoot MenuButtonsPanelRoot;


        public bool? IsActive { get; private set; } = null;
        public bool IsComplete { get; protected set; }

        public IFocusedButtonEnterHandler CurrentMenuButtonInputHandler => MenuButtonsPanelRoot.CurrentMenuButtonInputHandler;

        protected void Init(MenuButtonsPanelRoot menuButtonsPanelRoot)
        {
            gameObject.SetActive(false);
            IsComplete = true;
            IsActive = false;
            _canvasGroup.blocksRaycasts = true;

            MenuButtonsPanelRoot = menuButtonsPanelRoot;

            //_inputRoot = inputRoot;

            _fightMenuLabel.Init();
            _fightMenuPanel.Init();
            //_menuButtonsPanelRoot.Init(playerLoseActions, this, cardVolume, musicVolume, cardCapabilityDescription);
        }

        public void Activate()
        {
            if (IsActive == true || IsComplete == false)
                return;

            //_inputRoot.Pause();
            OnActivateInput();

            IsComplete = false;
            IsActive = true;

            gameObject.SetActive(true);

            Activating().ToUniTask();
        }

        protected abstract void OnActivateInput();

        public void Deactivate()
        {
            if (IsActive == false || IsComplete == false)
                return;

            IsActive = false;
            IsComplete = false;
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
            MenuButtonsPanelRoot.Activate();

            yield return new WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && MenuButtonsPanelRoot.IsComplete);

            //_inputRoot.ActivateFightMenu();
            OnActivatingInput();

            IsComplete = true;
        }

        protected abstract void OnActivatingInput();

        private IEnumerator Deactivating()
        {
            DeactivateChilds();

            yield return new WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && MenuButtonsPanelRoot.IsComplete);

            gameObject.SetActive(false);
            //_inputRoot.DeactivateFightMenu();
            OnDeactivatingInput();

            IsComplete = true;
        }

        private void DeactivateChilds()
        {
            _fightMenuLabel.Hide();
            _fightMenuPanel.Hide();
            MenuButtonsPanelRoot.Deactivate();
        }

        protected abstract void OnDeactivatingInput();

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Menu))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineMenuLabel(),
                DefineMenuPanel(),
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineMenuLabel))]
        private ComponentAttachInfo DefineMenuLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineMenuPanel))]
        private ComponentAttachInfo DefineMenuPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}