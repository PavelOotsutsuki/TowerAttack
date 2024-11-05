using System.Collections;
using Cysharp.Threading.Tasks;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public class DiscoverPlayer : Discover
    {
        [SerializeField] private DiscoverPanel _discoverPanel;
        [SerializeField] private DiscoverLabel _discoverLabel;

        public override void Init()
        {
            _discoverPanel.Init();
            _discoverLabel.Init();

            base.Init();
        }

        public override void Activate(DiscoverActivateData data)
        {
            base.Activate(data);

            FadableLabelActivateData labelData = new FadableLabelActivateData(data.ActivateMessage);

            _discoverPanel.Show();
            _discoverLabel.Activate(labelData);
        }

        protected override void Deactivate()
        {
            _discoverPanel.Hide();
            _discoverLabel.Deactivate();

            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => _discoverLabel.IsComplete && _discoverPanel.IsComplete);

            Debug.Log("startDeactivate");
            base.Deactivate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        protected override void DefineAllComponents()
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