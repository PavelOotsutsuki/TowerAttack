using System;
using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.Decks;
using GameFields.DiscardPiles;
using GameFields.Persons.Commons;
using GameFields.Persons.Fires;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
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

        public CardTransitManager(HandPlayer playerHand, HandAI enemyHand, Tower playerTower, Tower enemyTower, Deck deck,
            DiscardPile discardPile, FireRoot fireRoot)
        {
            _playerHand = playerHand;
            _playerTower = playerTower;

            _enemyHand = enemyHand;
            _enemyTower = enemyTower;

            _deck = deck;
            _discardPile = discardPile;

            _fireRoot = fireRoot;
        }

        public bool TryTransitCard(Card card, TransitFromType from, TransitToType to, Action callback = null)
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
                _ => throw new Exception($"Ошибка нахождения типа {typeof(TransitToType)}: {to}")
            };

            if (takable.TryTakeAwayCard(card) == false)
                return false;

            if (from == TransitFromType.FireRoot)
            {
                WaitUntilSeat(seatable, card, 1.3f, callback).ToUniTask();
            }
            else
            {
                seatable.SeatCard(card);
            }

            return true;
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