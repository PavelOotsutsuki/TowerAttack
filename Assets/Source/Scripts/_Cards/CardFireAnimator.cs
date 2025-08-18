using System;
using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class CardFireAnimator : MonoBehaviour
    {
        [SerializeField] private CardFrameFireAnimation _cardFrameFireAnimation;
        [SerializeField] private CardFireAnimation _cardFireAnimation;

        public void Init()
        {
            _cardFrameFireAnimation.Init();
            _cardFireAnimation.Init();
        }

        public void Play()
        {
            _cardFrameFireAnimation.Play();
            _cardFireAnimation.Play();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardFireAnimator))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCardFrameFireAnimation(),
                DefineCardFireAnimation()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCardFrameFireAnimation))]
        private ComponentAttachInfo DefineCardFrameFireAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFrameFireAnimation, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardFireAnimation))]
        private ComponentAttachInfo DefineCardFireAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFireAnimation, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
