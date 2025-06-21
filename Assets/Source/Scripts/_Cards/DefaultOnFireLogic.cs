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
            if (IsActive == true)
                return;

            IsActive = true;

            StartCoroutine(Activating(data.Delay));
        }

        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _cardFireAnimator.Deactivate();
            //StartCoroutine(Deactivating());
        }

        private IEnumerator Activating(WaitForSeconds delay)
        {
            yield return delay;

            _cardFireAnimator.Activate();
        }

        //private IEnumerator Deactivating()
        //{
        //    //yield return _currentDelay;

        //    _cardFireAnimator.Deactivate();
        //    yield break;
        //}

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