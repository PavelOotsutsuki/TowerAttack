using UnityEngine;
using UnityEngine.UI;

namespace Tools.UI.ImageChangers.V1
{
    public abstract class ButtonImageChanger: MonoBehaviour
    {
        public abstract void OnActivate();
        public abstract void OnEnterClick();
        public abstract void OnPointerClick();
        public abstract void OnPointerEnter();
        public abstract void OnPointerDown();
        public abstract void OnPointerExit();
        public abstract void OnPointerUp();
    }
}