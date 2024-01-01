using Cards;
using GameFields.FightProcess;
using Tools;
using UnityEngine;
using GameFields.Persons;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;

namespace GameFields
{
    public class GameField_2 : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private Player_2 _player;
        [SerializeField] private HandAI_2 _enemyHand;
        [SerializeField] private TableAI_2 _enemyTable;
        [SerializeField] private TowerAI_2 _enemyTower;
        [SerializeField] private int _countDrawCardsEnemy = 1;

        private EnemyAI_2 _enemyAI;
        private Fight_2 _fight;

        public void Init(Card[] cardsInDeck)
        {
            _fight = new Fight_2(_player, _enemyAI, _deck);

            InitDeck(cardsInDeck);
            InitEndTurnButton();
            InitPersons();
        }

        private void InitPersons()
        {
            _player.Init();
            _enemyAI = new EnemyAI_2(_enemyHand, _enemyTable, _enemyTower, _countDrawCardsEnemy);
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
    }
}
