using System.Collections.Generic;
using GameFields.Seats;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.Towers
{
    public class TowerSeat : Seat
    {
        [SerializeField] private Image _image;

        public Image Image => _image;

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerSeat))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineImage()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineImage))]
        private ComponentAttachInfo DefineImage()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _image, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}