using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using Cards;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using Tools.Utils;
using GameFields.Persons;
using GameFields.Persons.EffectHandlers;
using GameFields.InputSettings;
using Cards.Views;
using Tools.InputSettings;
using System.Linq;

namespace GameFields.Persons.EnemyProcessImitations
{
    public class EnemyDragAndDropImitation: PersonStep, IInputLogicObject
    {
        private const int CountLogics = 1;
        //private const float SelectYDirection = 1;
        //private const float UnselectYDirection = -1;

        private readonly Hand _hand;
        private readonly CardDragAndDropImitationActions _cardImitationActions;
        private readonly EnemyDragAndDropImitationData _data;
        private readonly SkipTurnChecker _skipTurnChecker;
        private readonly IDrawnCardWatcher _drawnCardWatcher;
        private readonly GnomeEffectHandler _gnomeEffectHandler;
        private readonly IAIThinkLogic _thinkLogic; 

        private bool _isComplete;

        internal EnemyDragAndDropImitation(CardDragAndDropImitationActions cardImitationActions, EnemyDragAndDropImitationData data,
            InteractionActivator interactionActivator, SkipTurnChecker skipTurnChecker, IDrawnCardWatcher drawnCardWatcher, Hand hand,
            IAIThinkLogic AIThinkLogic, GnomeEffectHandler gnomeEffectHandler) : base(interactionActivator)
        {
            _isComplete = false;
            _data = data;
            _hand = hand;
            _cardImitationActions = cardImitationActions;
            _skipTurnChecker = skipTurnChecker;
            _drawnCardWatcher = drawnCardWatcher;
            _gnomeEffectHandler = gnomeEffectHandler;
            _thinkLogic = AIThinkLogic;
        }

        public int CountDrawCards => _data.CountDrawCards;
        public float DrawCardsDelay => _data.DrawCardsDelay;
        public override bool IsComplete => _isComplete;

        private IEnumerator Skipping()
        {
            yield return new WaitForSeconds(1f);
            _isComplete = true;
        }

        protected override void OnStartStep()
        {
            _isComplete = false;

            Debug.Log("EnemyDragAndDropImitation.OnStartStep()");

            _skipTurnChecker.Activate();

            if (_skipTurnChecker.CanSkip)
            {
                //_isComplete = true;
                Skipping().ToUniTask();
                return;
            }



            //int logicNumber = Random.Range(1, CountLogics + 1);

            List<Card> workCardList;

            if (_hand.IsSlimeEffect)
            {
                workCardList = Utils.Shuffle(_drawnCardWatcher.DrawnCards);
                Debug.Log("_drawnCardWatcher.DrawnCards: " + _drawnCardWatcher.DrawnCards.Count());
            }
            else
            {
                workCardList = Utils.Shuffle(_hand.AllCards);
                Debug.Log("_hand.AllCards: " + _hand.AllCards.Count());
            }

            //List<Func<IEnumerator>> enableAIEndLogic = new List<Func<IEnumerator>>();

            Dictionary<Card, CardCapability> cardActions = new Dictionary<Card, CardCapability>();

            foreach (Card workCard in workCardList)
            {
                //enableAIEndLogic.Clear();

                CardCapability currentCapability = workCard.CardCapability;

                if (_cardImitationActions.CanPlay() == false && currentCapability == CardCapability.Play)
                    continue;

                //if (_cardImitationActions.CanPlay() == false)
                //    currentCapability &= ~CardCapability.Play;

                CardCapability type = _thinkLogic.FindActionType(workCard);
                cardActions.Add(workCard, type);
            }

            if (cardActions.Count > 0)
            {
                if (cardActions.ContainsValue(CardCapability.Attack) && cardActions.Values.Distinct().Count() > 1)
                {
                    foreach (KeyValuePair<Card, CardCapability> keyValuePair in cardActions)
                    {
                        if (keyValuePair.Value == CardCapability.Attack)
                            workCardList.Remove(keyValuePair.Key);
                    }
                }

                Card workCard = workCardList[0];
                CardCapability type = cardActions[workCard];

                Func<IEnumerator> endAction = type switch
                {
                    CardCapability.Attack => Attack,
                    CardCapability.Play => Play,
                    CardCapability.GnomeForging => Forging,
                    CardCapability.HandTransfer => HandTransfer,
                    _ => throw new Exception("Найден неизвестный CardCapability: " + type)
                };

                int logicNumber = Random.Range(1, CountLogics + 1);

                DragAndDropBehaviour dragAndDropBehaviour = logicNumber switch
                {
                    1 => new DragAndDropBehaviour1(_data, _cardImitationActions, workCard),
                    _ => throw new NullReferenceException("Задан неверный индекс логики поведения Enemy: " + logicNumber)
                };

                Processing(dragAndDropBehaviour, endAction).ToUniTask();

                return;
            }

            //foreach (Card workCard in workCardList)
            //{
            //    //enableAIEndLogic.Clear();

            //    CardCapability currentCapability = workCard.CardCapability;

            //    if (_cardImitationActions.CanPlay() == false && currentCapability == CardCapability.Play)
            //        continue;

            //    //if (_cardImitationActions.CanPlay() == false)
            //    //    currentCapability &= ~CardCapability.Play;

            //    CardCapability type = _thinkLogic.FindActionType(workCard);

            //    //if ((currentCapability & CardCapability.Attack) == CardCapability.Attack)
            //    //{
            //    //    enableAIEndLogic.Add(Attack);
            //    //}

            //    //if ((currentCapability & CardCapability.Play) == CardCapability.Play && _cardImitationActions.CanPlay())
            //    //{
            //    //    if ((currentCapability & CardCapability.GnomeChoice) == CardCapability.GnomeChoice)
            //    //    {
            //    //        if (_gnomeEffectHandler.CanActivate())
            //    //        {
            //    //            enableAIEndLogic.Add(Play);
            //    //        }
            //    //    }
            //    //    else
            //    //    {
            //    //        enableAIEndLogic.Add(Play);
            //    //    }
            //    //}

            //    //if ((currentCapability & CardCapability.GnomeForging) == CardCapability.GnomeForging)
            //    //{
            //    //    enableAIEndLogic.Add(Forging);
            //    //}

            //    //if ((currentCapability & CardCapability.HandTransfer) == CardCapability.HandTransfer)
            //    //{
            //    //    enableAIEndLogic.Add(HandTransfer);
            //    //}

            //    //if (enableAIEndLogic.Count == 0)
            //    //    continue;

            //    //int endActionIndex = Random.Range(0, enableAIEndLogic.Count);

            //    Func<IEnumerator> endAction = type switch
            //    {
            //        CardCapability.Attack => Attack,
            //        CardCapability.Play => Play,
            //        CardCapability.GnomeForging => Forging,
            //        CardCapability.HandTransfer => HandTransfer,
            //        _ => throw new Exception("Найден неизвестный CardCapability: " + type)
            //    };

            //    int logicNumber = Random.Range(1, CountLogics + 1);

            //    DragAndDropBehaviour dragAndDropBehaviour = logicNumber switch
            //    {
            //        1 => new DragAndDropBehaviour1(_data, _cardImitationActions, workCard),
            //        _ => throw new NullReferenceException("Задан неверный индекс логики поведения Enemy: " + logicNumber)
            //    };

            //    Processing(dragAndDropBehaviour, endAction).ToUniTask();

            //    //_cardImitationActions.SetCard(workCard);

            //    //if (endActionIndex == 1)
            //    //{
            //    //    //DragAndDropBehaviour1().ToUniTask();
            //    //    DragAndDropBehaviour2().ToUniTask();
            //    //}

            //    //if (endActionIndex == 2)
            //    //{
            //    //    DragAndDropBehaviour2().ToUniTask();
            //    //}

            //    return;
            //}

            _isComplete = true;
        }

        private IEnumerator Attack()
        {
            _cardImitationActions.Attack();

            yield break;
        }

        private IEnumerator Play()
        {
            _cardImitationActions.MoveOnPlace(_data.CardTranslateInDropPlaceTime);

            if (_cardImitationActions.CanPlay())
            {
                yield return _cardImitationActions.Play();
            }
            else
            {
                yield return _cardImitationActions.ReturningInHand(_data.CardReturnInHandTime);
            }
        }

        private IEnumerator Forging()
        {
            _gnomeEffectHandler.Upgrade();

            yield return _cardImitationActions.Forging();
        }

        private IEnumerator HandTransfer()
        {
            _cardImitationActions.HandTransfer();

            yield break;
        }

        private IEnumerator Processing(DragAndDropBehaviour dragAndDropBehaviour, Func<IEnumerator> endAction)
        {
            dragAndDropBehaviour.Activate();

            yield return new WaitUntil(() => dragAndDropBehaviour.IsComplete);

            yield return endAction.Invoke();

            yield return new WaitForSeconds(_data.EndTurnDelay);
            //yield return new WaitForSeconds(5f);

            _isComplete = true;
        }

        //private IEnumerator DragAndDropBehaviour1()
        //{
        //    float startDelay = Random.Range(_data.StartDelayMin, _data.StartDelayMax);
        //    float countRepeat = Random.Range(0, _data.MaxCountRepeat + 1);

        //    yield return new WaitForSeconds(startDelay);

        //    for (int i = 0; i < countRepeat + 1; i++)
        //    {
        //        float cardViewDelay = Random.Range(_data.CardViewDelayMin, _data.CardViewDelayMax);

        //        _cardImitationActions.ViewCard(_data.CardViewTime, SelectYDirection);
        //        yield return new WaitForSeconds(_data.CardViewTime + cardViewDelay);

        //        if (i != countRepeat)
        //        {
        //            _cardImitationActions.ViewCard(_data.CardViewTime, UnselectYDirection);
        //            yield return new WaitForSeconds(_data.CardViewTime);
        //        }
        //    }

        //    //_cardImitationActions.MoveOnPlace(_data.CardTranslateInDropPlaceTime);

        //    //if (_cardImitationActions.CanPlay() == false)
        //    //{
        //    //    _cardImitationActions.ReturnInHand(_data.CardReturnInHandTime);
        //    //    yield return new WaitForSeconds(_data.CardReturnInHandTime);
        //    //}
        //    //else
        //    //{
        //    //    yield return _cardImitationActions.Play();
        //    //}

        //    _cardImitationActions.Attack();

        //    yield return new WaitForSeconds(_data.EndTurnDelay);

        //    _isComplete = true;
        //}

        //private IEnumerator DragAndDropBehaviour2()
        //{
        //    float startDelay = Random.Range(_data.StartDelayMin, _data.StartDelayMax);
        //    float countRepeat = Random.Range(0, _data.MaxCountRepeat + 1);

        //    yield return new WaitForSeconds(startDelay);

        //    for (int i = 0; i < countRepeat + 1; i++)
        //    {
        //        float cardViewDelay = Random.Range(_data.CardViewDelayMin, _data.CardViewDelayMax);

        //        _cardImitationActions.ViewCard(_data.CardViewTime, SelectYDirection);
        //        yield return new WaitForSeconds(_data.CardViewTime + cardViewDelay);

        //        if (i != countRepeat)
        //        {
        //            _cardImitationActions.ViewCard(_data.CardViewTime, UnselectYDirection);
        //            yield return new WaitForSeconds(_data.CardViewTime);
        //        }
        //    }

        //    _cardImitationActions.MoveOnPlace(_data.CardTranslateInDropPlaceTime);

        //    if (_cardImitationActions.CanPlay() == false)
        //    {
        //        _cardImitationActions.ReturnInHand(_data.CardReturnInHandTime);
        //        yield return new WaitForSeconds(_data.CardReturnInHandTime);
        //    }
        //    else
        //    {
        //        yield return _cardImitationActions.Play();
        //    }

        //    yield return new WaitForSeconds(_data.EndTurnDelay);

        //    _isComplete = true;
        //}
    }
}