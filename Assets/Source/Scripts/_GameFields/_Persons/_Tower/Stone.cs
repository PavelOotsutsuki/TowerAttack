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
        private const float BoomDuration = 0.8f;
        private const float Offset = 3f;
        private const Ease BoomEase = Ease.InQuint;

        [SerializeField] private RectTransform _rectTransform;

        public void Boom(float heightTower)
        {
            float xOffset = Random.Range(Offset * (-1), Offset);

            float positionX = transform.localPosition.x + xOffset;
            float positionY = (heightTower / 2) * (-1) + _rectTransform.rect.height / 2;
            float positionZ = transform.localPosition.z;

            transform.DOLocalMove(new Vector3(positionX, positionY, positionZ), BoomDuration).SetEase(BoomEase);
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
