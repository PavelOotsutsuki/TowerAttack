using Cards;
using Tools;
using UnityEngine;
using GameFields.Persons;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private Player _player;
        [SerializeField] private HandAI _enemyHand;
        [SerializeField] private TableAI _enemyTable;
        [SerializeField] private TowerAI _enemyTower;
        [SerializeField] private int _countDrawCardsEnemy = 1;
        [SerializeField] private float _drawCardsDelayEnemy = 0.5f;
        [SerializeField] private Fight _fight;

        private EnemyAI _enemyAI;
        //private Fight _fight;

        public void Init(Card[] cardsInDeck)
        {
            InitDeck(cardsInDeck);
            InitPersons();

            //_fight = new Fight(_player, _enemyAI, _deck);
            _fight.Init(_player, _enemyAI, _deck);

            InitEndTurnButton();
        }

        private void InitPersons()
        {
            _player.Init();
            _enemyAI = new EnemyAI(_enemyHand, _enemyTable, _enemyTower, _countDrawCardsEnemy, _drawCardsDelayEnemy);
        }

        private void InitDeck(Card[] cards)
        {
            _deck.Init(cards);
        }

        private void InitEndTurnButton()
        {
            _endTurnButton.Init(_fight);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDeck();
            DefineEndTurnButton();
            DefineHandAI();
            DefineTableAI();
            DefineTowerAI();
            DefineFight();
        }

        [ContextMenu(nameof(DefineDeck))]
        private void DefineDeck()
        {
            AutomaticFillComponents.DefineComponent(this, ref _deck, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineEndTurnButton))]
        private void DefineEndTurnButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _endTurnButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineHandAI))]
        private void DefineHandAI()
        {
            AutomaticFillComponents.DefineComponent(this, ref _enemyHand, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTableAI))]
        private void DefineTableAI()
        {
            AutomaticFillComponents.DefineComponent(this, ref _enemyTable, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTowerAI))]
        private void DefineTowerAI()
        {
            AutomaticFillComponents.DefineComponent(this, ref _enemyTower, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFight))]
        private void DefineFight()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fight, ComponentLocationTypes.InChildren);
        }
    }
}
