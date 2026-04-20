using Tools;
using Tools.InputSettings;
using UnityEngine;

namespace Menues
{
    public abstract class MenuButtonsPanel : MonoBehaviour, IWorkable, IFocusedButtonEnterHandler
    {
        public abstract bool? IsActive { get; protected set; }

        public virtual void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            gameObject.SetActive(false);
        }

        public abstract void OnDownArrow();
        public abstract void OnEnterPress();
        public abstract void OnUpArrow();
    }
}