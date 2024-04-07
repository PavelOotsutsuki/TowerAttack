using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFields.Persons.PersonAnimators
{
    public class EnemyDragAndDropImitation : MonoBehaviour
    {
        private const int CountLogics = 1;
        private const float SelectYDirection = -1;
        private const float UnselectYDirection = 1;

        [SerializeField] private float _startDelayMin = 1f;
        [SerializeField] private float _startDelayMax = 2f;
        [SerializeField] private float _cardViewTime = 1f;
        [SerializeField] private float _cardViewDelayMin = 2f;
        [SerializeField] private float _cardViewDelayMax = 4f;
        [SerializeField] private float _cardTranslateInDropPlaceTime = 0.5f;
        [SerializeField] private float _cardReturnInHandTime = 0.5f;
        [SerializeField] private float _endTurnDelay = 2f;
        [SerializeField] private int _maxCountRepeat = 1;

        private IEndTurnHandler _endTurnHandler;
        private CardDragAndDropImitationActions _cardImitationActions;

        internal void Init(IEndTurnHandler endTurnHandler, CardDragAndDropImitationActions cardImitationActions)
        {
            _endTurnHandler = endTurnHandler;
            _cardImitationActions = cardImitationActions;
        }

        internal void StartDragAndDropAnimation()
        {
            int logicNumber = Random.Range(1, CountLogics + 1);

            if (logicNumber == 1)
            {
                DragAndDropBehaviour1().ToUniTask();
            }
        }

        private IEnumerator DragAndDropBehaviour1()
        {
            float startDelay = Random.Range(_startDelayMin, _startDelayMax);
            float countRepeat = Random.Range(0, _maxCountRepeat + 1);

            yield return new WaitForSeconds(startDelay);

            for (int i = 0; i < countRepeat + 1; i++)
            {
                float cardViewDelay = Random.Range(_cardViewDelayMin, _cardViewDelayMax);

                _cardImitationActions.ViewCard(_cardViewTime, SelectYDirection);
                yield return new WaitForSeconds(_cardViewTime + cardViewDelay);

                if (i != countRepeat)
                {
                    _cardImitationActions.ViewCard(_cardViewTime, UnselectYDirection);
                    yield return new WaitForSeconds(_cardViewTime);
                }
            }

            _cardImitationActions.PlayOnPlace(_cardTranslateInDropPlaceTime);
            yield return new WaitForSeconds(_cardTranslateInDropPlaceTime);

            if (_cardImitationActions.TryReturnToHand(_cardReturnInHandTime))
            {
                yield return new WaitForSeconds(_cardReturnInHandTime);
            }

            yield return new WaitForSeconds(_endTurnDelay);

            _endTurnHandler.OnEndTurn();
        }
    }
}