using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.CommonAnimations;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;
using GameFields.Signals;
using Cards.DependencyInterlayers;
using GameFields.CardTransits;

namespace GameFields.Persons.Hands
{
    public abstract class ExtraEffectZone : MonoBehaviour, IWorkable, ICompletable, IExtraEffectZone
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private InvertCardAnimationData _invertCardAnimationData;

        private ICardSeatable _seatable;
        private SignalBus _bus;
        private InvertCardAnimation _invertCardAnimation;

        public bool IsComplete { get; protected set; }

        public bool? IsActive { get; private set; } = null;

        public void Init(ICardSeatable cardSeatable, SignalBus signalBus)
        {
            _seatable = cardSeatable;
            _bus = signalBus;
            _invertCardAnimation = new InvertCardAnimation(_invertCardAnimationData);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _canvasGroup.blocksRaycasts = true;
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _canvasGroup.blocksRaycasts = false;
        }

        public void StartExtraEffect(Card card)
        {
            IsComplete = false;

            StartCoroutine(Processing(card));
        }

        private IEnumerator Processing(Card card)
        {
            _bus.Fire(new PushStepSignalPlayer(this));

            _invertCardAnimation.Play(card);

            yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

            _seatable.SeatCard(card);

            OnEndProcessing();
        }

        protected abstract void OnEndProcessing();

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(ExtraEffectZone))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}