using System.Collections;
using Cards;
using UnityEngine;

namespace GameFields.Persons.EnemyProcessImitations
{
    public class DragAndDropBehaviour1 : DragAndDropBehaviour
    {
        private const float SelectYDirection = 1;
        private const float UnselectYDirection = -1;

        private readonly EnemyDragAndDropImitationData _data;
        private readonly CardDragAndDropImitationActions _cardImitationActions;
        private readonly Card _targetCard;

        public DragAndDropBehaviour1(EnemyDragAndDropImitationData data, CardDragAndDropImitationActions cardImitationActions,
            Card targetCard) : base()
        {
            _data = data;
            _cardImitationActions = cardImitationActions;
            _targetCard = targetCard;
        }

        protected override IEnumerator OnActivating()
        {
            _cardImitationActions.SetCard(_targetCard);

            float startDelay = Random.Range(_data.StartDelayMin, _data.StartDelayMax);
            float countRepeat = Random.Range(0, _data.MaxCountRepeat + 1);

            yield return new WaitForSeconds(startDelay);

            for (int i = 0; i < countRepeat + 1; i++)
            {
                float cardViewDelay = Random.Range(_data.CardViewDelayMin, _data.CardViewDelayMax);

                _cardImitationActions.ViewCard(_data.CardViewTime, SelectYDirection);
                yield return new WaitForSeconds(_data.CardViewTime + cardViewDelay);

                if (i != countRepeat)
                {
                    _cardImitationActions.ViewCard(_data.CardViewTime, UnselectYDirection);
                    yield return new WaitForSeconds(_data.CardViewTime);
                }
            }
        }
    }
}