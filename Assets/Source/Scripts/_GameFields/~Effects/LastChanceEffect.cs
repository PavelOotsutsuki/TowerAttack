using Cards;
using System.Collections.Generic;
using GameFields.CardTransits;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace GameFields.Effects
{
    public class LastChanceEffect : Effect
    {
        private const float StartDuration = 0.5f;

        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public LastChanceEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager,
             ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _viewRoot = viewRoot;
            _transitManager = transitManager;
            _typesRoot = typesRoot;

            Play();
        }

        protected override string GetName() => nameof(LastChanceEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Последнего шанса закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            DiscardPileTypes discardPileTypes = _typesRoot.DiscardPile;
            IReadOnlyList<Card> discardPileCards = _viewRoot.GetAllCards(discardPileTypes.ViewType).ToList();

            if (discardPileCards.Count <= 0)
                return;

            TransitFromType discardPileFrom = discardPileTypes.FromType;
            TransitToType deckTo = _typesRoot.Deck.ToType;

            Card lastCard = discardPileCards.Last(); // Ищем последнюю карту чтобы не делать сто миллионов Shuffle Deck
            float duration = StartDuration;

            foreach (Card returnableCard in discardPileCards)
            {
                if (returnableCard != lastCard)
                {
                    _transitManager.TransitCard(returnableCard, discardPileFrom, deckTo, index: 0);
                }
                else
                {
                    _transitManager.TransitCard(returnableCard, discardPileFrom, deckTo); //Без индекса чтобы триггернуть Shuffle
                }
                await UniTask.WaitForSeconds(duration, cancellationToken: Token);
                duration = NextDuration(duration);
            }
        }

        private float NextDuration(float currentDuration)
        {
            return currentDuration - currentDuration / 10f;
        }
    }
}