using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Views;
using GameFields.CardTransits;
using GameFields.Persons;
using Tools;
using UnityEngine;

namespace GameFields.Effects
{
    public class FallenGuardian_FalseChoiceEffect : Effect
    {
        private readonly IPersonObject _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public FallenGuardian_FalseChoiceEffect(IPersonObject activePerson, CardLocationViewRoot viewRoot,
            CardTransitManager transitManager, ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _viewRoot = viewRoot;
            _transitManager = transitManager;
            _typesRoot = typesRoot;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Падшего Хранителя(2.0) закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            PersonTypes activePersonTypes = _typesRoot.GetPersonTypes(_activePerson);
            ViewType handView = activePersonTypes.Hand.ViewType;

            IEnumerable<Card> cardsInHand = _viewRoot.GetAllCards(handView);

            if (cardsInHand.Count() <= 0)
                yield break;

            TransitFromType handFrom = activePersonTypes.Hand.FromType;
            TransitToType firePoolTo = activePersonTypes.FirePool;

            int indexOffset = 0; // Смещение индекса для спавна пиромантов из-за сжигания других карт
            Dictionary<Card, int> firedCardIndexPair = new Dictionary<Card, int>();
            Card lastPyromancersManuscript = null;
            Card lastCard = null;

            foreach (Card firedCard in cardsInHand)
            {
                int index = _viewRoot.IndexOf(handView, firedCard);
                firedCardIndexPair.Add(firedCard, index);

                if (firedCard.IsPyromancersManuscript)
                {
                    lastPyromancersManuscript = firedCard;
                }

                lastCard = firedCard;
            }

            CallbackHandler lastPyromancersManuscriptFireCallback = null;
            CallbackHandler lastCardFireCallback = null;

            foreach (Card firedCard in cardsInHand)
            {
                int realIndex = firedCardIndexPair[firedCard] + indexOffset;

                if (lastPyromancersManuscript != null && firedCard == lastPyromancersManuscript)
                {
                    lastPyromancersManuscriptFireCallback = new CallbackHandler();
                    _transitManager.TransitCard(firedCard, handFrom, firePoolTo, () => lastPyromancersManuscriptFireCallback.Complete(), index: realIndex);
                }
                else
                {
                    if (firedCard == lastCard)
                    {
                        lastCardFireCallback = new CallbackHandler();
                        _transitManager.TransitCard(firedCard, handFrom, firePoolTo, () => lastCardFireCallback.Complete(), index: realIndex);
                    }
                    else
                    {
                        _transitManager.TransitCard(firedCard, handFrom, firePoolTo, index: realIndex);
                    }
                }

                if (firedCard.IsPyromancersManuscript)
                {
                    indexOffset += 1; // -1 за счет минус карты, +2 за счет 2 пиромантов
                }
                else
                {
                    indexOffset -= 1; // -1 за счет минус карты
                }

                yield return new WaitForSeconds(0.2f);
            }

            if (lastCardFireCallback != null)
                yield return new WaitUntil(() => lastCardFireCallback.IsComplete);

            if (lastPyromancersManuscriptFireCallback != null)
                yield return new WaitUntil(() => lastPyromancersManuscriptFireCallback.IsComplete);

            yield break;
        }
    }
}