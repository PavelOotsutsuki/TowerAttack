using System;
using Cards;
using GameFields.Decks;
using GameFields.DiscardPiles;
using GameFields.Persons.Commons;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;

namespace GameFields
{
    public class CardTransitManager
    {
        private readonly ITransitable _playerHand;
        private readonly ITowerTransitable _playerTower;
        private readonly ITransitable _enemyHand;
        private readonly ITowerTransitable _enemyTower;
        private readonly ITransitable _deck;
        private readonly ICardTakable _discardPile;

        public CardTransitManager(HandPlayer playerHand, HandAI enemyHand, Tower playerTower, Tower enemyTower, Deck deck,
            DiscardPile discardPile)
        {
            _playerHand = playerHand;
            _playerTower = playerTower;

            _enemyHand = enemyHand;
            _enemyTower = enemyTower;

            _deck = deck;
            _discardPile = discardPile;
        }

        public bool TryTransitCard(Card card, TransitFromType from, TransitToType to)
        {
            ICardTakable takable = from switch
            {
                TransitFromType.DiscardPile => _discardPile,
                TransitFromType.HandEnemy => _enemyHand,
                TransitFromType.HandPlayer => _playerHand,
                _ => throw new Exception($"Ошибка нахождения типа {typeof(TransitFromType)}: {from}")
            };

            ICardSeatable seatable = to switch
            {
                TransitToType.Deck => _deck,
                TransitToType.HandPlayer => _playerHand,
                TransitToType.HandEnemy => _enemyHand,
                _ => throw new Exception($"Ошибка нахождения типа {typeof(TransitToType)}: {to}")
            };

            if (takable.TryTakeAwayCard(card) == false)
                return false;

            seatable.SeatCard(card);
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
    }
}