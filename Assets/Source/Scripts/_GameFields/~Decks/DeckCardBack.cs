using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields.Decks
{
    internal class DeckCardBack : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;

        private Movement _movement;
        private ReadOnlyTransform _ROTransform;

        public void Init()
        {
            _movement = new Movement(_transform);
            _ROTransform = new ReadOnlyTransform(_transform);
        }

        public void MoveOn(Vector2 position)
        {
            _movement.MoveLocalInstantly(position, _ROTransform.GetRotationVector());
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(DeckCardBack))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}