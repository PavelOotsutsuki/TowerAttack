using System;
using System.Collections.Generic;
using Cards;
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

        public override void Activate(List<Card> cards, string activateMessage, Action<Card> callback)
        {
            base.Activate(cards, activateMessage, callback);

            _discoverPanel.Activate();
            _discoverLabel.Activate(activateMessage);
        }

        public override void Deactivate()
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
