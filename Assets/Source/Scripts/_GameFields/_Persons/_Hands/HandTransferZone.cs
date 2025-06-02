using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.CommonAnimations;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandTransferZone : MonoBehaviour, IWorkable, ICompletable, IHandTransferable
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private InvertCardAnimationData _invertCardAnimationData;

        private Hand _hand;
        private PersonsState _personState;
        private InvertCardAnimation _invertCardAnimation;

        private bool _isComplete;

        public bool IsComplete => _isComplete;

        public bool? IsActive { get; private set; } = null;

        public void Init(Hand hand, PersonsState personsState)
        {
            _hand = hand;
            _personState = personsState;
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

        public void StartHandTransfing(Card card)
        {
            _isComplete = false;

            StartCoroutine(Transfing(card));
        }

        //public void StartForging(Card card)
        //{
        //    _isComplete = false;

        //    StartCoroutine(Forginging(card));

        //    //StartCoroutine(WaitingUntilComplete());
        //}
        private IEnumerator Transfing(Card card)
        {
            _personState.Active.StartAction(this);

            _invertCardAnimation.Play(card);

            yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

            _hand.SeatCard(card);

            //IDrawCardManager drawCardManager = _personState.Active;
            //drawCardManager.DrawCards(1, Continue);

            //_personState.Active.PersonEffectsHandler.GnomeEffectCounter.Upgrade();
            yield return WaitingUntilComplete();
        }

        //private void Continue()
        //{
        //    StartCoroutine(WaitingUntilComplete());
        //}

        private IEnumerator WaitingUntilComplete()
        {
            yield return new WaitForSeconds(0.5f);

            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(HandTransferZone))]
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