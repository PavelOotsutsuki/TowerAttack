using UnityEngine;

namespace Tools.Loads
{
    internal class LoadPanel : MonoBehaviour, IWorkable
    {
        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            gameObject.SetActive(false);
        }
    }
}