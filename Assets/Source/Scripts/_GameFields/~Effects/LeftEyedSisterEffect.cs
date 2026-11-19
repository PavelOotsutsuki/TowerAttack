using Cards;
using System.Collections.Generic;
using GameFields.Persons;
using GameFields.Persons.Discovers;
using GameFields.CardTransits;
using Cysharp.Threading.Tasks;

namespace GameFields.Effects
{
    public class LeftEyedSisterEffect : Effect
    {
        private const int CountDiscoverFromHand = 3;

        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public LeftEyedSisterEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            _viewRoot = viewRoot;
            _transitManager = transitManager;
            _typesRoot = typesRoot;

            Play();
        }

        protected override string GetName() => nameof(LeftEyedSisterEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Левоглазой сестры закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            //ViewType viewType = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;
            //ViewType viewType = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, true);
            ViewType handView = _typesRoot.GetPersonTypes(_activePerson).Hand.ViewType;

            //_activePerson

            //for (int i = CountDicoverFromHand; i > 0; i--)
            //{
            if (_viewRoot.TryView(out IReadOnlyList<Card> cards, CountDiscoverFromHand, handView))
            {
                DiscoverResult discoverResult = new DiscoverResult();
                _activePerson.DiscoverCards(cards, "Выберите новую карту в замок", discoverResult);

                await UniTask.WaitUntil(() => discoverResult.IsComplete, cancellationToken: Token);

                _transitManager.TryExchangeTower((Card)discoverResult.Result, _activePerson, TowerTransitType.Hand);
            }
            //}
        }
    }
}