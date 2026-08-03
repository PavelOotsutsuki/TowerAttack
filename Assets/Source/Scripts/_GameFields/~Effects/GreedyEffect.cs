using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using System.Collections.Generic;
using System.Linq;

namespace GameFields.Effects
{
    public class GreedyEffect : Effect
    {
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public GreedyEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager, EffectData data)
            : base(data)
        {
            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        protected override string GetName() => nameof(GreedyEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Жадины закончен");
        //}

        protected override UniTask OnPlaying()
        {
            IEnumerable<Card> enemyCards = _viewRoot.GetAllCards(ViewType.HandAI);
            IEnumerable<Card> playerCards = _viewRoot.GetAllCards(ViewType.HandPlayer);

            GetCards(playerCards, TransitFromType.HandPlayer, TransitToType.HandEnemy);
            GetCards(enemyCards, TransitFromType.HandEnemy, TransitToType.HandPlayer);

            return UniTask.CompletedTask;
        }

        private void GetCards(IEnumerable<Card> givenCards, TransitFromType fromType, TransitToType toType)
        {
            if (givenCards is not null)
            {
                if (givenCards.Count() > 0)
                {
                    foreach (Card card in givenCards)
                    {
                        _transitManager.TransitCard(card, fromType, toType);
                    }
                }
            }
        }
    }
}