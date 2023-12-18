using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerSeat _towerSeat;

        public void Init(IPlayCardManager playCardManager)
        {
            _towerSeat.Init();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTowerSeat();
        }

        [ContextMenu(nameof(DefineTowerSeat))]
        private void DefineTowerSeat()
        {
            AutomaticFillComponents.DefineComponent(this, ref _towerSeat, ComponentLocationTypes.InChildren);
        }
    }
}
