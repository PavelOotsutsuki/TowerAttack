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

            DiscoverLabelActivateData labelData = new DiscoverLabelActivateData(data.ActivateMessage);

            _discoverPanel.Activate();
            _discoverLabel.Activate(labelData);
        }

        protected override void Deactivate()
        {
            _discoverPanel.Deactivate();
            _discoverLabel.Deactivate();

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