using Tools;
using UnityEngine;

namespace Cards
{
    internal abstract class OnFireLogic : MonoBehaviour, IWorkable<OnFireLogicActivateData>
    {
        public bool? IsActive { get; protected set; } = null;

        public abstract void Init();
        public abstract void Activate(OnFireLogicActivateData data);
        public abstract void Deactivate();
    }
}