using Zenject;

namespace Roots
{
    public class GameInstaller : MonoInstaller
    {
        //[Header("Sounds:")]

        //[SerializeField] private ForegroundSoundConfig _foregroundSoundConfig;
        //[SerializeField] private BackgroundSoundConfig _backgroundSoundConfig;

        public override void InstallBindings()
        {
            //DeclareSignals();

            //Container.Bind<BackgroundSoundConfig>().FromScriptableObject(_backgroundSoundConfig).AsSingle();
            //Container.Bind<ForegroundSoundConfig>().FromScriptableObject(_foregroundSoundConfig).AsSingle();
        }

        //private void DeclareSignals()
        //{
        //    SignalBusInstaller.Install(Container);

        //    Container.DeclareSignal<DiscardCardsSignal>();
        //    Container.DeclareSignal<PushStepSignalPlayer>();
        //    Container.DeclareSignal<PushStepSignalEnemyAI>();
        //    Container.DeclareSignal<PersonWinSignal>();
        //}
    }
}