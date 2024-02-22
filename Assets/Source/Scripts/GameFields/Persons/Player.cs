using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Persons.PersonAnimators;
using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

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
        [SerializeField] private Transform _transform;

        public int CountDrawCards => _countDrawCards;
        public float DrawCardsDelay => _drawCardsDelay;

        public void Init(IStartFightListener startFightListener)
        {
            _hand.Init();
            _table.Init(this);
            InitTower(startFightListener);
            _playerAnimator.Init(_hand, _transform);
        }

        public List<Card> GetDiscardCards()
        {
            return _table.GetDiscardCards();
        }

        public TowerCardSelector CreateTowerSelector()
        {
            return new TowerCardSelector(_tower, _hand);
        }

        public void PlayCard(Card card)
        {
            _hand.RemoveCard(card);
        }

        public void ActivateDropPlaces()
        {
            _table.Activate();
            _tower.Activate();
        }

        public void DeativateDropPlaces()
        {
            _table.Deactivate();
            _tower.Deactivate();
        }

        public void DrawCard(Card card)
        {
            card.SetDragAndDropListener(_hand);
            _playerAnimator.StartDrawCardAnimation(card).ToUniTask();
        }

        private void InitTower(IStartFightListener startFightListener)
        {
            _tower.Init(this);
            _tower.SetStartFightListener(startFightListener);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineHand();
            DefineTable();
            DefineTower();
            DefinePlayerAnimator();
            DefineTransform();
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

        [ContextMenu(nameof(DefineTransform))]
        private void DefineTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
    }
}
