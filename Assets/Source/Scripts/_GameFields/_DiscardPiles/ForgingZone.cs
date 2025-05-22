using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.CommonAnimations;
using GameFields.Persons.CardTransits;
using GameFields.Persons.Tables;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.DiscardPiles
{
    public class ForgingZone : MonoBehaviour, IWorkable, ICompletable, IForging
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private InvertCardAnimationData _invertCardAnimationData;

        private DiscardPile _discardPile;
        private PersonsState _personState;
        private InvertCardAnimation _invertCardAnimation;

        private bool _isComplete;

        public bool IsComplete => _isComplete;

        public bool? IsActive { get; private set; } = null;

        public void Init(DiscardPile discardPile, PersonsState personsState)
        {
            _discardPile = discardPile;
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

        public void StartForging(Card card)
        {
            _isComplete = false;

            StartCoroutine(Forginging(card));

            //StartCoroutine(WaitingUntilComplete());
        }
        private IEnumerator Forginging(Card card)
        {
            _invertCardAnimation.Play(card);

            yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

            _personState.Active.StartAction(this);

            _discardPile.SeatCard(card);

            IDrawCardManager drawCardManager = _personState.Active;
            drawCardManager.DrawCards(1, Continue);

            _personState.Active.PersonEffectsCounter.GnomeEffectCounter.Upgrade(card);
        }

        private void Continue()
        {
            StartCoroutine(WaitingUntilComplete());
        }

        private IEnumerator WaitingUntilComplete()
        {
            yield return new WaitForSeconds(0.5f);

            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(ForgingZone))]
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