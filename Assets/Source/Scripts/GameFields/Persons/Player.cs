using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Persons.PersonAnimators;
using Tools;
using UnityEngine;

namespace GameFields.Persons
{
    public class Player : MonoBehaviour, IPerson
    {
        [SerializeField] private int _countDrawCards = 1;
        [SerializeField] private float _drawCardsDelay = 2.5f;
        [SerializeField] private HandPlayer _hand;
        [SerializeField] private TablePlayer _table;
        [SerializeField] private TowerPlayer _tower;
        [SerializeField] private PlayerAnimator _playerAnimator;

        public int CountDrawCards => _countDrawCards;
        public float DrawCardsDelay => _drawCardsDelay;

        public void Init()
        {
            _hand.Init();
            _table.Init(this);
            _tower.Init(this);
            _playerAnimator.Init(_hand);
        }

        public void PlayCard(Card card)
        {
            _hand.RemoveCard(card);
        }

        public void DeactivateTable()
        {
            _table.Deactivate();
        }

        public void ActivateTable()
        {
            _table.Activate();
        }

        public void DrawCard(Card card)
        {
            card.SetDragAndDropListener(_hand);
            _playerAnimator.StartDrawCardAnimation(card);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineHand();
            DefineTable();
            DefineTower();
            DefinePlayerAnimator();
        }

        [ContextMenu(nameof(DefineHand))]
        private void DefineHand()
        {
            AutomaticFillComponents.DefineComponent(this, ref _hand, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTable))]
        private void DefineTable()
        {
            AutomaticFillComponents.DefineComponent(this, ref _table, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTower))]
        private void DefineTower()
        {
            AutomaticFillComponents.DefineComponent(this, ref _tower, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefinePlayerAnimator))]
        private void DefinePlayerAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _playerAnimator, ComponentLocationTypes.InThis);
        }
    }
}
