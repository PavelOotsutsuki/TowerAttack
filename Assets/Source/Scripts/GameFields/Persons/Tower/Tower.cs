using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public abstract class Tower : MonoBehaviour
    {
        [SerializeField] protected TowerSeat TowerSeat;
        [SerializeField] protected CanvasGroup CanvasGroup;

        protected IPlayCardManager PlayCardManager;

        public virtual void Init(IPlayCardManager playCardManager)
        {
            PlayCardManager = playCardManager;

            TowerSeat.Init();
        }

        public void Deactivate()
        {
            CanvasGroup.blocksRaycasts = false;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTowerSeat();
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineTowerSeat))]
        private void DefineTowerSeat()
        {
            AutomaticFillComponents.DefineComponent(this, ref TowerSeat, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref CanvasGroup, ComponentLocationTypes.InThis);
        }
    }
}
