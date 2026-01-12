using System.Collections;
using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards.Animations.Fires
{
    internal class CardFireAnimator : MonoBehaviour, IWorkable, ICompletable
    {
        [SerializeField] private CardFrameFireAnimation _cardFrameFireAnimation;
        [SerializeField] private CardFireAnimation _cardFireAnimation;
        [SerializeField] private CardFrameRiseAnimation _cardFrameRiseAnimation;
        [SerializeField] private CardRiseAnimation _cardRiseAnimation;

        public bool? IsActive { get; private set; } = null;

        public bool IsComplete => _cardFrameFireAnimation.IsComplete && _cardFireAnimation.IsComplete;

        public void Init()
        {
            _cardFrameRiseAnimation.Init();
            _cardRiseAnimation.Init();
            _cardFrameFireAnimation.Init();
            _cardFireAnimation.Init();
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _cardFrameFireAnimation.Play();
            _cardFireAnimation.Play();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _cardFrameRiseAnimation.Play();
            _cardRiseAnimation.Play();

            StartCoroutine(WaitingUntilDeactivate());
        }

        private IEnumerator WaitingUntilDeactivate()
        {
            yield return new WaitUntil(() => _cardFrameRiseAnimation.IsComplete && _cardRiseAnimation.IsComplete);

            _cardFireAnimation.Deactivate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardFireAnimator))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCardFrameFireAnimation(),
                DefineCardFireAnimation(),
                DefineCardFrameRiseAnimation(),
                DefineCardRiseAnimation()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCardFrameFireAnimation))]
        private ComponentAttachInfo DefineCardFrameFireAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFrameFireAnimation, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardFireAnimation))]
        private ComponentAttachInfo DefineCardFireAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFireAnimation, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardFrameRiseAnimation))]
        private ComponentAttachInfo DefineCardFrameRiseAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFrameRiseAnimation, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardRiseAnimation))]
        private ComponentAttachInfo DefineCardRiseAnimation()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardRiseAnimation, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}