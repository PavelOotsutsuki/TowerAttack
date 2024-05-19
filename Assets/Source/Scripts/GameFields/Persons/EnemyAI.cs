using Cards;
using GameFields.DiscardPiles;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using System.Collections;
using UnityEngine;

namespace GameFields.Persons
{
    //[Serializable]
    public class EnemyAI : Person
    {
        //[SerializeField] private EnemyAiData _enemyAiData;

        //[SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        private DiscoverImitation _discoverImitation;
        private ITableDeactivator _tableDeactivator;

        private CardDragAndDropImitationActions _cardDragAndDropImitationActions;

        private bool _isComplete;
        private readonly MonoBehaviour _coroutineContainer;

        public bool IsComplete => _isComplete;
        //public bool IsImitationComplete => StartTurnDraw.IsComplete && Imitation.IsComplete && EffectCard.IsComplete;

        public EnemyAI(DiscardPile discardPile, ITableDeactivator tableDeactivator, EnemyDragAndDropImitation enemyDragAndDropImitation,
            HandAI hand, Table table, Tower tower, DrawCardRoot drawCardRoot, DiscoverImitation discoverImitation,
            StartTurnDraw startTurnDraw, MonoBehaviour coroutineContainer)
            : base(hand, table, drawCardRoot, tower, discardPile, startTurnDraw)
        {
            _enemyDragAndDropImitation = enemyDragAndDropImitation;
            _discoverImitation = discoverImitation;
            _tableDeactivator = tableDeactivator;
            _coroutineContainer = coroutineContainer;

            _cardDragAndDropImitationActions = new CardDragAndDropImitationActions(Hand, Table);
        }

        public override void Init()
        {
            _enemyDragAndDropImitation.Init(_cardDragAndDropImitationActions, CompleteImitation, _coroutineContainer);
        }

        public override void StartTurn()
        {
            _tableDeactivator.Deactivate();

            _isComplete = false;

            //if (cards.Count > 0)
            //{
            _coroutineContainer.StartCoroutine(ProcessingTurn());
            //}
            //else
            //{
            //    PlayDragAndDropImitation();
            //}
        }

        private void PlayDragAndDropImitation()
        {
            if (Hand.TryGetCard(out Card card))
            {
                _cardDragAndDropImitationActions.SetCard(card);
                _enemyDragAndDropImitation.StartDragAndDropAnimation();
            }
            else
            {
                CompleteImitation();
            }
        }

        private void CompleteImitation()
        {
            _isComplete = true;
        }

        private IEnumerator ProcessingTurn()
        {
            //StartTurnDraw.PrepareToStart();
            StartTurnDraw.StartStep();

            yield return new WaitUntil(() => StartTurnDraw.IsComplete);

            PlayDragAndDropImitation();
        }
    }
}
