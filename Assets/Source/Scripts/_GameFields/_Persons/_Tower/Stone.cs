using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class Stone : MonoBehaviour, IAutomaticFillComponents
    {
        private const float BoomDuration = 1f;
        private const float Offset = 3f;
        private const Ease BoomEase = Ease.InQuint;

        [SerializeField] private RectTransform _rectTransform;

        public void Boom(float positionY)
        {
            float xOffset = Random.Range(Offset * (-1), Offset);

            float positionX = transform.position.x + xOffset;
            positionY = positionY + _rectTransform.rect.height / 2;
            float positionZ = transform.position.z;

            transform.DOMove(new Vector3(positionX, positionY, positionZ), BoomDuration).SetEase(BoomEase);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Stone))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}
