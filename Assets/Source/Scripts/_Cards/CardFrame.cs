using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CardFrame : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private CardFrameFireAnimation _cardFireAnimation;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _frameImage;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _enableFrameColor;
        [SerializeField] private Color _disableFrameColor;

        public bool? IsShown { get; private set; } = null;

        public void Init()
        {
            _cardFireAnimation.Init();
            _frameImage.color = _defaultColor;

            //Show();
        }

        public void Fire()
        {
            _cardFireAnimation.Play();
        }

        public void Block()
        {
            _frameImage.color = _disableFrameColor;

            _canvasGroup.blocksRaycasts = false;
        }

        public void Unblock()
        {
            _frameImage.color = _enableFrameColor;

            _canvasGroup.blocksRaycasts = true;
        }

        public void Neutral()
        {
            _frameImage.color = _defaultColor;

            _canvasGroup.blocksRaycasts = false;
        }

        //public void Show()
        //{
        //    if (IsShown == true)
        //        return;

        //    IsShown = true;

        //    _canvasGroup.alpha = 1;
        //}

        //public void Hide()
        //{
        //    if (IsShown == false)
        //        return;

        //    IsShown = false;

        //    _canvasGroup.alpha = 0;
        //}

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardFrame))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup(),
                DefineCardFrameFireAnimation(),
                DefineImage()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCardFrameFireAnimation))]
        private ComponentAttachInfo DefineCardFrameFireAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFireAnimation, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineImage))]
        private ComponentAttachInfo DefineImage()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _frameImage, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}