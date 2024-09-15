using GameFields;
using GameFields.DiscardPiles;
using GameFields.Persons;
using GameFields.Persons.Discovers;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using GameFields.Signals;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private SeatPool _seatPool;
        [SerializeField] private DiscardPileConfig _discardPileConfig;

        [SerializeField] private CardPlayingZonePlayer _playerPlayingZone;
        [SerializeField] private HandPlayer _playerHand;
        [SerializeField] private TablePlayer _playerTable;
        [SerializeField] private TowerPlayer _playerTower;
        [SerializeField] private DiscoverPlayer _playerDiscover;

        [SerializeField] private CardPlayingZoneAI _enemyPlayingZone;
        [SerializeField] private HandAI _enemyHand;
        [SerializeField] private TableAI _enemyTable;
        [SerializeField] private TowerAI _enemyTower;
        [SerializeField] private DiscoverAI _enemyDiscoverImitation;

        public override void InstallBindings()
        {
            DeclareSignals();

            Container.Bind<Deck>().FromInstance(_deck).AsSingle();
            Container.Bind<DiscardPileConfig>().FromInstance(_discardPileConfig).AsSingle();
            Container.Bind<SeatPool>().FromInstance(_seatPool).AsSingle();
            Container.Bind<DiscardPile>().AsSingle().NonLazy();

            Container.Bind<HandPlayer>().FromInstance(_playerHand).AsSingle();
            Container.Bind<TablePlayer>().FromInstance(_playerTable).AsSingle();
            Container.Bind<TowerPlayer>().FromInstance(_playerTower).AsSingle();
            Container.Bind<DiscoverPlayer>().FromInstance(_playerDiscover).AsSingle();
            Container.Bind<CardPlayingZonePlayer>().FromInstance(_playerPlayingZone).AsSingle();

            Container.Bind<HandAI>().FromInstance(_enemyHand).AsSingle();
            Container.Bind<TableAI>().FromInstance(_enemyTable).AsSingle();
            Container.Bind<TowerAI>().FromInstance(_enemyTower).AsSingle();
            Container.Bind<DiscoverAI>().FromInstance(_enemyDiscoverImitation).AsSingle();
            Container.Bind<CardPlayingZoneAI>().FromInstance(_enemyPlayingZone).AsSingle();
        }

        private void DeclareSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<DiscardCardsSignal>();
            Container.DeclareSignal<StartEffectSignal>();
        }
    }
}