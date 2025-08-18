using System.Collections;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    [RequireComponent(typeof(CardFireAnimator))]
    public class DefaultOnFireLogic : OnFireLogic, IAutomaticFillComponents
    {
        [SerializeField] private CardFireAnimator _cardFireAnimator;

        public override void Init()
        {
            _cardFireAnimator.Init();
        }

        public override void Activate(OnFireLogicActivateData data)
        {
            StartCoroutine(Activating(data.Delay));
        }

        private IEnumerator Activating(WaitForSeconds delay)
        {
            yield return delay;

            _cardFireAnimator.Play();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(DefaultOnFireLogic))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCardFireAnimator(),
            };

            return list;
        }

        [ContextMenu(nameof(DefineCardFireAnimator))]
        private ComponentAttachInfo DefineCardFireAnimator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFireAnimator, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}