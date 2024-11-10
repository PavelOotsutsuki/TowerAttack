using System.Collections;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DiscoverPlayer : Discover
    {
        [SerializeField] private DiscoverPanel _discoverPanel;
        [SerializeField] private DiscoverLabel _discoverLabel;

        [SerializeField] private CanvasGroup _canvasGroup;

        private IHandBlockable _handBlockable;

        public void Init(IHandBlockable handBlockable)
        {
            _handBlockable = handBlockable;

            _canvasGroup.blocksRaycasts = true;

            _discoverPanel.Init();
            _discoverLabel.Init();

            base.Init();
        }

        public override void Activate(DiscoverActivateData data)
        {
            _handBlockable.ForciblyBlock();
            _canvasGroup.blocksRaycasts = true;

            base.Activate(data);

            FadableLabelActivateData labelData = new FadableLabelActivateData(data.ActivateMessage);

            _discoverPanel.Show();
            _discoverLabel.Show(labelData);
        }

        protected override void Deactivate()
        {
            _canvasGroup.blocksRaycasts = false;

            _discoverPanel.Hide();
            _discoverLabel.Hide();

            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => _discoverLabel.IsComplete && _discoverPanel.IsComplete);

            _handBlockable.Unblock();
            base.Deactivate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(DiscoverPlayer))]
        public override void DefineAllComponents()
        {
            DefineDiscoverPanel();
            DefineDiscoverLabel();

            base.DefineAllComponents();
        }

        [ContextMenu(nameof(DefineDiscoverPanel))]
        private void DefineDiscoverPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discoverPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDiscoverLabel))]
        private void DefineDiscoverLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discoverLabel, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}