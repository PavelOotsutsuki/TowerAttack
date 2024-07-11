using GameFields.Discarding;
using Zenject;

namespace GameFields
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<CardPlayedSignal>();
            Container.DeclareSignal<DiscardCardsSignal>();
            Container.DeclareSignal<EffectCreatedSignal>();
        }
    }
}