using Zenject;
using StartMenues;
using System.Threading;

namespace Roots
{
    public class StartMenuInstaller : MonoInstaller
    {
        //[Header("Sounds:")]

        //[SerializeField] private BackgroundSoundConfig _backgroundSoundConfig;
        //[SerializeField] private ForegroundSoundConfig _foregroundSoundConfig;

        //private ScreenRoot _screenRoot;
        //private CardCapabilityDescription _cardCapabilityDescription;
        [Inject] private GameRootCTSHolder _gameRootCTSHolder;

        private StartMenuCTSHolder _startMenuCTSHolder;

        public override void InstallBindings()
        {
            CancellationTokenSource startMenuCTS = CancellationTokenSource.CreateLinkedTokenSource(_gameRootCTSHolder.Token);

            _startMenuCTSHolder = new StartMenuCTSHolder(startMenuCTS);
            Container.Bind<StartMenuCTSHolder>().FromInstance(_startMenuCTSHolder).AsSingle();

            //_screenRoot = new ScreenRoot();
            //Container.Bind<ScreenRoot>().FromInstance(_screenRoot).AsSingle();
        }
    }
}