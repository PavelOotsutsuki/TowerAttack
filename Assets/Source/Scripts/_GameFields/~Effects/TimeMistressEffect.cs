using Cards;
using System.Collections.Generic;
using GameFields.Persons;
using GameFields.CardTransits;
using Cysharp.Threading.Tasks;

namespace GameFields.Effects
{
    public class TimeMistressEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public TimeMistressEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager,
             ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            _viewRoot = viewRoot;
            _transitManager = transitManager;
            _typesRoot = typesRoot;

            Play();
        }

        protected override string GetName() => nameof(TimeMistressEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Повелительницы времени закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            //TransitToType transitTo = _activePerson is Player ? TransitToType.HandPlayer : TransitToType.HandEnemy;
            //TransitToType transitTo = ViewTransitTypeConverter.GetPersonHandTransitToType(_activePerson, true);
            //ViewType discardPile = ViewType.DiscardPile;
            //TransitFromType transitFromType = ViewTransitTypeConverter.ConvertToTransitFromType(discardPile);
            TransitToType handTo = _typesRoot.GetPersonTypes(_activePerson).Hand.ToType;
            DiscardPileTypes discardPileTypes = _typesRoot.DiscardPile;

            ViewType discardPileView = discardPileTypes.ViewType;
            TransitFromType discardPileFrom = discardPileTypes.FromType;

            if (_viewRoot.TryView(out IReadOnlyList<Card> cards, 1, discardPileView))
            {
                Card card = cards[0];

                bool isTransit = false;

                _transitManager.TransitCard(card, discardPileFrom, handTo, () => isTransit = true);
                await UniTask.WaitUntil(() => isTransit, cancellationToken: Token);
            }
        }
    }
}