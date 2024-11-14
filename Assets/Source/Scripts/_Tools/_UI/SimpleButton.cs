using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tools.UI
{
    [RequireComponent(typeof(OnEnterColorChanger))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class SimpleButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerUpHandler, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private OnEnterColorChanger _enterColorChanger;

        [SerializeField] protected CanvasGroup CanvasGroup;
        [SerializeField] protected Image Image;
        [SerializeField] protected Color NormalColor;
        [SerializeField] protected Color ClickColor;

        protected Color CurrentColor;

        public bool IsClicked { get; protected set; }

        public bool? IsActive { get; protected set; } = null;

        public virtual void Init()
        {
            _enterColorChanger.Init(Image);
        }

        public virtual void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            Image.color = NormalColor;
            CurrentColor = Image.color;
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

        public void OnPointerExit(PointerEventData eventData)
        {
            Image.color = CurrentColor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Image.color = CurrentColor;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SimpleButton))]
        public virtual void DefineAllComponents()
        {
            DefineColorChangePointer();
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineColorChangePointer))]
        private void DefineColorChangePointer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _enterColorChanger, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref CanvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}