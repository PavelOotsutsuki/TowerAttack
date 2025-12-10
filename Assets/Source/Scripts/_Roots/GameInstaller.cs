using Cards;
using GameFields;
using GameFields.Decks;
using GameFields.DiscardPiles;
using GameFields.InformationLabels;
using GameFields.Persons.SelectMenues.Attacks;
using GameFields.Persons.Discovers;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using GameFields.Signals;
using UnityEngine;
using Zenject;
using GameFields.Persons.SelectMenues;
using GameFields.Persons.SelectMenues.Choices;
using GameFields.Persons.LookCardMenues;
using GameFields.Effects;
using GameFields.Persons;
using GameFields.InputSettings;
using GameFields.FightMenues;
using Tools.Utils.Screens;
using Cards.Views.BigCardViews;
using Cards.Views.BigCardViews.Capabilities;
using Cards.Sounds;
using Tools.UI;

namespace Roots
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private UIHelperDescription _UIHelperDescription;

        [SerializeField] private BigCardRoot _bigCardRoot;

        [SerializeField] private InformationLabel _informationLabel;
        //[SerializeField] private InputRoot _inputRoot;
        [SerializeField] private SoundRoot _soundRoot;
        [SerializeField] private FightMenuActivateButton _fightMenuActivateButton;
        [SerializeField] private FightMenu _fightMenu;

        [SerializeField] private Deck _deck;
        [SerializeField] private DiscardPileConfig _discardPileConfig;
        [SerializeField] private SeatPool _seatPool;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private SkipTurnLabelPlayer _skipTurnLabelPlayer;
        [SerializeField] private SkipTurnLabelEnemyAI _skipTurnLabelEnemyAI;

        [SerializeField] private VariantCardCreator _variantCardCreator;

        [SerializeField] private HandPlayer _playerHand;
        [SerializeField] private TablePlayer _playerTable;
        [SerializeField] private TowerPlayer _playerTower;
        [SerializeField] private DiscoverPlayer _playerDiscover;
        [SerializeField] private CardPlayingZonePlayer _playerPlayingZone;
        [SerializeField] private ChoiceMenuPlayer _choiceMenuPlayer;
        [SerializeField] private ChoiceMenuImitationPlayer _choiceMenuImitationPlayer;
        [SerializeField] private AttackMenuPlayer _attackMenuPlayer;
        [SerializeField] private CardAttackZonePlayer _playerCardAttackZone;

        [SerializeField] private LookCardMenuPlayer _lookCardMenuPlayer;
        [SerializeField] private StartPlayerTurnLabel _startPlayerTurnLabel;
        [SerializeField] private StartEndGamePanel _startEndGamePanel;

        [SerializeField] private ForgingZone _forgingZone;
        [SerializeField] private HandTransferZone _handTransferZone;

        [SerializeField] private HandAI _enemyHand;
        [SerializeField] private TableAI _enemyTable;
        [SerializeField] private TowerAI _enemyTower;
        [SerializeField] private DiscoverAI _enemyDiscoverImitation;
        [SerializeField] private CardPlayingZoneAI _enemyPlayingZone;
        [SerializeField] private ChoiceMenuEnemyAI _enemyChoiceMenu;
        [SerializeField] private ChoiceMenuImitationEnemyAI _enemyChoiceMenuImitation;
        [SerializeField] private AttackMenuEnemyAI _enemyAttackMenu;
        [SerializeField] private CardAttackZoneEnemyAI _enemyCardAttackZone;

        private CardSoundRoot _cardSoundRoot;
        private ScreenRoot _screenRoot;
        private CardCapabilityDescription _cardCapabilityDescription;

        public override void InstallBindings()
        {
            DeclareSignals();

            Container.Bind<UIHelperDescription>().FromInstance(_UIHelperDescription).AsSingle();
            _UIHelperDescription.Init();

            Container.Bind<BigCardRoot>().FromInstance(_bigCardRoot).AsSingle();

            _cardSoundRoot = new CardSoundRoot();
            Container.Bind<CardSoundRoot>().FromInstance(_cardSoundRoot).AsSingle();

            _screenRoot = new ScreenRoot();
            Container.Bind<ScreenRoot>().FromInstance(_screenRoot).AsSingle();

            _cardCapabilityDescription = new CardCapabilityDescription();
            Container.Bind<CardCapabilityDescription>().FromInstance(_cardCapabilityDescription).AsSingle();

            Container.Bind<InformationLabel>().FromInstance(_informationLabel).AsSingle();
            //Container.Bind<InputRoot>().FromInstance(_inputRoot).AsSingle();
            Container.Bind<SoundRoot>().FromInstance(_soundRoot).AsSingle();
            Container.Bind<FightMenuActivateButton>().FromInstance(_fightMenuActivateButton).AsSingle();
            Container.Bind<FightMenu>().FromInstance(_fightMenu).AsSingle();

            Container.Bind<Deck>().FromInstance(_deck).AsSingle();
            Container.Bind<DiscardPileConfig>().FromInstance(_discardPileConfig).AsSingle();
            Container.Bind<SeatPool>().FromInstance(_seatPool).AsSingle();
            Container.Bind<DiscardPile>().AsSingle().NonLazy();
            Container.Bind<SkipTurnLabelPlayer>().FromInstance(_skipTurnLabelPlayer).AsSingle();
            Container.Bind<SkipTurnLabelEnemyAI>().FromInstance(_skipTurnLabelEnemyAI).AsSingle();

            Container.Bind<VariantCardCreator>().FromInstance(_variantCardCreator).AsSingle();

            Container.Bind<HandPlayer>().FromInstance(_playerHand).AsSingle();
            Container.Bind<TablePlayer>().FromInstance(_playerTable).AsSingle();
            Container.Bind<TowerPlayer>().FromInstance(_playerTower).AsSingle();
            Container.Bind<DiscoverPlayer>().FromInstance(_playerDiscover).AsSingle();
            Container.Bind<CardPlayingZonePlayer>().FromInstance(_playerPlayingZone).AsSingle();
            Container.Bind<ChoiceMenuPlayer>().FromInstance(_choiceMenuPlayer).AsSingle();
            Container.Bind<ChoiceMenuImitationPlayer>().FromInstance(_choiceMenuImitationPlayer).AsSingle();
            Container.Bind<AttackMenuPlayer>().FromInstance(_attackMenuPlayer).AsSingle();
            Container.Bind<CardAttackZonePlayer>().FromInstance(_playerCardAttackZone).AsSingle();

            Container.Bind<LookCardMenuPlayer>().FromInstance(_lookCardMenuPlayer).AsSingle();
            Container.Bind<StartPlayerTurnLabel>().FromInstance(_startPlayerTurnLabel).AsSingle();
            Container.Bind<StartEndGamePanel>().FromInstance(_startEndGamePanel).AsSingle();

            Container.Bind<ForgingZone>().FromInstance(_forgingZone).AsSingle();
            Container.Bind<HandTransferZone>().FromInstance(_handTransferZone).AsSingle();

            Container.Bind<HandAI>().FromInstance(_enemyHand).AsSingle();
            Container.Bind<TableAI>().FromInstance(_enemyTable).AsSingle();
            Container.Bind<TowerAI>().FromInstance(_enemyTower).AsSingle();
            Container.Bind<DiscoverAI>().FromInstance(_enemyDiscoverImitation).AsSingle();
            Container.Bind<CardPlayingZoneAI>().FromInstance(_enemyPlayingZone).AsSingle();
            Container.Bind<ChoiceMenuEnemyAI>().FromInstance(_enemyChoiceMenu).AsSingle();
            Container.Bind<ChoiceMenuImitationEnemyAI>().FromInstance(_enemyChoiceMenuImitation).AsSingle();
            Container.Bind<AttackMenuEnemyAI>().FromInstance(_enemyAttackMenu).AsSingle();
            Container.Bind<CardAttackZoneEnemyAI>().FromInstance(_enemyCardAttackZone).AsSingle();
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