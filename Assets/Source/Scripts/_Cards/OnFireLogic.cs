using Tools;
using UnityEngine;

namespace Cards
{
    public abstract class OnFireLogic : MonoBehaviour, IActivatable<OnFireLogicActivateData>
    {
        public abstract void Init();
        public abstract void Activate(OnFireLogicActivateData data);
    }
}