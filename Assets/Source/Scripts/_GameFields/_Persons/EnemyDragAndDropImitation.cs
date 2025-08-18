using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using Cards;

namespace GameFields.Persons
{
    public class EnemyDragAndDropImitation: PersonStep
    {
        private const int CountLogics = 2;
        private const float SelectYDirection = 1;
        private const float UnselectYDirection = -1;

        private readonly Hand _hand;
        private readonly CardDragAndDropImitationActions _cardImitationActions;
        private readonly EnemyDragAndDropImitationData _data;

        private bool _isComplete;

        internal EnemyDragAndDropImitation(CardDragAndDropImitationActions cardImitationActions, EnemyDragAndDropImitationData data,
            InteractionActivator interactionActivator, Hand hand): base(interactionActivator)
        {
            _isComplete = false;
            _data = data;
            _hand = hand;
            _cardImitationActions = cardImitationActions;
        }

        public int CountDrawCards => _data.CountDrawCards;
        public float DrawCardsDelay => _data.DrawCardsDelay;
        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;
            int logicNumber = Random.Range(1, CountLogics + 1);

            if (logicNumber == 1)
            {
                if (_hand.TryGetCard(out Card card))
                {
                    _cardImitationActions.SetCard(card);
                    //DragAndDropBehaviour1().ToUniTask();
                    DragAndDropBehaviour2().ToUniTask();
                }
                else
                {
                    _isComplete = true;
                }
            }

            if (logicNumber == 2)
            {
                if (_hand.TryGetCard(out Card card))
                {
                    _cardImitationActions.SetCard(card);
                    DragAndDropBehaviour2().ToUniTask();
                }
                else
                {
                    _isComplete = true;
                }
            }
        }

        private IEnumerator DragAndDropBehaviour1()
        {
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

            //_cardImitationActions.MoveOnPlace(_data.CardTranslateInDropPlaceTime);

            //if (_cardImitationActions.CanPlay() == false)
            //{
            //    _cardImitationActions.ReturnInHand(_data.CardReturnInHandTime);
            //    yield return new WaitForSeconds(_data.CardReturnInHandTime);
            //}
            //else
            //{
            //    yield return _cardImitationActions.Play();
            //}

            _cardImitationActions.Attack();

            yield return new WaitForSeconds(_data.EndTurnDelay);

            _isComplete = true;
        }

        private IEnumerator DragAndDropBehaviour2()
        {
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

            _cardImitationActions.MoveOnPlace(_data.CardTranslateInDropPlaceTime);

            if (_cardImitationActions.CanPlay() == false)
            {
                _cardImitationActions.ReturnInHand(_data.CardReturnInHandTime);
                yield return new WaitForSeconds(_data.CardReturnInHandTime);
            }
            else
            {
                yield return _cardImitationActions.Play();
            }

            yield return new WaitForSeconds(_data.EndTurnDelay);

            _isComplete = true;
        }
    }
}