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
            try
            {
                _imageChanger = (ISelectableButtonImageChanger)_ISelectableButtonImageChanger;
            }
            catch
            {
                _imageChanger = GetComponent<ISelectableButtonImageChanger>();
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