using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class Tower : MonoBehaviour, ICardDropPlace
    {
        [SerializeField] private TowerSeat _towerSeat;

        private IPlayCardManager _playCardManager;

        public void Init(IPlayCardManager playCardManager)
        {
            _playCardManager = playCardManager;

            _towerSeat.Init();
        }

        public void GetCard(Card card)
        {
            _playCardManager.PlayCard(card);

            _towerSeat.GetCard(card);
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
