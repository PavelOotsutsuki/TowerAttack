using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.InputSettings;
using Tools.Utils;
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
        protected CancellationToken MenuParentToken;

        private CancellationTokenSource _currentCTS;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete { get; protected set; }

        public IFocusedButtonEnterHandler CurrentMenuButtonInputHandler => MenuButtonsPanelRoot.CurrentMenuButtonInputHandler;

        protected void Init(MenuButtonsPanelRoot menuButtonsPanelRoot, CancellationToken menuParentToken)
        {
            gameObject.SetActive(false);
            IsComplete = true;
            IsActive = false;
            _canvasGroup.blocksRaycasts = true;

            MenuButtonsPanelRoot = menuButtonsPanelRoot;
            MenuParentToken = menuParentToken;

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

            Utils.DestroyCTS(ref _currentCTS);
            //_currentCTS?.Cancel();
            //_currentCTS?.Dispose();
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(MenuParentToken);

            gameObject.SetActive(true);

            Activating(_currentCTS.Token).Forget();
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
            Utils.DestroyCTS(ref _currentCTS);
            //_currentCTS?.Cancel();
            //_currentCTS?.Dispose();
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(MenuParentToken);

            OnDeactivateInput();

            Deactivating(_currentCTS.Token).Forget();
        }

        protected abstract void OnDeactivateInput();

        private async UniTask Activating(CancellationToken token)
        {
            _fightMenuLabel.Show(new CancellationTokenData(token));
            _fightMenuPanel.Show(new CancellationTokenData(token));
            MenuButtonsPanelRoot.Activate();

            await UniTask.WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && MenuButtonsPanelRoot.IsComplete, cancellationToken: token);

            //_inputRoot.ActivateFightMenu();
            OnActivatingInput();

            IsComplete = true;
        }

        protected abstract void OnActivatingInput();

        private async UniTask Deactivating(CancellationToken token)
        {
            DeactivateChilds(token);

            await UniTask.WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && MenuButtonsPanelRoot.IsComplete, cancellationToken: token);

            gameObject.SetActive(false);
            //_inputRoot.DeactivateFightMenu();
            OnDeactivatingInput();

            IsComplete = true;
        }

        private void DeactivateChilds(CancellationToken token)
        {
            _fightMenuLabel.Hide(new CancellationTokenData(token));
            _fightMenuPanel.Hide(new CancellationTokenData(token));
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