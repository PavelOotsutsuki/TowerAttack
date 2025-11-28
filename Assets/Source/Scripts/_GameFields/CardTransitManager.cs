using System;
using System.Collections;
using Cards;
using Cards.Views;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.Decks;
using GameFields.DiscardPiles;
using GameFields.Persons;
using GameFields.Persons.Fires;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using Tools;
using UnityEngine;

namespace GameFields
{
    public class CardTransitManager
    {
        private readonly ITransitable _playerHand;
        private readonly ITowerTransitable _playerTower;
        private readonly ITransitable _enemyHand;
        private readonly ITowerTransitable _enemyTower;
        private readonly ITransitable _deck;
        private readonly ITransitable _discardPile;
        private readonly ICardTakable _fireRoot;
        private readonly IFirePoolSeatable _playerFirePool;
        private readonly IFirePoolSeatable _enemyFirePool;

        public CardTransitManager(HandPlayer playerHand, HandAI enemyHand, Tower playerTower, Tower enemyTower, Deck deck,
            DiscardPile discardPile, FireRoot fireRoot, FirePool playerFirePool, FirePool enemyFirePool)
        {
            _playerHand = playerHand;
            _playerTower = playerTower;

            _enemyHand = enemyHand;
            _enemyTower = enemyTower;

            _deck = deck;
            _discardPile = discardPile;

            _fireRoot = fireRoot;

            _playerFirePool = playerFirePool;
            _enemyFirePool = enemyFirePool;
        }

        //public void InsertIntoDeck(Card card, int index, TransitFromType from)
        //{
        //    ICardTakable takable = from switch
        //    {
        //        TransitFromType.DiscardPile => _discardPile,
        //        TransitFromType.HandEnemy => _enemyHand,
        //        TransitFromType.HandPlayer => _playerHand,
        //        TransitFromType.FireRoot => _fireRoot,
        //        _ => throw new Exception($"Ошибка нахождения типа {typeof(TransitFromType)}: {from}")
        //    };

        //    if (takable.TryTakeAwayCard(card) == false)
        //        throw new Exception("Ошибка: не найдена карта в from");

        //    _deck.SeatCard(card, index);
        //}

        public void TransitCard(Card card, TransitFromType from, TransitToType to, Action callback = null, int index = -1)
        {
            TransitingCard(card, from, to, index, callback).ToUniTask();
        }

        private IEnumerator TransitingCard(Card card, TransitFromType from, TransitToType to, int index, Action callback)
        {
            ICardTakable takable = from switch
            {
                TransitFromType.DiscardPile => _discardPile,
                TransitFromType.HandEnemy => _enemyHand,
                TransitFromType.HandPlayer => _playerHand,
                TransitFromType.FireRoot => _fireRoot,
                _ => throw new Exception($"Ошибка нахождения типа {typeof(TransitFromType)}: {from}")
            };

            ICardSeatable seatable = to switch
            {
                TransitToType.Deck => _deck,
                TransitToType.HandPlayer => _playerHand,
                TransitToType.HandEnemy => _enemyHand,
                TransitToType.DiscardPile => _discardPile,
                TransitToType.PlayerFirePool => null,
                TransitToType.EnemyFirePool => null,
                _ => throw new Exception($"Ошибка нахождения типа {typeof(TransitToType)}: {to}")
            };

            if (from == TransitFromType.FireRoot &&
                (to == TransitToType.PlayerFirePool || to == TransitToType.EnemyFirePool))
                throw new Exception("Ошибка: попытка положить карту из FirePool-а в FirePool. Зачем???");

            if (to == TransitToType.PlayerFirePool || to == TransitToType.EnemyFirePool)
            {
                CallbackHandler callbackHandler = new CallbackHandler();
                card.Fire(new WaitForSeconds(0.1f), callbackHandler);

                yield return new WaitUntil(() => callbackHandler.IsComplete);

                IFirePoolSeatable fireSeatable = to switch
                {
                    TransitToType.PlayerFirePool => _playerFirePool,
                    TransitToType.EnemyFirePool => _enemyFirePool,
                    _ => throw new Exception($"Ошибка нахождения {typeof(IFirePoolSeatable)} типа {typeof(TransitToType)}: {to}")
                };

                if (takable.TryTakeAwayCard(card) == false)
                    throw new Exception("Ошибка: не найдена карта в from");

                ICardSeatable fireNewCardsSeatable = from switch
                {
                    TransitFromType.DiscardPile => _discardPile,
                    TransitFromType.HandEnemy => _enemyHand,
                    TransitFromType.HandPlayer => _playerHand,
                    _ => throw new Exception($"Ошибка нахождения типа {typeof(TransitFromType)}: {from}")
                };

                CallbackHandler callbackHandlerSeatInFirePool = new CallbackHandler();
                fireSeatable.SeatCard(card, fireNewCardsSeatable, index, callbackHandlerSeatInFirePool);

                yield return new WaitUntil(() => callbackHandlerSeatInFirePool.IsComplete);
                callback?.Invoke();
                yield break;
            }

            if (takable.TryTakeAwayCard(card) == false)
                throw new Exception("Ошибка: не найдена карта в from");

            if (from == TransitFromType.FireRoot)
            {
                WaitUntilSeat(seatable, card, 1.3f, callback).ToUniTask();
            }
            else
            {
                seatable.SeatCard(card, index);
                callback?.Invoke();
            }
        }


        public bool TryExchangeTower(Card cardToTower, IPersonObject exchangeObject, TowerTransitType transitType)
        {
            ITowerTransitable exchangedTower = exchangeObject is IPlayerObject ? _playerTower : _enemyTower;
            ITransitable transitable = transitType switch
            {
                TowerTransitType.Deck => _deck,
                TowerTransitType.Hand => exchangeObject is IPlayerObject ? _playerHand : _enemyHand,
                _ => throw new NullReferenceException("TowerTransitType is not founded")
            };

            if (exchangedTower.TryTakeAwayCard(out Card towerCard) == false)
                return false;

            transitable.SeatCard(towerCard);

            if (transitable.TryTakeAwayCard(cardToTower) == false)
                return false;

            exchangedTower.SeatCard(cardToTower);

            return true;
        }

        private IEnumerator WaitUntilSeat(ICardSeatable seatable, Card card, float duration, Action callback)
        {
            yield return new WaitForSeconds(duration);

            InvertCardAnimationData invertCardAnimationData = new InvertCardAnimationData(0.5f, 0.5f, 0.5f, false, SideType.Front);
            InvertCardAnimation invertCardAnimation = new InvertCardAnimation(invertCardAnimationData);
            invertCardAnimation.Play(card);

            yield return new WaitUntil(() => invertCardAnimation.IsComplete);

            seatable.SeatCard(card);
            callback?.Invoke();
        }
    }
}