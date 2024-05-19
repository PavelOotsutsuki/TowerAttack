using System.Collections;
using UnityEngine;

namespace GameFields.Persons
{
    [System.Serializable]
    public class EnemyDragAndDropImitation
    {
        private const int CountLogics = 1;
        private const float SelectYDirection = 1;
        private const float UnselectYDirection = -1;

        [SerializeField] private float _startDelayMin = 1f;
        [SerializeField] private float _startDelayMax = 2f;
        [SerializeField] private float _cardViewTime = 1f;
        [SerializeField] private float _cardViewDelayMin = 2f;
        [SerializeField] private float _cardViewDelayMax = 4f;
        [SerializeField] private float _cardTranslateInDropPlaceTime = 0.5f;
        [SerializeField] private float _cardReturnInHandTime = 0.5f;
        [SerializeField] private float _endTurnDelay = 2f;
        [SerializeField] private int _maxCountRepeat = 2;
        [SerializeField] private int _countDrawCards = 1;
        [SerializeField] private float _drawCardsDelay = 0.5f;
        
        private MonoBehaviour _coroutineContainer;

        public int CountDrawCards => _countDrawCards;
        public float DrawCardsDelay => _drawCardsDelay;
        //public bool IsComplete => _isComplete;

        //private IEndTurnHandler _endTurnHandler;

        private System.Action _callback;
        private CardDragAndDropImitationActions _cardImitationActions;

        internal void Init(CardDragAndDropImitationActions cardImitationActions, System.Action callback, MonoBehaviour coroutineContainer)
        {
            //_endTurnHandler = endTurnHandler;
            _callback = callback;
            _cardImitationActions = cardImitationActions;
            _coroutineContainer = coroutineContainer;
        }

        internal void StartDragAndDropAnimation()
        {
            int logicNumber = Random.Range(1, CountLogics + 1);

            if (logicNumber == 1)
            {
                _coroutineContainer.StartCoroutine(DragAndDropBehaviour1());
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

            //_endTurnHandler.OnEndTurn();
            _callback?.Invoke();
        }
    }
}