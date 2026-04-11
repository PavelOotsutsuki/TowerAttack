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
using GameFields.Persons.SelectMenues.Choices;
using GameFields.Persons.LookCardMenues;
using GameFields.Effects;
using GameFields.Persons;
using GameFields.FightMenues;
using Tools.Utils.Screens;
using Cards.Views.BigCardViews;
using Cards.Views.BigCardViews.Capabilities;
using Cards.Sounds;
using Tools.UI.UIHelpers;
using GameFields.Histories;
using GameFields.Persons.ConfirmableNumbersView;
using Sounds;
using System.Collections.Generic;
using Tools;
using System.Linq;
using GameFields.Backgrounds;

namespace Roots
{
    public class GameFieldInstaller :  MonoInstaller
    {
        //[Header("Sounds:")]

        //[SerializeField] private BackgroundSoundConfig _backgroundSoundConfig;
        //[SerializeField] private ForegroundSoundConfig _foregroundSoundConfig;

        //[Space]
        //[Header("----------------------------")]
        //[Space]
        [Header("GameFields:")]

        [SerializeField] private UIHelperDescription _UIHelperDescription;

        [SerializeField] private BigCardRoot _bigCardRoot;

        [SerializeField] private InformationLabel _informationLabel;
        //[SerializeField] private InputRoot _inputRoot;
        [SerializeField] private SoundRoot _soundRoot;

        [SerializeField] private FightMenuActivateButton _fightMenuActivateButton;
        [SerializeField] private FightMenu _fightMenu;

        [SerializeField] private HistoryMenuActivateButton _historyMenuActivateButton;
        [SerializeField] private HistoryMenu _historyMenu;

        [SerializeField] private Deck _deck;
        [SerializeField] private DiscardPileConfig _discardPileConfig;
        [SerializeField] private SeatPool _seatPool;
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
        //[SerializeField] private SwitchRootPanel _startEndGamePanel;

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

        [SerializeField] private ConfirmableNumbersViewRoot _confirmableNumbersViewRoot;

        [SerializeField] private BackgroundRoot _backgroundRoot;

        private CardSoundRoot _cardSoundRoot;
        //private ScreenRoot _screenRoot;
        //private CardCapabilityDescription _cardCapabilityDescription;
        private FightButtonsActivator _fightButtonsActivator;
        private HistoryRoot _historyRoot;
        private DiscardPile _discardPile;

        //private DiContainer _sceneContainer;

        //private List<IUnbindable> _binds;

        //public void Awake()
        //{
        //    SceneContext sceneContext = FindObjectOfType<SceneContext>();
        //    _sceneContainer = sceneContext.Container;

        //    _sceneContainer.InjectGameObject(gameObject);
        //    //InstallBindings();
        //}

        //[Inject]
        //private void Construct(BackgroundSoundConfig backgroundSoundConfig, ForegroundSoundConfig foregroundSoundConfig,
        //    SignalBus bus) 
        //{
        //    _binds = new List<IUnbindable>();

        //    _binds.Add(new BindManager<UIHelperDescription>(_sceneContainer, _UIHelperDescription));
        //    _UIHelperDescription.Init();

        //    _binds.Add(new BindManager<BigCardRoot>(_sceneContainer, _bigCardRoot));

        //    _cardSoundRoot = new CardSoundRoot(foregroundSoundConfig);
        //    _binds.Add(new BindManager<CardSoundRoot>(_sceneContainer, _cardSoundRoot));

        //    _screenRoot = new ScreenRoot();
        //    _binds.Add(new BindManager<ScreenRoot>(_sceneContainer, _screenRoot));

        //    _historyRoot = new HistoryRoot(_historyMenu);
        //    _binds.Add(new BindManager<HistoryRoot>(_sceneContainer, _historyRoot));

        //    _cardCapabilityDescription = new CardCapabilityDescription();
        //    _binds.Add(new BindManager<CardCapabilityDescription>(_sceneContainer, _cardCapabilityDescription));

        //    _binds.Add(new BindManager<InformationLabel>(_sceneContainer, _informationLabel));
        //    _binds.Add(new BindManager<SoundRoot>(_sceneContainer, _soundRoot));

        //    _binds.Add(new BindManager<FightMenuActivateButton>(_sceneContainer, _fightMenuActivateButton));
        //    _binds.Add(new BindManager<FightMenu>(_sceneContainer, _fightMenu));

        //    _binds.Add(new BindManager<HistoryMenuActivateButton>(_sceneContainer, _historyMenuActivateButton));
        //    _binds.Add(new BindManager<HistoryMenu>(_sceneContainer, _historyMenu));

        //    _fightButtonsActivator = new FightButtonsActivator(_fightMenuActivateButton, _historyMenuActivateButton);
        //    _binds.Add(new BindManager<FightButtonsActivator>(_sceneContainer, _fightButtonsActivator));

        //    _binds.Add(new BindManager<Deck>(_sceneContainer, _deck));
        //    _binds.Add(new BindManager<DiscardPileConfig>(_sceneContainer, _discardPileConfig));
        //    _binds.Add(new BindManager<SeatPool>(_sceneContainer, _seatPool));

        //    _discardPile = new DiscardPile(_seatPool, bus, _discardPileConfig);
        //    _binds.Add(new BindManager<DiscardPile>(_sceneContainer, _discardPile));

        //    _binds.Add(new BindManager<SkipTurnLabelPlayer>(_sceneContainer, _skipTurnLabelPlayer));
        //    _binds.Add(new BindManager<SkipTurnLabelEnemyAI>(_sceneContainer, _skipTurnLabelEnemyAI));

        //    _binds.Add(new BindManager<VariantCardCreator>(_sceneContainer, _variantCardCreator));

        //    _binds.Add(new BindManager<HandPlayer>(_sceneContainer, _playerHand));
        //    _binds.Add(new BindManager<TablePlayer>(_sceneContainer, _playerTable));
        //    _binds.Add(new BindManager<TowerPlayer>(_sceneContainer, _playerTower));
        //    _binds.Add(new BindManager<DiscoverPlayer>(_sceneContainer, _playerDiscover));
        //    _binds.Add(new BindManager<CardPlayingZonePlayer>(_sceneContainer, _playerPlayingZone));
        //    _binds.Add(new BindManager<ChoiceMenuPlayer>(_sceneContainer, _choiceMenuPlayer));
        //    _binds.Add(new BindManager<ChoiceMenuImitationPlayer>(_sceneContainer, _choiceMenuImitationPlayer));
        //    _binds.Add(new BindManager<AttackMenuPlayer>(_sceneContainer, _attackMenuPlayer));
        //    _binds.Add(new BindManager<CardAttackZonePlayer>(_sceneContainer, _playerCardAttackZone));

        //    _binds.Add(new BindManager<LookCardMenuPlayer>(_sceneContainer, _lookCardMenuPlayer));
        //    _binds.Add(new BindManager<StartPlayerTurnLabel>(_sceneContainer, _startPlayerTurnLabel));
        //    _binds.Add(new BindManager<StartEndGamePanel>(_sceneContainer, _startEndGamePanel));

        //    _binds.Add(new BindManager<ForgingZone>(_sceneContainer, _forgingZone));
        //    _binds.Add(new BindManager<HandTransferZone>(_sceneContainer, _handTransferZone));

        //    _binds.Add(new BindManager<HandAI>(_sceneContainer, _enemyHand));
        //    _binds.Add(new BindManager<TableAI>(_sceneContainer, _enemyTable));
        //    _binds.Add(new BindManager<TowerAI>(_sceneContainer, _enemyTower));
        //    _binds.Add(new BindManager<DiscoverAI>(_sceneContainer, _enemyDiscoverImitation));
        //    _binds.Add(new BindManager<CardPlayingZoneAI>(_sceneContainer, _enemyPlayingZone));
        //    _binds.Add(new BindManager<ChoiceMenuEnemyAI>(_sceneContainer, _enemyChoiceMenu));
        //    _binds.Add(new BindManager<ChoiceMenuImitationEnemyAI>(_sceneContainer, _enemyChoiceMenuImitation));
        //    _binds.Add(new BindManager<AttackMenuEnemyAI>(_sceneContainer, _enemyAttackMenu));
        //    _binds.Add(new BindManager<CardAttackZoneEnemyAI>(_sceneContainer, _enemyCardAttackZone));

        //    _binds.Add(new BindManager<ConfirmableNumbersViewRoot>(_sceneContainer, _confirmableNumbersViewRoot));

        //    GameRoot gameRoot = FindObjectOfType<GameRoot>();

        //    _sceneContainer.InjectGameObject(gameRoot.gameObject);

        //    Debug.Log("GameInstaller УСПЕШНО ВСЕ ЗАБИНДИЛ");
        //}

        //[Inject]
        //private void Construct(BackgroundSoundConfig backgroundSoundConfig, ForegroundSoundConfig foregroundSoundConfig,
        //    SignalBus bus)
        //{
        //    _sceneContainer.Bind<UIHelperDescription>().FromInstance(_UIHelperDescription).AsSingle();
        //    _UIHelperDescription.Init();

        //    _sceneContainer.Bind<BigCardRoot>().FromInstance(_bigCardRoot).AsSingle();

        //    _cardSoundRoot = new CardSoundRoot(foregroundSoundConfig);
        //    _sceneContainer.Bind<CardSoundRoot>().FromInstance(_cardSoundRoot).AsSingle();

        //    _screenRoot = new ScreenRoot();
        //    _sceneContainer.Bind<ScreenRoot>().FromInstance(_screenRoot).AsSingle();

        //    _historyRoot = new HistoryRoot(_historyMenu);
        //    _sceneContainer.Bind<HistoryRoot>().FromInstance(_historyRoot).AsSingle();

        //    _cardCapabilityDescription = new CardCapabilityDescription();
        //    _sceneContainer.Bind<CardCapabilityDescription>().FromInstance(_cardCapabilityDescription).AsSingle();

        //    _sceneContainer.Bind<InformationLabel>().FromInstance(_informationLabel).AsSingle();
        //    //Container.Bind<InputRoot>().FromInstance(_inputRoot).AsSingle();
        //    _sceneContainer.Bind<SoundRoot>().FromInstance(_soundRoot).AsSingle();

        //    _sceneContainer.Bind<FightMenuActivateButton>().FromInstance(_fightMenuActivateButton).AsSingle();
        //    _sceneContainer.Bind<FightMenu>().FromInstance(_fightMenu).AsSingle();

        //    _sceneContainer.Bind<HistoryMenuActivateButton>().FromInstance(_historyMenuActivateButton).AsSingle();
        //    _sceneContainer.Bind<HistoryMenu>().FromInstance(_historyMenu).AsSingle();

        //    _fightButtonsActivator = new FightButtonsActivator(_fightMenuActivateButton, _historyMenuActivateButton);
        //    _sceneContainer.Bind<FightButtonsActivator>().FromInstance(_fightButtonsActivator).AsSingle();

        //    _sceneContainer.Bind<Deck>().FromInstance(_deck).AsSingle();
        //    _sceneContainer.Bind<DiscardPileConfig>().FromInstance(_discardPileConfig).AsSingle();
        //    _sceneContainer.Bind<SeatPool>().FromInstance(_seatPool).AsSingle();
        //    _sceneContainer.Bind<DiscardPile>().AsSingle().NonLazy();
        //    _sceneContainer.Bind<SkipTurnLabelPlayer>().FromInstance(_skipTurnLabelPlayer).AsSingle();
        //    _sceneContainer.Bind<SkipTurnLabelEnemyAI>().FromInstance(_skipTurnLabelEnemyAI).AsSingle();

        //    _sceneContainer.Bind<VariantCardCreator>().FromInstance(_variantCardCreator).AsSingle();

        //    _sceneContainer.Bind<HandPlayer>().FromInstance(_playerHand).AsSingle();
        //    _sceneContainer.Bind<TablePlayer>().FromInstance(_playerTable).AsSingle();
        //    _sceneContainer.Bind<TowerPlayer>().FromInstance(_playerTower).AsSingle();
        //    _sceneContainer.Bind<DiscoverPlayer>().FromInstance(_playerDiscover).AsSingle();
        //    _sceneContainer.Bind<CardPlayingZonePlayer>().FromInstance(_playerPlayingZone).AsSingle();
        //    _sceneContainer.Bind<ChoiceMenuPlayer>().FromInstance(_choiceMenuPlayer).AsSingle();
        //    _sceneContainer.Bind<ChoiceMenuImitationPlayer>().FromInstance(_choiceMenuImitationPlayer).AsSingle();
        //    _sceneContainer.Bind<AttackMenuPlayer>().FromInstance(_attackMenuPlayer).AsSingle();
        //    _sceneContainer.Bind<CardAttackZonePlayer>().FromInstance(_playerCardAttackZone).AsSingle();

        //    _sceneContainer.Bind<LookCardMenuPlayer>().FromInstance(_lookCardMenuPlayer).AsSingle();
        //    _sceneContainer.Bind<StartPlayerTurnLabel>().FromInstance(_startPlayerTurnLabel).AsSingle();
        //    _sceneContainer.Bind<StartEndGamePanel>().FromInstance(_startEndGamePanel).AsSingle();

        //    _sceneContainer.Bind<ForgingZone>().FromInstance(_forgingZone).AsSingle();
        //    _sceneContainer.Bind<HandTransferZone>().FromInstance(_handTransferZone).AsSingle();

        //    _sceneContainer.Bind<HandAI>().FromInstance(_enemyHand).AsSingle();
        //    _sceneContainer.Bind<TableAI>().FromInstance(_enemyTable).AsSingle();
        //    _sceneContainer.Bind<TowerAI>().FromInstance(_enemyTower).AsSingle();
        //    _sceneContainer.Bind<DiscoverAI>().FromInstance(_enemyDiscoverImitation).AsSingle();
        //    _sceneContainer.Bind<CardPlayingZoneAI>().FromInstance(_enemyPlayingZone).AsSingle();
        //    _sceneContainer.Bind<ChoiceMenuEnemyAI>().FromInstance(_enemyChoiceMenu).AsSingle();
        //    _sceneContainer.Bind<ChoiceMenuImitationEnemyAI>().FromInstance(_enemyChoiceMenuImitation).AsSingle();
        //    _sceneContainer.Bind<AttackMenuEnemyAI>().FromInstance(_enemyAttackMenu).AsSingle();
        //    _sceneContainer.Bind<CardAttackZoneEnemyAI>().FromInstance(_enemyCardAttackZone).AsSingle();

        //    _sceneContainer.Bind<ConfirmableNumbersViewRoot>().FromInstance(_confirmableNumbersViewRoot).AsSingle();

        //    Debug.Log("GameInstaller УСПЕШНО ВСЕ ЗАБИНДИЛ");
        //}
        [Inject] private ForegroundSoundConfig _foregroundSoundConfig;

        public override void InstallBindings()
        {
            //DeclareSignals();

            //Container.Bind<BackgroundSoundConfig>().FromScriptableObject(_backgroundSoundConfig).AsSingle();
            //Container.Bind<ForegroundSoundConfig>().FromScriptableObject(_foregroundSoundConfig).AsSingle();

            Container.Bind<UIHelperDescription>().FromInstance(_UIHelperDescription).AsSingle();
            _UIHelperDescription.Init();

            Container.Bind<BigCardRoot>().FromInstance(_bigCardRoot).AsSingle();

            _cardSoundRoot = new CardSoundRoot(_foregroundSoundConfig);
            Container.Bind<CardSoundRoot>().FromInstance(_cardSoundRoot).AsSingle();

            //_screenRoot = new ScreenRoot();
            //Container.Bind<ScreenRoot>().FromInstance(_screenRoot).AsSingle();

            _historyRoot = new HistoryRoot(_historyMenu);
            Container.Bind<HistoryRoot>().FromInstance(_historyRoot).AsSingle();

            Container.Bind<InformationLabel>().FromInstance(_informationLabel).AsSingle();
            //Container.Bind<InputRoot>().FromInstance(_inputRoot).AsSingle();
            Container.Bind<SoundRoot>().FromInstance(_soundRoot).AsSingle();

            Container.Bind<FightMenuActivateButton>().FromInstance(_fightMenuActivateButton).AsSingle();
            Container.Bind<FightMenu>().FromInstance(_fightMenu).AsSingle();

            Container.Bind<HistoryMenuActivateButton>().FromInstance(_historyMenuActivateButton).AsSingle();
            Container.Bind<HistoryMenu>().FromInstance(_historyMenu).AsSingle();

            _fightButtonsActivator = new FightButtonsActivator(_fightMenuActivateButton, _historyMenuActivateButton);
            Container.Bind<FightButtonsActivator>().FromInstance(_fightButtonsActivator).AsSingle();

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
            //Container.Bind<SwitchRootPanel>().FromInstance(_startEndGamePanel).AsSingle();

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

            Container.Bind<ConfirmableNumbersViewRoot>().FromInstance(_confirmableNumbersViewRoot).AsSingle();

            Container.Bind<BackgroundRoot>().FromInstance(_backgroundRoot).AsSingle();
        }

        //private void DeclareSignals()
        //{
        //    SignalBusInstaller.Install(Container);

        //    Container.DeclareSignal<DiscardCardsSignal>();
        //    Container.DeclareSignal<PushStepSignalPlayer>();
        //    Container.DeclareSignal<PushStepSignalEnemyAI>();
        //    Container.DeclareSignal<PersonWinSignal>();
        //}

        //private void DeclareSignals()
        //{
        //    _sceneContainer.DeclareSignal<DiscardCardsSignal>();
        //    _sceneContainer.DeclareSignal<PushStepSignalPlayer>();
        //    _sceneContainer.DeclareSignal<PushStepSignalEnemyAI>();
        //    _sceneContainer.DeclareSignal<PersonWinSignal>();
        //}

        //private void OnDestroy()
        //{
        //    if (_binds.Count <= 0)
        //        return;

        //    foreach (IUnbindable unbindable in _binds)
        //    {
        //        unbindable.UnBind();
        //    }

        //    _binds.Clear();

        //    GameFieldGC.Collect();
        //}
    }
}