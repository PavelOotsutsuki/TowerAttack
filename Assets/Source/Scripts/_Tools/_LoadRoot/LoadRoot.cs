using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.Loads
{
    public class LoadRoot : MonoBehaviour/*, IWorkable*/, IAutomaticFillComponents
    {
        [SerializeField] private LoadPanel _loadPanel;
        [SerializeField] private LoadText _loadText;

        private readonly List<LoadSession> _currentSessions = new List<LoadSession>();

        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            _loadPanel.Init();
            _loadText.Init();
        }

        public void AddSession(LoadSession loadSession)
        {
            _currentSessions.Add(loadSession);

            Activate();
        }

        private void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _loadPanel.Activate();
            _loadText.Activate();

            WaitingAllSessions(this.destroyCancellationToken).Forget();
        }

        private async UniTask WaitingAllSessions(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _currentSessions.Any(s => s.IsComplete == false) == false, cancellationToken: token);

            _currentSessions.Clear();

            Deactivate();
        }

        private void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _loadPanel.Deactivate();
            _loadText.Deactivate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LoadRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLoadPanel(),
                DefineLoadText()
            };

            return list;
        }

        [ContextMenu(nameof(DefineLoadPanel))]
        private ComponentAttachInfo DefineLoadPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _loadPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineLoadText))]
        private ComponentAttachInfo DefineLoadText()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _loadText, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}