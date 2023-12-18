using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hands;
using Tables;
using Cards;
using Tools;
using Towers;

namespace Persons
{
    public abstract class Person : MonoBehaviour, IPlayCardManager
    {
        [SerializeField] private int _countDrawCards = 1;
        [SerializeField] private HandOwner _handOwner;
        [SerializeField] private Hand _hand;
        [SerializeField] private Table _table;
        [SerializeField] private Tower _tower;

        //private PersonFightActions _personFightActions;
        public int CountDrawCards => _countDrawCards;

        public void Init()
        {
            _hand.Init(_handOwner);
            _table.Init(this);
            _tower.Init(this);
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
            card.SetEndDragListener(_hand);
            DrawCard(card, _hand);
        }

        //public void RemoveCard(Card card)
        //{
        //    _hand.RemoveCard(card);
        //}

        protected abstract void DrawCard(Card drawnCard, Hand hand);

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineHand();
            DefineTable();
            DefineTower();
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
    }
}
