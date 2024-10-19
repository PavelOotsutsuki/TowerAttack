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

        private readonly int _firstTurnCardsCount;
        private readonly StartTowerCardSelectionImitationData _data;

        private List<Card> _enemyCards;

        public StartTowerCardSelectionImitation(Person person, int firstTurnCardsCount, StartTowerCardSelectionImitationData data) : base(person)
        {
            _handTransitTryGet = person;
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

        private IEnumerator StartingEnemyProcess(List<Card> enemyCards)
        {
            yield return new WaitForSeconds(_data.WaitDurationBeforeStartActions);

            //for (int i = 0; i < _firstTurnCardsCount; i++)
            //{
            //    yield return new WaitForSeconds(1f);
            //}

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
