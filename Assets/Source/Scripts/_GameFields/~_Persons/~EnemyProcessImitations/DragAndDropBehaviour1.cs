using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
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
            Card targetCard, CancellationToken fightToken) : base(fightToken)
        {
            _data = data;
            _cardImitationActions = cardImitationActions;
            _targetCard = targetCard;
        }

        protected override async UniTask OnActivating(CancellationToken token)
        {
            _cardImitationActions.SetCard(_targetCard);

            float startDelay = Random.Range(_data.StartDelayMin, _data.StartDelayMax);
            float countRepeat = Random.Range(0, _data.MaxCountRepeat + 1);

            await UniTask.WaitForSeconds(startDelay, cancellationToken: token);

            for (int i = 0; i < countRepeat + 1; i++)
            {
                float cardViewDelay = Random.Range(_data.CardViewDelayMin, _data.CardViewDelayMax);

                _cardImitationActions.ViewCard(_data.CardViewTime, SelectYDirection);
                await UniTask.WaitForSeconds(_data.CardViewTime + cardViewDelay, cancellationToken: token);

                if (i != countRepeat)
                {
                    _cardImitationActions.ViewCard(_data.CardViewTime, UnselectYDirection);
                    await UniTask.WaitForSeconds(_data.CardViewTime, cancellationToken: token);
                }
            }
        }
    }
}