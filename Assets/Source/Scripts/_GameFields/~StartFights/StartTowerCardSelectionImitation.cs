using System.Collections.Generic;
using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionImitation: StartTowerCardSelection, IEnemyAIObject
    {
        private readonly ICardTakable _hand;
        private readonly IDrawCardManager _drawCardManager;

        private readonly int _firstTurnCardsCount;
        private readonly StartTowerCardSelectionImitationData _data;

        private List<Card> _enemyCards;

        public StartTowerCardSelectionImitation(Person person, HandAI hand, TowerAI tower, int firstTurnCardsCount, StartTowerCardSelectionImitationData data) : base(tower)
        {
            _hand = hand;
            _drawCardManager = person;
            _data = data;

            _firstTurnCardsCount = firstTurnCardsCount;
        }

        public override void StartProcess(CancellationToken token)
        {
            _enemyCards = _drawCardManager.DrawCards(_firstTurnCardsCount, token, () => StartEnemyProcess(token));
        }

        private void StartEnemyProcess(CancellationToken token)
        {
            StartingEnemyProcess(_enemyCards, token).Forget();
        }

        private async UniTask StartingEnemyProcess(IReadOnlyList<Card> enemyCards, CancellationToken token)
        {
            await UniTask.WaitForSeconds(_data.WaitDurationBeforeStartActions, cancellationToken: token);

            int selectedCardIndex = Random.Range(0, _firstTurnCardsCount);

            if (_hand.TryTakeAwayCard(enemyCards[selectedCardIndex]))
            {
                if (Tower.HasFreeSeat)
                {
                    Tower.SeatCard(enemyCards[selectedCardIndex]);
                }
                else
                {
                    throw new System.Exception("Не удалость посадить в замок");
                }
            }
            else
            {
                throw new System.Exception("Не удалось найти карту в руке");
            }
        }
    }
}