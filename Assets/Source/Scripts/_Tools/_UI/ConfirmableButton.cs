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
            try
            {
                _imageChanger = (IConfirmableButtonImageChanger)_IConfirmableButtonImageChanger;
            }
            catch
            {
                _imageChanger = GetComponent<IConfirmableButtonImageChanger>();
            }

            base.Init(_imageChanger);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData != null) // Если null, значит насильно вызвали, значит надо
            {
                if (CanBeClicked() == false)
                    return;
            }

            _imageChanger.OnPointerClick();

            IsClicked = true;
            CanvasGroup.blocksRaycasts = false;
            OnEnterClick();
        }

        protected override void OnEnterClick()
        { }
    }
}