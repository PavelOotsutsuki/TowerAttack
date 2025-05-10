using UnityEngine;

namespace Tools.UI.ImageChangers.V1
{
    public class ConfirmableButtonImageChanger : ButtonImageChangerDecorator
    {
        [SerializeField] private ButtonImageChangerRealization _realization; 

        public override void OnActivate()
        {
            _realization.OnActivate();
        }

        public override void OnEnterClick()
        {
            _realization.OnEnterClick();
        }

        public override void OnPointerClick()
        {
            _realization.OnPointerClick();
        }

        public override void OnPointerDown()
        {
            _realization.OnPointerDown();
        }

        public override void OnPointerEnter()
        {
            _realization.OnPointerEnter();
        }

        public override void OnPointerExit()
        {
            _realization.OnPointerExit();
        }

        public override void OnPointerUp()
        {
            _realization.OnPointerUp();
        }
    }
}