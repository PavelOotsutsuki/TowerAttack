using Zenject;
using Tools.Utils.Screens;

namespace Roots
{
    public class StartMenuInstaller : MonoInstaller
    {
        //[Header("Sounds:")]

        //[SerializeField] private BackgroundSoundConfig _backgroundSoundConfig;
        //[SerializeField] private ForegroundSoundConfig _foregroundSoundConfig;

        //private ScreenRoot _screenRoot;
        //private CardCapabilityDescription _cardCapabilityDescription;

        public override void InstallBindings()
        {
            //_screenRoot = new ScreenRoot();
            //Container.Bind<ScreenRoot>().FromInstance(_screenRoot).AsSingle();
        }
    }
}