using GameFields.Discarding;
using GameFields.Seats;
using UnityEngine;
using Zenject;

namespace GameFields
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private SeatPool _seatPool;
        
        public override void InstallBindings()
        {
            DeclareSignals();

            Container.Bind<Deck>().FromInstance(_deck).AsSingle();
            Container.Bind<DiscardPile>().FromInstance(_discardPile).AsSingle();
            Container.Bind<SeatPool>().FromInstance(_seatPool).AsSingle();
        }

        private void DeclareSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<EffectCreatedSignal>();
            Container.DeclareSignal<DiscardCardsSignal>();
        }
    }
}