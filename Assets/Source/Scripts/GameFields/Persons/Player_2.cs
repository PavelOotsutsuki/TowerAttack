using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons
{
    public class Player_2 : MonoBehaviour, Person_2
    {
        [SerializeField] private int _countDrawCards = 1;
        [SerializeField] private HandPlayer_2 _hand;
        [SerializeField] private TablePlayer_2 _table;
        [SerializeField] private TowerPlayer_2 _tower;
        [SerializeField] private DrawCardAnimator _drawCardAnimator;
        [SerializeField] private CanvasScaler _canvasScaler;

        //private PersonFightActions _personFightActions;
        public int CountDrawCards => _countDrawCards;

        public void Init()
        {
            _hand.Init();
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
            _drawCardAnimator.Init(_hand, card, _canvasScaler);
        }

        //public void RemoveCard(Card card)
        //{
        //    _hand.RemoveCard(card);
        //}


        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineHand();
            DefineTable();
            DefineTower();
            DefineDrawCardAnimator();
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

        [ContextMenu(nameof(DefineDrawCardAnimator))]
        private void DefineDrawCardAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _drawCardAnimator, ComponentLocationTypes.InThis);
        }
    }
}
