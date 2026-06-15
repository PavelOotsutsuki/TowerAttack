using System.Collections.Generic;
using Tools.UI.ImageChangers;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI
{
    //[RequireComponent(typeof(OnEnterColorChanger))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class SimpleButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerUpHandler, IPointerEnterHandler, IPointerDownHandler, IAutomaticFillComponents
    {
        [SerializeField] protected CanvasGroup CanvasGroup;

        private IButtonImageChanger _imageChanger;
        private readonly PointerDisableSettingsRoot _pointerDisableSettingsRoot = new PointerDisableSettingsRoot();

        public bool IsClicked { get; protected set; }
        public bool IsTurnOff => CanvasGroup.blocksRaycasts == false;
        public bool IsDisable { get; private set; }
        public bool? IsActive { get; protected set; } = null;

        public PointerDisableSettingsRoot PointerDisableSettingsRoot => _pointerDisableSettingsRoot;

        public abstract void Init();

        protected void Init(IButtonImageChanger imageChanger)
        {
            _imageChanger = imageChanger;
        }

        protected void BaseActivate()
        {
            if (IsActive == true)
                return;

            IsActive = true;
            IsClicked = false;

            if (IsDisable)
            {
                SetDisableView();
                return;
            }

            _imageChanger.OnActivate();
            CanvasGroup.blocksRaycasts = true;
        }

        protected void BaseDeactivate()
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
            if (IsTurnOff)
                return;

            if (_pointerDisableSettingsRoot.OnPointerExit.IsDisable)
                return;

            _imageChanger.OnPointerExit();

            OnExit();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (IsTurnOff)
                return;

            _imageChanger.OnPointerUp();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsTurnOff)
                return;

            _imageChanger.OnPointerEnter();

            OnEnter();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsTurnOff)
                return;

            _imageChanger.OnPointerDown();
        }

        public void SetDisableView()
        {
            CanvasGroup.blocksRaycasts = false;
            _imageChanger.OnDisabled();
            IsDisable = true;
        }

        public void SetUndisableView()
        {
            CanvasGroup.blocksRaycasts = true;
            _imageChanger.OnActivate();
            IsDisable = false;
        }

        public virtual bool CanBeClicked()
        {
            return IsTurnOff == false;
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