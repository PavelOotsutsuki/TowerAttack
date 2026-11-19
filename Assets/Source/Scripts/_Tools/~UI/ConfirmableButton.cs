using Tools.UI.ImageChangers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI
{
    public abstract class ConfirmableButton : SimpleButton
    {
        [SerializeField] private MonoBehaviour _IConfirmableButtonImageChanger;

        protected IConfirmableButtonImageChanger ImageChanger;

        public override void Init()
        {
            try
            {
                ImageChanger = (IConfirmableButtonImageChanger)_IConfirmableButtonImageChanger;
            }
            catch
            {
                ImageChanger = GetComponent<IConfirmableButtonImageChanger>();
            }

            base.Init(ImageChanger);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData != null) // Если null, значит насильно вызвали, значит надо
            {
                if (CanBeClicked() == false)
                    return;
            }

            ImageChanger.OnPointerClick();

            IsClicked = true;
            CanvasGroup.blocksRaycasts = false;
            OnEnterClick();
        }

        protected override void OnEnter()
        { }

        protected override void OnExit()
        { }
    }
}