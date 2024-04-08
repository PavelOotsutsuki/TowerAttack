using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Persons.PersonAnimators;
using Tools;
using UnityEngine;
using System.Collections.Generic;
using GameFields.Effects;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using System.Collections;
using System;

namespace GameFields.Persons
{
    internal class Player : MonoBehaviour, IPerson
    {
        [SerializeField] private HandPlayer _hand;
        [SerializeField] private TablePlayer _table;
        [SerializeField] private Tower _tower;
        [SerializeField] private Discover _discover;
        //[SerializeField] private PlayerAnimator _playerAnimator;
        //[SerializeField] private Transform _transform;
        [SerializeField] private int _countStartTurnDrawCards = 1;

        [SerializeField] private DrawCardRoot _drawCardRoot;

        private Deck _deck;

        public bool IsTowerFilled => _tower.IsTowerFill;

        public void Init(EffectRoot effectRoot, Deck deck, Action startDrawCallback)
        {
            _deck = deck;

            _hand.Init();
            _tower.Init(_hand);
            _table.Init(_hand, effectRoot);
            //            cardEffects.SetPlayerGameFieldElements(_table, _hand, _tower);

            _drawCardRoot.Init(_hand, startDrawCallback);
            _discover.Deactivate();
        }

        public List<Card> GetDiscardCards()
        {
            return _table.GetDiscardCards();
        }

        //public void UnbindHandsDragableCard()
        //{
        //    _hand.RemoveDraggableCard();
        //}

        public void ActivateDropPlaces()
        {
            _table.Activate();
            //_tower.Activate();
        }

        public void DeactivateDropPlaces()
        {
            _table.Deactivate();
            //_tower.Deactivate();
        }

        public void DrawCards(Queue<IHandSeatable> cards)
        {
            _drawCardRoot.TakeCards(cards);
        }

        public void StartTurnDraw()
        {
            Queue<IHandSeatable> cards = new Queue<IHandSeatable>();

            for (int i = 0; i < _countStartTurnDrawCards; i++)
            {
                if (_deck.IsHasCards(1))
                {
                    cards.Enqueue(_deck.TakeTopCard());
                }
            }

            if (cards != null)
            {
                _drawCardRoot.StartTurnDraw(cards);
            }
        }

        public void StartTowerCardSelection(int drawCardsCount)
        {
            Queue<IHandSeatable> cards = new Queue<IHandSeatable>();

            for (int i = 0; i < drawCardsCount; i++)
            {
                if (_deck.IsHasCards(1))
                {
                    cards.Enqueue(_deck.TakeTopCard());
                }
            }

            if (cards != null)
            {
                _drawCardRoot.TakeCards(cards);
            }
            //
        }

        //public IEnumerator StartTowerCardSelectionDraw()
        //{
        //    yield return _drawCardRoot.StartTowerCardSelectionDraw();
        //}

        //public IEnumerator PatriarchCorallDraw()
        //{
        //    yield return _drawCardRoot.PatriarchCorallDraw();
        //}

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineHandPlayer();
            DefineTablePlayer();
            DefineTowerPlayer();
            DefineDiscover();
            //DefinePlayerAnimator();
            //DefineTransform();
        }

        [ContextMenu(nameof(DefineHandPlayer))]
        private void DefineHandPlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _hand, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTablePlayer))]
        private void DefineTablePlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _table, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTowerPlayer))]
        private void DefineTowerPlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _tower, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDiscover))]
        private void DefineDiscover()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discover, ComponentLocationTypes.InChildren);
        }

        //[ContextMenu(nameof(DefinePlayerAnimator))]
        //private void DefinePlayerAnimator()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _playerAnimator, ComponentLocationTypes.InThis);
        //}

        //[ContextMenu(nameof(DefineTransform))]
        //private void DefineTransform()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        //}
    }
}
