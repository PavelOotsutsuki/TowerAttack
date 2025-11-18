using System.Collections.Generic;
using Tools.UI.ImageChangers;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI
{
    //[RequireComponent(typeof(OnEnterColorChanger))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class SimpleButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerUpHandler, IPointerEnterHandler, IPointerDownHandler, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] protected CanvasGroup CanvasGroup;

        private IButtonImageChanger _imageChanger;
        private readonly PointerDisableSettingsRoot _pointerDisableSettingsRoot = new PointerDisableSettingsRoot();

        public bool IsClicked { get; protected set; }
        public bool IsDisable => CanvasGroup.blocksRaycasts == false;
        public bool? IsActive { get; protected set; } = null;

        public PointerDisableSettingsRoot PointerDisableSettingsRoot => _pointerDisableSettingsRoot;

        public abstract void Init();

        protected void Init(IButtonImageChanger imageChanger)
        {
            _imageChanger = imageChanger;
        }

        public virtual void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _imageChanger.OnActivate();

            IsClicked = false;
            CanvasGroup.blocksRaycasts = true;
        }

        public virtual void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            CanvasGroup.blocksRaycasts = false;
        }

        public abstract void OnPointerClick(PointerEventData eventData);
        protected abstract void OnEnterClick();
        protected abstract void OnEnter();
        protected abstract void OnExit();

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsDisable)
                return;

            if (_pointerDisableSettingsRoot.OnPointerExit.IsDisable)
                return;

            _imageChanger.OnPointerExit();

            OnExit();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (IsDisable)
                return;

            _imageChanger.OnPointerUp();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsDisable)
                return;

            _imageChanger.OnPointerEnter();

            OnEnter();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsDisable)
                return;

            _imageChanger.OnPointerDown();
        }

        public virtual bool CanBeClicked()
        {
            return IsDisable == false;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SimpleButton))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
           return AutomaticFillComponents.DefineComponent(this, ref CanvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}