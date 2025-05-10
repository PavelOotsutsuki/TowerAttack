using Tools.UI.ImageChangers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI
{
    public class SelectableButton : SimpleButton
    {
        [SerializeField] private MonoBehaviour _ISelectableButtonImageChanger;

        private ISelectableButtonImageChanger _imageChanger;

        public override void Init()
        {
            //_imageChanger = GetComponent<ISelectableButtonImageChanger>();
            _imageChanger = (ISelectableButtonImageChanger)_ISelectableButtonImageChanger;

            base.Init(_imageChanger);
        }

        public sealed override void OnPointerClick(PointerEventData eventData)
        {
            if (IsClicked == false)
            {
                OnEnterClick();
            }
            else
            {
                OnExitClick();
            }

            IsClicked = IsClicked == false;
            _imageChanger.OnPointerClick();
        }

        protected override void OnEnterClick()
        {
            _imageChanger.OnEnterClick();
        }

        protected virtual void OnExitClick()
        {
            _imageChanger.OnExitClick();
        }
    }
}