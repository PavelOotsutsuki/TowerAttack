using Cards.Views.BigCardViews.Capabilities;
using GameFields.Seats;
using GameFields.Signals;
using Sounds;
using Tools.Utils.Screens;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Sounds:")]

        [SerializeField] private ForegroundSoundConfig _foregroundSoundConfig;
        [SerializeField] private BackgroundSoundConfig _backgroundSoundConfig;

        private ScreenRoot _screenRoot;
        private CardCapabilityDescription _cardCapabilityDescription;

        public override void InstallBindings()
        {
            DeclareSignals();

            Container.Bind<BackgroundSoundConfig>().FromScriptableObject(_backgroundSoundConfig).AsSingle();
            Container.Bind<ForegroundSoundConfig>().FromScriptableObject(_foregroundSoundConfig).AsSingle();

            _screenRoot = new ScreenRoot();
            Container.Bind<ScreenRoot>().FromInstance(_screenRoot).AsSingle();

            _cardCapabilityDescription = new CardCapabilityDescription();
            Container.Bind<CardCapabilityDescription>().FromInstance(_cardCapabilityDescription).AsSingle();
        }

        private void DeclareSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<DiscardCardsSignal>();
            Container.DeclareSignal<PushStepSignalPlayer>(); 
            Container.DeclareSignal<PushStepSignalEnemyAI>();
            Container.DeclareSignal<PersonWinSignal>();
        }
    }
}