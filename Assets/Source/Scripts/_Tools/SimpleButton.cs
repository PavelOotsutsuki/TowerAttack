using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tools
{
    [RequireComponent(typeof(OnEnterColorChanger))]
    public abstract class SimpleButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] private OnEnterColorChanger _enterColorChanger;

        [SerializeField] protected Image Image;
        [SerializeField] protected Color NormalColor;
        [SerializeField] protected Color ClickColor;

        protected Color CurrentColor;

        public bool IsClicked { get; protected set; }

        public virtual void Init()
        {
            _enterColorChanger.Init(Image);

            Image.color = NormalColor;
            CurrentColor = Image.color;
        }

        public abstract void OnPointerClick(PointerEventData eventData);

        public void OnPointerExit(PointerEventData eventData)
        {
            Image.color = CurrentColor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Image.color = CurrentColor;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponentsSimpleButton))]
        public void DefineAllComponentsSimpleButton()
        {
            DefineColorChangePointer();
        }

        [ContextMenu(nameof(DefineColorChangePointer))]
        private void DefineColorChangePointer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _enterColorChanger, ComponentLocationTypes.InThis);
        }

        #endregion 
    }
}