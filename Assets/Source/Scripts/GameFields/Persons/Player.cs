using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using System.Collections.Generic;
using GameFields.Effects;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.DiscardPiles;
using GameFields.Seats;
using System;
using UnityEngine;

namespace GameFields.Persons
{
    [Serializable]
    internal class Player : IPerson
    {
        [SerializeField] private PlayerData _playerData;

        private HandPlayer _hand;
        private TablePlayer _table;
        private Tower _tower;
        private Discover _discover;
        private int _countStartTurnDrawCards;
        private DrawCardRoot _drawCardRoot;

        private Deck _deck;
        private DiscardPile _discardPile;

        public bool IsTowerFilled => _tower.IsTowerFill;

        //public Player(EffectRoot effectRoot, Deck deck, DiscardPile discardPile, SeatPool seatPool, PlayerData playerData)
        //{
        //    //_hand.Init(seatPool);
        //    //_tower.Init(_hand);
        //    //_table.Init(_hand, effectRoot);
        //    //            cardEffects.SetPlayerGameFieldElements(_table, _hand, _tower);

        //    //_drawCardRoot.Init(_hand);
        //    //_discover.Deactivate();

        //    //string handPath = "Prefabs/HandPanels/HandPanelPlayer";

        //    //HandPlayer handPlayerPrefab = Resources.Load<HandPlayer>(handPath);
        //    //_hand = Instantiate(handPlayerPrefab, transform);


        //    _deck = deck;
        //    _discardPile = discardPile;

        //    _hand = playerData.Hand;
        //    _table = playerData.Table;
        //    _tower = playerData.Tower;
        //    _discover = playerData.Discover;
        //    _countStartTurnDrawCards = playerData.CountStartTurnDrawCards;

        //    _hand.Init(seatPool);
        //    _tower.Init(_hand);
        //    _table.Init(_hand, effectRoot);

        //    _drawCardRoot.Init(_hand);
        //    _discover.Deactivate();
        //}

        public void Init(EffectRoot effectRoot, Deck deck, DiscardPile discardPile, SeatPool seatPool)
        {
            //_hand.Init(seatPool);
            //_tower.Init(_hand);
            //_table.Init(_hand, effectRoot);
            //            cardEffects.SetPlayerGameFieldElements(_table, _hand, _tower);

            //_drawCardRoot.Init(_hand);
            //_discover.Deactivate();

            //string handPath = "Prefabs/HandPanels/HandPanelPlayer";

            //HandPlayer handPlayerPrefab = Resources.Load<HandPlayer>(handPath);
            //_hand = Instantiate(handPlayerPrefab, transform);


            _deck = deck;
            _discardPile = discardPile;

            _hand = _playerData.Hand;
            _table = _playerData.Table;
            _tower = _playerData.Tower;
            _discover = _playerData.Discover;
            _countStartTurnDrawCards = _playerData.CountStartTurnDrawCards;
            _drawCardRoot = _playerData.DrawCardRoot;

            _hand.Init(seatPool);
            _tower.Init(_hand);
            _table.Init(_hand, effectRoot);

            _drawCardRoot.Init(_hand);
            _discover.Deactivate();
        }

        public void DiscardCards()
        {
            _discardPile.DiscardCards(_table.GetDiscardCards());
        }

        //public List<Card> GetDiscardCards()
        //{
        //    return _table.GetDiscardCards();
        //}

        //public void UnbindHandsDragableCard()
        //{
        //    _hand.RemoveDraggableCard();
        //}

        public void ActivateDropPlaces()
        {
            _table.Activate();
        }

        public void DeactivateDropPlaces()
        {
            _table.Deactivate();
        }

        public void DrawCards(Queue<Card> cards)
        {
            _drawCardRoot.TakeCards(cards);
        }

        public void StartTurnDraw()
        {
            Queue<Card> cards = new Queue<Card>();

            for (int i = 0; i < _countStartTurnDrawCards; i++)
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
        }

        public void StartTowerCardSelection(int drawCardsCount)
        {
            Queue<Card> cards = new Queue<Card>();

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
    }
}
