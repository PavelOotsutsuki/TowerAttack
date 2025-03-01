using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Discovers;
using Tools.UI;
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

            LabelActivateData labelData = new LabelActivateData(data.ActivateMessage);

            _discoverLabel.Show(labelData);
        }

        protected override void Deactivate()
        {
            _discoverLabel.Hide();

            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => _discoverLabel.IsComplete);

            base.Deactivate();
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(StartTowerCardSelectionPlayerDiscover))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineDiscoverLabel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineDiscoverLabel))]
        private ComponentAttachInfo DefineDiscoverLabel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _discoverLabel, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}