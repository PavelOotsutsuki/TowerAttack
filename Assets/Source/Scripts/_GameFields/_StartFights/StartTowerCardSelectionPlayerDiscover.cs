using System;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Discovers;
using Tools;
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

        public override void Activate(List<Card> cards, string activateMessage, Action<Card> callback)
        {
            base.Activate(cards, activateMessage, callback);

            _discoverLabel.Activate(activateMessage);
        }

        public override void Deactivate()
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
