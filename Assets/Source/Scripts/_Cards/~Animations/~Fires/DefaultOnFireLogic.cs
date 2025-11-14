using System.Collections;
using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards.Animations.Fires
{
    [RequireComponent(typeof(CardFireAnimator))]
    internal class DefaultOnFireLogic : OnFireLogic, IAutomaticFillComponents
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

            StartCoroutine(Activating(data));
        }

        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _cardFireAnimator.Deactivate();
        }

        private IEnumerator Activating(OnFireLogicActivateData data)
        {
            yield return data.Delay;

            _cardFireAnimator.Activate();

            yield return new WaitUntil(() => _cardFireAnimator.IsComplete);

            yield return new WaitForSeconds(1f);

            data.CallbackHandler.Complete();
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