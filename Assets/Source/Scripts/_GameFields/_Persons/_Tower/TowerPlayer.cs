using System.Collections.Generic;
using Cards;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.Persons.Towers
{
    public class TowerPlayer : Tower, IPlayerObject, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TowerCardView _towerCardView;

        public override void Init()
        {
            base.Init();

            _towerCardView.Init();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            BigCardShowData data = new BigCardShowData(GameSettings.CardSize, ReadOnlyRectTransform, TowerSeat.Card.ViewConfig);
            _towerCardView.Activate(data);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _towerCardView.Deactivate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerPlayer))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerCardView()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineTowerCardView))]
        private ComponentAttachInfo DefineTowerCardView()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _towerCardView, ComponentLocationTypes.InScene);
        }
        #endregion 
    }
}