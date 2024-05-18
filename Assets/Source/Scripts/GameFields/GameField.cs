using Cards;
using Tools;
using UnityEngine;
using GameFields.Persons;
using GameFields.EndTurnButtons;
using GameFields.StartTowerCardSelections;
using GameFields.Effects;
using GameFields.DiscardPiles;
using GameFields.Seats;
using GameFields.Persons.Tables;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private StartTowerCardSelection _startTowerCardSelection;
        [SerializeField] private SeatPool _seatPool;
        //[SerializeField] private SpeedUpButton _speedUpButton;

        [SerializeField] private PersonCreator _personCreator;

        private Player _player;
        private EnemyAI _enemyAI;

        public void Init(Card[] cardsInDeck)
        {
            _seatPool.Init();
            _deck.Init(cardsInDeck);
            _discardPile.Init(_seatPool);
            _personCreator.Init(_discardPile, _deck);

            _player = _personCreator.CreatePlayer();
            _enemyAI = _personCreator.CreateEnemyAI();

            _startTowerCardSelection.Init(_player, _enemyAI);

            FightResult fightResult = new FightResult();

            Fight fight = new Fight(_player, _enemyAI, _endTurnButton, fightResult);
            EffectRoot effectRoot = new EffectRoot(_deck, _discardPile, fight);
            EndFight endFight = new EndFight(fightResult);

            _personCreator.InitPersonsData(effectRoot, _seatPool);
            _player.Init();
            _enemyAI.Init();

            _endTurnButton.Init(fight);
            FightStepsController fightStepsController = new FightStepsController(_startTowerCardSelection, fight, endFight);

            //fightStepsController.PrepareToStart();
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
