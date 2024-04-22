using Cards;
using GameFields.DiscardPiles;
using GameFields.Effects;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.PersonAnimators;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Persons
{
    [Serializable]
    public class EnemyAI : IPerson
    {
        [SerializeField] private EnemyAiData _enemyAiData;

        private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        private HandAI _hand;
        private TableAI _table;
        private Tower _tower;
        private DrawCardRoot _drawCardRoot;

        private CardDragAndDropImitationActions _cardDragAndDropImitationActions;

        //private IEndTurnHandler _endTurnHandler;
        private Deck _deck;
        private DiscardPile _discardPile;

        private bool _isImitationComplete;

        public bool IsTowerFilled => _tower.IsTowerFill;
        public bool IsImitationComplete => _isImitationComplete;

        //public EnemyAI(/*IEndTurnHandler endTurnHandler, */EffectRoot effectRoot, Deck deck, DiscardPile discardPile, SeatPool seatPool, EnemyAiData enemyAiData)
        //{
        //    _deck = deck;
        //    _discardPile = discardPile;
        //    //_endTurnHandler = endTurnHandler;

        //    _enemyDragAndDropImitation = enemyAiData.EnemyDragAndDropImitation;
        //    _hand = enemyAiData.Hand;
        //    _table = enemyAiData.Table;
        //    _tower = enemyAiData.Tower;
        //    _drawCardRoot = enemyAiData.DrawCardRoot;

        //    _hand.Init(seatPool);
        //    _tower.Init(_hand);
        //    _table.Init(_hand, effectRoot);

        //    _drawCardRoot.Init(_hand);
        //    _cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_hand, _table);
        //    _enemyDragAndDropImitation.Init(_cardDragAndDropImitationActions, CompleteImitation);
        //    //_enemyAnimator.Init(_endTurnHandler, _cardDragAndDropImitationActions);
        //}

        public void Init(/*IEndTurnHandler endTurnHandler, */EffectRoot effectRoot, Deck deck, DiscardPile discardPile, SeatPool seatPool)
        {
            _deck = deck;
            _discardPile = discardPile;
            //_endTurnHandler = endTurnHandler;

            _enemyDragAndDropImitation = _enemyAiData.EnemyDragAndDropImitation;
            _hand = _enemyAiData.Hand;
            _table = _enemyAiData.Table;
            _tower = _enemyAiData.Tower;
            _drawCardRoot = _enemyAiData.DrawCardRoot;

            _hand.Init(seatPool);
            _tower.Init(_hand);
            _table.Init(_hand, effectRoot);

            _drawCardRoot.Init(_hand);
            _cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_hand, _table);
            _enemyDragAndDropImitation.Init(_cardDragAndDropImitationActions, CompleteImitation);
            //_enemyAnimator.Init(_endTurnHandler, _cardDragAndDropImitationActions);
        }

        public void DrawCards(Queue<Card> cards)
        {
            _drawCardRoot.TakeCards(cards);
        }

        //public void UnbindHandsDragableCard()
        //{
        //    _hand.RemoveDraggableCard();
        //}

        //public List<Card> GetDiscardCards()
        //{
        //    return _table.GetDiscardCards();
        //}

        public void DiscardCards()
        {
            _discardPile.DiscardCards(_table.GetDiscardCards());
        }

        public void StartTurnDraw()
        {
            Queue<Card> cards = new Queue<Card>();

            for (int i = 0; i < _enemyDragAndDropImitation.CountDrawCards; i++)
            {
                if (_deck.IsHasCards(1))
                {
                    cards.Enqueue(_deck.TakeTopCard());
                }
            }

            _isImitationComplete = false;

            if (cards.Count > 0)
            {
                _drawCardRoot.TakeCards(cards, PlayDragAndDropImitation);
            }
            else
            {
                PlayDragAndDropImitation();
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

            if (cards.Count > 0)
            {
                _drawCardRoot.TakeCards(cards);//, PlayDragAndDropImitation);
            }
            //else
            //{
            //    PlayDragAndDropImitation();
            //}
        }

        private void PlayDragAndDropImitation()
        {
            if (_hand.TryGetCard(out Card card))
            {
                _cardDragAndDropImitationActions.SetCard(card);
                _enemyDragAndDropImitation.StartDragAndDropAnimation();
            }
            else
            {
                //_endTurnHandler.OnEndTurn();
                CompleteImitation();
            }
        }

        private void CompleteImitation()
        {
            _isImitationComplete = true;
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
