using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Persons;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;

namespace GameFields.Effects
{
    public class SharperEffect : Effect
    {
        private const string DiscoverDeckMessage = "Выберете, какую карту возьмете";
        private const string DiscoverHandMessage = "Выберете, какую карту положите на её место";

        private const int CountDeckCards = 3;
        private const int CountHandCards = 3;

        private readonly Person _activePerson;
        private readonly IDrawCardManager _drawCardManager;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public SharperEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _drawCardManager = data.ActivePerson;
            _typesRoot = typesRoot;

            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        protected override string GetName() => nameof(SharperEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Шулера закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            //ViewType hand = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;
            //ViewType hand = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, true);
            HandTypes activePersonHandTypes = _typesRoot.GetPersonTypes(_activePerson).Hand;
            ViewType handView = activePersonHandTypes.ViewType;

            if (_viewRoot.TryViewDeckLastCards(out IReadOnlyList<Card> deckCards, CountDeckCards) == false)
            {
                return;
            }

            if (_viewRoot.TryView(out IReadOnlyList<Card> handCards, CountHandCards, handView) == false)
            {
                return;
            }

            DiscoverResult deckResult = new DiscoverResult();
            _activePerson.DiscoverCards(deckCards, DiscoverDeckMessage, deckResult);

            await UniTask.WaitUntil(() => deckResult.IsComplete, cancellationToken: Token);

            Card cardFromDeck = (Card)deckResult.Result;

            DiscoverResult handResult = new DiscoverResult();
            _activePerson.DiscoverCards(handCards, DiscoverHandMessage, handResult);

            await UniTask.WaitUntil(() => handResult.Result != null, cancellationToken: Token);

            Card cardFromHand = (Card)handResult.Result;

            bool isDraw = false;
            bool isTransit = false;

            int indexHand = _viewRoot.IndexOf(handView, cardFromHand);
            int indexDeck = _drawCardManager.DrawCard(cardFromDeck, Token, () => isDraw = true, index: indexHand);

            TransitFromType handFrom = activePersonHandTypes.FromType;

            _transitManager.TransitCard(cardFromHand, handFrom, TransitToType.Deck, () => isTransit = true, index: indexDeck);

            await UniTask.WaitUntil(() => isDraw && isTransit, cancellationToken: Token);
        }
    }
}