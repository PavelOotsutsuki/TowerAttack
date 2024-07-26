using Cards;
using GameFields.Discarding;
using Tools;
using UnityEngine;
using GameFields.Persons;
using GameFields.EndTurnButtons;
using GameFields.StartTowerCardSelections;
using GameFields.Effects;
using GameFields.Seats;
using Zenject;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private StartTowerCardSelection _startTowerCardSelection;
        [SerializeField] private SeatPool _seatPool;

        [SerializeField] private PersonCreator _personCreator;

        private Player _player;
        private EnemyAI _enemyAI;

        private EffectFactory _effectFactory;
        private SignalBus _bus;

        [Inject]
        private void Construct(SignalBus bus, DiContainer container)
        {
            _bus = bus;
        }

        public void Init(Card[] startCards)
        {
            _effectFactory = new EffectFactory(_deck);
            _seatPool.Init();
            _deck.Init(startCards);
            _discardPile.Init(_seatPool);
            _endTurnButton.Init();
            _personCreator.Init(_bus, _deck, _endTurnButton, _seatPool, _effectFactory);

            InitLevel();
        }

        private void InitLevel()
        {
            _player = _personCreator.CreatePlayer();
            _enemyAI = _personCreator.CreateEnemyAI();

            _startTowerCardSelection.Init(_player, _enemyAI);

            FightResult fightResult = new();
            Fight fight = new(_player, _enemyAI, fightResult, _bus);
            EndFight endFight = new(fightResult);

            FightStepsController fightStepsController = new(_startTowerCardSelection, fight, endFight);

            fightStepsController.StartStep();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDeck();
            DefineDiscardPile();
            DefineEndTurnButton();
            DefinePersonCreator();
            DefineSeatPool();
            DefineFirstTurn();
        }

        [ContextMenu(nameof(DefineDeck))]
        private void DefineDeck()
        {
            AutomaticFillComponents.DefineComponent(this, ref _deck, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDiscardPile))]
        private void DefineDiscardPile()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discardPile, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineEndTurnButton))]
        private void DefineEndTurnButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _endTurnButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefinePersonCreator))]
        private void DefinePersonCreator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _personCreator, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSeatPool))]
        private void DefineSeatPool()
        {
            AutomaticFillComponents.DefineComponent(this, ref _seatPool, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFirstTurn))]
        private void DefineFirstTurn()
        {
            AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelection, ComponentLocationTypes.InChildren);
        }
    }
}
