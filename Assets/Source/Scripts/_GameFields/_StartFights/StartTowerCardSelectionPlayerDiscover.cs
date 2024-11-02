using GameFields.Persons.Discovers;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionPlayerDiscover : Discover
    {
        [SerializeField] private DiscoverLabel _discoverLabel;

        public override void Init()
        {
            _discoverLabel.Init();

            base.Init();
        }

        public override void Activate(DiscoverActivateData data)
        {
            base.Activate(data);

            DiscoverLabelActivateData labelData = new DiscoverLabelActivateData(data.ActivateMessage);

            _discoverLabel.Activate(labelData);
        }

        protected override void Deactivate()
        {
            _discoverLabel.Deactivate();

            base.Deactivate();
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents))]
        protected override void DefineAllComponents()
        {
            DefineDiscoverLabel();

            base.DefineAllComponents();
        }

        [ContextMenu(nameof(DefineDiscoverLabel))]
        private void DefineDiscoverLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discoverLabel, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}