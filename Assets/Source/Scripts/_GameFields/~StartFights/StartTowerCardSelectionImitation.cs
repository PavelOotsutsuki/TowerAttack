using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionImitation: StartTowerCardSelection
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

        public override void StartProcess()
        {
            _enemyCards = _drawCardManager.DrawCards(_firstTurnCardsCount, StartEnemyProcess);
        }

        private void StartEnemyProcess()
        {
            StartingEnemyProcess(_enemyCards).ToUniTask();
        }

        private IEnumerator StartingEnemyProcess(IReadOnlyList<Card> enemyCards)
        {
            yield return new WaitForSeconds(_data.WaitDurationBeforeStartActions);

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