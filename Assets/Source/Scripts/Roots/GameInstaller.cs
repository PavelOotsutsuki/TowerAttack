using GameFields;
using GameFields.DiscardPiles;
using GameFields.Seats;
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
        
        public override void InstallBindings()
        {
            DeclareSignals();

            Container.Bind<Deck>().FromInstance(_deck).AsSingle();
            Container.Bind<DiscardPileConfig>().FromInstance(_discardPileConfig).AsSingle();
            Container.Bind<SeatPool>().FromInstance(_seatPool).AsSingle();
            Container.Bind<DiscardPile>().AsSingle().NonLazy();
        }

        private void DeclareSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<DiscardCardsSignal>();
        }
    }
}