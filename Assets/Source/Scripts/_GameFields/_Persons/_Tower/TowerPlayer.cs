using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Common;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.Persons.Towers
{
    public class TowerPlayer : Tower, IPlayerObject//, IPointerEnterHandler, IPointerExitHandler
    {
        //[SerializeField] private TowerCardView _towerCardView;

        //public override void Init()
        //{
        //    base.Init();

        //    _towerCardView.Init();
        //}

        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    Debug.Log("OnPointerEnter");
        //    BigCardShowData data = new BigCardShowData(GameSettings.CardSize, ReadOnlyRectTransform, TowerSeat.Card.ViewConfig);
        //    _towerCardView.Activate(data);
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    Debug.Log("OnPointerExit");
        //    _towerCardView.Deactivate();
        //}

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents) + nameof(TowerPlayer))]
        //public override List<ComponentAttachInfo> DefineAllComponents()
        //{
        //    List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
        //    {
        //        DefineTowerCardView()
        //    };

        //    list.AddRange(base.DefineAllComponents());

        //    return list;
        //}

        //[ContextMenu(nameof(DefineTowerCardView))]
        //private ComponentAttachInfo DefineTowerCardView()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _towerCardView, ComponentLocationTypes.InScene);
        //}
        //#endregion

        [SerializeField] private TowerPlayerHelper _towerPlayerHelper;

        public override void Init()
        {
            base.Init();

            _towerPlayerHelper.Init(GetCardViewData);
        }

        public override void SeatCard(Card card)
        {
            if (HasFreeSeat)
            {
                _towerPlayerHelper.Activate();
            }
            else
            {
                _towerPlayerHelper.Deactivate();
            }

            base.SeatCard(card);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerPlayer))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerPlayerHelper()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineTowerPlayerHelper))]
        private ComponentAttachInfo DefineTowerPlayerHelper()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _towerPlayerHelper, ComponentLocationTypes.InScene);
        }
        #endregion
    }
}