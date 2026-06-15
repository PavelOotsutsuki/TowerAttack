using Cards;
using System.Collections.Generic;
using GameFields.Persons;
using GameFields.CardTransits;
using Cysharp.Threading.Tasks;

namespace GameFields.Effects
{
    public class RightEyedSisterEffect : Effect
    {
        private const int CountCardsFromDeck = 1;

        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public RightEyedSisterEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Правоглазой сестры закончен");
        //}

        protected override UniTask OnPlaying()
        {
            ViewType viewType = ViewType.Deck;

            //_activePerson

            //for (int i = CountDicoverFromHand; i > 0; i--)
            //{
            if (_viewRoot.TryView(out IReadOnlyList<Card> cards, CountCardsFromDeck, viewType))
            {
                Card cardFromDeck = cards[0];

                _transitManager.TryExchangeTower(cardFromDeck, _activePerson, TowerTransitType.Deck);
            }
            //}
            return UniTask.CompletedTask;
        }
    }
}