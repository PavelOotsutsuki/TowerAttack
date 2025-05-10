using Tools.UI.ImageChangers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI
{
    public class ConfirmableButton : SimpleButton
    {
        [SerializeField] private MonoBehaviour _IConfirmableButtonImageChanger;

        private IConfirmableButtonImageChanger _imageChanger;

        public override void Init()
        {
            _imageChanger = (IConfirmableButtonImageChanger)_IConfirmableButtonImageChanger;

            base.Init(_imageChanger);
        }

        public sealed override void OnPointerClick(PointerEventData eventData)
        {
            _imageChanger.OnPointerClick();

            IsClicked = true;
            CanvasGroup.blocksRaycasts = false;
            OnEnterClick();
        }

        protected override void OnEnterClick()
        { }
    }
}