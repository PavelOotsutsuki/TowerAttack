using System.Collections.Generic;
using UnityEngine;
using Cards;
using Tools.Utils.FillComponents;
using Tools;
using Tools.Utils.Movements;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using Tools.Utils;

namespace GameFields.Decks
{
    public class Deck : MonoBehaviour, IAutomaticFillComponents, IDeckTake, IDeckView
    {
        [SerializeField] private DeckCardContainer _cardContainer;
        [SerializeField] private DeckCardBackViewer _cardBackViewer;
        [SerializeField] private int _countCardsInGroup = 10;

        private readonly float _startCardAddPositionX = 0f;
        private readonly float _startCardAddPositionY = 0f;

        private List<Card> _cards;

        public void Init(IEnumerable<Card> cards)
        {
            _cards = new List<Card>();
            _cardBackViewer.Init(_startCardAddPositionX, _startCardAddPositionY);

            foreach (Card card in cards)
            {
                _cards.Add(card);
                BindCard(card.ReadOnlyRectTransform, card.CardMovement);
            }

            ShuffleCards();
        }

        public bool IsHasCards(int count, IEnumerable<int> exceptions = null)
        {
            exceptions ??= new List<int>();

            return _cards.Where(c => exceptions.Contains(c.ViewConfig.Number) == false).Count() >= count;
        }

        public void AddCard(Card card)
        {
            int position = Random.Range(0, _cards.Count);
            _cards.Insert(position, card);
            BindCard(card.ReadOnlyRectTransform, card.CardMovement);

            ShuffleCards();
        }

        public Card TakeTopCard()
        {
            return TakeCardByIndex(_cards.Count - 1);
        }

        public Card TakeCard(Card card)
        {
            RemoveCard(card);

            return card;
        }

        public IReadOnlyList<Card> ViewRandomCards(int count, IEnumerable<int> exceptions)
        {
            List<int> existingIndices = new List<int>();
            List<Card> result = new List<Card>();

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, _cards.Count);

                while (existingIndices.Contains(randomIndex) || exceptions.Contains(randomIndex))
                {
                    randomIndex = Random.Range(0, _cards.Count);
                }

                result.Add(_cards[randomIndex]);
            }

            return result;
        }

        public Card ViewCardFromEndDeck(int index = 0)
        {
            return _cards[_cards.Count - 1 - index];
        }

        private Card TakeCardByIndex(int index)
        {
            Card card = _cards[index];

            RemoveCard(card);

            return card;
        }

        private void ShuffleCards()
        {
            _cards = Utils.Shuffle(_cards);
            //List<Card> shuffleCards = new List<Card>();

            //while (_cards.Count > 0)
            //{
            //    Card card = _cards[Random.Range(0, _cards.Count)];
            //    shuffleCards.Add(card);
            //    _cards.Remove(card);
            //}

            //_cards = shuffleCards;
        }

        private void RemoveCard(Card card)
        {
            _cards.Remove(card);

            if (_cards.Count % _countCardsInGroup == 0)
            {
                _cardBackViewer.Remove();
            }
        }

        private void BindCard(ReadOnlyTransform cardTransform, Movement cardMovement)
        {
            cardTransform.SetParent(_cardContainer.GetTransform());

            Vector2 cardAddPosition = new Vector2(_startCardAddPositionX, _startCardAddPositionY);

            cardMovement.MoveLocalInstantly(cardAddPosition, cardTransform.GetRotationVector());

            if (_cards.Count % _countCardsInGroup == 1)
            {
                _cardBackViewer.Add();
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Deck))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineDeckCardContainer(),
                DeckCardBackViewer()
            };

            return list;
        }

        [ContextMenu(nameof(DefineDeckCardContainer))]
        private ComponentAttachInfo DefineDeckCardContainer()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardContainer, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DeckCardBackViewer))]
        private ComponentAttachInfo DeckCardBackViewer()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardBackViewer, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}