using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.CardTransits;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionImitation: StartTowerCardSelection
    {
        private readonly IHandTransitTryGet _handTransitTryGet;
        private readonly IDrawCardManager _drawCardManager;

        private List<Card> _enemyCards;
        private int _firstTurnCardsCount;

        public StartTowerCardSelectionImitation(Person person, int firstTurnCardsCount) : base(person)
        {
            _handTransitTryGet = person;
            _drawCardManager = person;

            _firstTurnCardsCount = firstTurnCardsCount;
        }

        public override void StartProcess()
        {
            _enemyCards = _drawCardManager.DrawCards(_firstTurnCardsCount, StartingEnemyProcess);
        }

        private void StartingEnemyProcess()
        {
            StartingEnemyProcess(_enemyCards).ToUniTask();
        }

        private IEnumerator StartingEnemyProcess(List<Card> enemyCards)
        {
            yield return new WaitForSeconds(2f);

            for (int i = 0; i < _firstTurnCardsCount; i++)
            {
                yield return new WaitForSeconds(1f);
            }

            int selectedCardIndex = Random.Range(0, _firstTurnCardsCount);

            if (_handTransitTryGet.TryGet(enemyCards[selectedCardIndex]))
            {
                if (TowerTransitCheck.IsFill == false)
                {
                    TowerTransitSet.Set(enemyCards[selectedCardIndex]);
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
