using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using Cards;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using Tools.Utils;
using System.Linq;

namespace GameFields.Persons.Commons
{
    public class EnemyDragAndDropImitation: PersonStep
    {
        private const int CountLogics = 2;
        private const float SelectYDirection = 1;
        private const float UnselectYDirection = -1;

        private readonly Hand _hand;
        private readonly CardDragAndDropImitationActions _cardImitationActions;
        private readonly EnemyDragAndDropImitationData _data;
        private readonly SkipTurnChecker _skipTurnChecker;
        private readonly IDrawnCardWatcher _drawnCardWatcher;

        private bool _isComplete;

        internal EnemyDragAndDropImitation(CardDragAndDropImitationActions cardImitationActions, EnemyDragAndDropImitationData data,
            InteractionActivator interactionActivator, SkipTurnChecker skipTurnChecker, IDrawnCardWatcher drawnCardWatcher, Hand hand): base(interactionActivator)
        {
            _isComplete = false;
            _data = data;
            _hand = hand;
            _cardImitationActions = cardImitationActions;
            _skipTurnChecker = skipTurnChecker;
            _drawnCardWatcher = drawnCardWatcher;
        }

        public int CountDrawCards => _data.CountDrawCards;
        public float DrawCardsDelay => _data.DrawCardsDelay;
        public override bool IsComplete => _isComplete;

        protected override void OnStartStep()
        {
            _isComplete = false;

            _skipTurnChecker.Activate();

            if (_skipTurnChecker.CanSkip)
            {
                _isComplete = true;
                return;
            }

            int logicNumber = Random.Range(1, CountLogics + 1);

            IEnumerable<Card> workCardList;

            if (_hand.IsSlimeEffect)
            {
                workCardList = _drawnCardWatcher.DrawnCards;
            }
            else
            {
                workCardList = _hand.AllCards;
            }

            Card workCard = Utils.Shuffle(workCardList).First();

            _cardImitationActions.SetCard(workCard);

            if (logicNumber == 1)
            {
                //DragAndDropBehaviour1().ToUniTask();
                DragAndDropBehaviour2().ToUniTask();
            }

            if (logicNumber == 2)
            {
                DragAndDropBehaviour2().ToUniTask();
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