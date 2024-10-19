using GameFields.DiscardPiles;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using Zenject;

namespace GameFields
{
    public class CardTransitManager
    {
        private CardPlayingZone _playerPlayingZone;
        private HandPlayer _playerHand;
        private Tower _playerTower;

        private CardPlayingZone _enemyPlayingZone;
        private HandAI _enemyHand;
        private Tower _enemyTower;

        private Deck _deck;
        private DiscardPile _discardPile;

        [Inject]
        public void Construct(CardPlayingZonePlayer playerPlayingZone, HandPlayer playerHand, TowerPlayer playerTower,
            CardPlayingZoneAI enemyPlayingZone, HandAI enemyHand, TowerAI enemyTower, Deck deck, DiscardPile discardPile)
        {
            _playerPlayingZone = playerPlayingZone;
            _playerHand = playerHand;
            _playerTower = playerTower;
            _enemyPlayingZone = enemyPlayingZone;
            _enemyHand = enemyHand;
            _enemyTower = enemyTower;
            _deck = deck;
            _discardPile = discardPile;
        }

    }
}
