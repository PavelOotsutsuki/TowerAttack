using System.Collections.Generic;
using System.Linq;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class DumbMonkEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public DumbMonkEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _viewRoot = viewRoot;
            _transitManager = transitManager;
            _typesRoot = typesRoot;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Глупого Монаха закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            //ViewType viewType = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;
            //ViewType viewType = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, true);
            PersonTypes activePersonTypes = _typesRoot.GetPersonTypes(_activePerson);
            ViewType handView = activePersonTypes.Hand.ViewType;
            IReadOnlyList<Card> cards = _viewRoot.GetAllCards(handView).ToList();

            if (cards.Count <= 0)
            {
                return;
            }

            TransitFromType handFrom = activePersonTypes.Hand.FromType;
            TransitToType firePoolTo = activePersonTypes.FirePool;

            //if (viewType == ViewType.HandPlayer)
            //{
            //    transitFromType = TransitFromType.HandPlayer;
            //    transitToType = TransitToType.PlayerFirePool;
            //}
            //else
            //{
            //    transitFromType = TransitFromType.HandEnemy;
            //    transitToType = TransitToType.EnemyFirePool;
            //}
            Card firedCard = cards[0];
            int index = 0; // Логично

            bool isTransit = false;
            _transitManager.TransitCard(firedCard, handFrom, firePoolTo, () => isTransit = true, index);
            //_activePerson.ActivateFateInevitability(_duration);

            await UniTask.WaitUntil(() => isTransit, cancellationToken: Token);
        }
    }
}