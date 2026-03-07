using Cards.Sounds;
using Cards.Views.BigCardViews;
using Cards.Views.BigCardViews.Capabilities;
using GameFields;
using GameFields.Decks;
using GameFields.DiscardPiles;
using GameFields.Effects;
using GameFields.FightMenues;
using GameFields.Histories;
using GameFields.InformationLabels;
using GameFields.Persons;
using GameFields.Persons.ConfirmableNumbersView;
using GameFields.Persons.Discovers;
using GameFields.Persons.Hands;
using GameFields.Persons.LookCardMenues;
using GameFields.Persons.SelectMenues.Attacks;
using GameFields.Persons.SelectMenues.Choices;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using GameFields.Signals;
using Sounds;
using Tools.UI.UIHelpers;
using Tools.Utils.Screens;
using UnityEngine;
using Zenject;

namespace Roots
{
    public class TestGameInstaller : MonoInstaller
    {
        //[SerializeField] private Script0 _script0Instance;


        //[Header("Sounds:")]

        //[SerializeField] private ForegroundSoundConfig _foregroundSoundConfig;
        //[SerializeField] private BackgroundSoundConfig _backgroundSoundConfig;

        //[SerializeField] private GameObject _fightRootPrefab;

        public override void InstallBindings()
        {
            //DeclareSignals();

            //Container.Bind<BackgroundSoundConfig>().FromScriptableObject(_backgroundSoundConfig).AsSingle();
            //Container.Bind<ForegroundSoundConfig>().FromScriptableObject(_foregroundSoundConfig).AsSingle();
            //Debug.Log("(0.1.1) CreatorInstaller: Installing bindings...");

            ////if (_script0Instance != null)
            ////{
            ////    Container.Bind<Script0>().FromInstance(_script0Instance).AsSingle().NonLazy();
            ////}
            ////else
            ////{
            ////    Container.Bind<Script0>().FromNewComponentOnNewGameObject().WithGameObjectName("Script0_Instance").AsSingle().NonLazy();
            ////}

            //Container.Bind<TestCreator>().FromComponentInHierarchy().AsSingle().NonLazy();

            //Debug.Log("(0.1.2) CreatorInstaller: Bindings installed successfully");

            //Container.Bind<UIHelperDescription>().AsTransient();
            //Container.Bind<BigCardRoot>().AsTransient();
            //Container.Bind<CardSoundRoot>().AsTransient();
            //Container.Bind<ScreenRoot>().AsTransient();
            //Container.Bind<HistoryRoot>().AsTransient();
            //Container.Bind<CardCapabilityDescription>().AsTransient();
            //Container.Bind<InformationLabel>().AsTransient();
            //Container.Bind<SoundRoot>().AsTransient();
            //Container.Bind<FightMenuActivateButton>().AsTransient();
            //Container.Bind<FightMenu>().AsTransient();
            //Container.Bind<HistoryMenuActivateButton>().AsTransient();
            //Container.Bind<HistoryMenu>().AsTransient();
            //Container.Bind<FightButtonsActivator>().AsTransient();
            //Container.Bind<Deck>().AsTransient();
            //Container.Bind<DiscardPileConfig>().AsTransient();
            //Container.Bind<SeatPool>().AsTransient();
            //Container.Bind<DiscardPile>().AsTransient();
            //Container.Bind<SkipTurnLabelPlayer>().AsTransient();
            //Container.Bind<SkipTurnLabelEnemyAI>().AsTransient();
            //Container.Bind<VariantCardCreator>().AsTransient();
            //Container.Bind<HandPlayer>().AsTransient();
            //Container.Bind<TablePlayer>().AsTransient();
            //Container.Bind<TowerPlayer>().AsTransient();
            //Container.Bind<DiscoverPlayer>().AsTransient();
            //Container.Bind<CardPlayingZonePlayer>().AsTransient();
            //Container.Bind<ChoiceMenuPlayer>().AsTransient();
            //Container.Bind<ChoiceMenuImitationPlayer>().AsTransient();
            //Container.Bind<AttackMenuPlayer>().AsTransient();
            //Container.Bind<CardAttackZonePlayer>().AsTransient();
            //Container.Bind<VariantCardCreator>().AsTransient();
            //Container.Bind<LookCardMenuPlayer>().AsTransient();
            //Container.Bind<StartPlayerTurnLabel>().AsTransient();
            //Container.Bind<StartEndGamePanel>().AsTransient();
            //Container.Bind<ForgingZone>().AsTransient();
            //Container.Bind<HandTransferZone>().AsTransient();
            //Container.Bind<HandAI>().AsTransient();
            //Container.Bind<TableAI>().AsTransient();
            //Container.Bind<TowerAI>().AsTransient();
            //Container.Bind<DiscoverAI>().AsTransient();
            //Container.Bind<CardPlayingZoneAI>().AsTransient();
            //Container.Bind<ChoiceMenuEnemyAI>().AsTransient();
            //Container.Bind<ChoiceMenuImitationEnemyAI>().AsTransient();
            //Container.Bind<AttackMenuEnemyAI>().AsTransient();
            //Container.Bind<CardAttackZoneEnemyAI>().AsTransient();
            //Container.Bind<ConfirmableNumbersViewRoot>().AsTransient();

            //Container.BindFactory<GameRoot, FightRootFactory>().FromComponentInNewPrefab(_fightRootPrefab);
        }

        //private void DeclareSignals()
        //{
        //    SignalBusInstaller.Install(Container);

        //    //FightRoot
        //    Container.DeclareSignal<DiscardCardsSignal>();
        //    Container.DeclareSignal<PushStepSignalPlayer>();
        //    Container.DeclareSignal<PushStepSignalEnemyAI>();
        //    Container.DeclareSignal<PersonWinSignal>();
        //}
    }
}