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
    public class Deck : MonoBehaviour, IAutomaticFillComponents, IDeckTake, IDeckView, ICardsCounter
    {
        [SerializeField] private DeckCardContainer _cardContainer;
        [SerializeField] private DeckCardBackViewer _cardBackViewer;
        [SerializeField] private DeckHelper _deckHelper;
        [SerializeField] private int _countCardsInGroup = 10;

        private readonly float _startCardAddPositionX = 0f;
        private readonly float _startCardAddPositionY = 0f;

        private List<Card> _cards;

        public event Action OnSeatsCountChange;

        public int CountCards => _cards.Count;

        public void Init(IEnumerable<Card> cards)
        {
            _cards = new List<Card>();
            _cardBackViewer.Init(_startCardAddPositionX, _startCardAddPositionY);
            _deckHelper.Init();

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

            return _cards.Where(c => exceptions.Contains(c.ViewData.Number) == false).Count() >= count;
        }

        public bool Contains(int number)
        {
            return _cards.Select(c => c.ViewData.Number).Contains(number);
        }

        public void AddCard(Card card)
        {
            int position = Random.Range(0, _cards.Count);
            _cards.Insert(position, card);

            OnSeatsCountChange?.Invoke();

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

            IReadOnlyList<Card> cards = Utils.Shuffle(_cards);

            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    if (existingIndices.Contains(cards[i].ViewData.Number) == false && exceptions.Contains(cards[i].ViewData.Number) == false)
                    {
                        result.Add(cards[i]);
                        existingIndices.Add(cards[i].ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (exceptions.Contains(cards[i].ViewData.Number) == false)
                        {
                            result.Add(cards[i]);
                            existingIndices.Add(cards[i].ViewData.Number);
                        }
                    }
                }
            }

            if (result.Count < count)
            {
                for (int c = result.Count - 1; c < count; c++)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        result.Add(cards[i]);
                        existingIndices.Add(cards[i].ViewData.Number);
                    }
                }
            }

            if (result.Count < count)
                throw new Exception("Ошибка вычисления чисел. Слишком мало карт!");

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

            OnSeatsCountChange?.Invoke();

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
                DeckCardBackViewer(),
                DeckCardDeckHelper()
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

        [ContextMenu(nameof(DeckCardDeckHelper))]
        private ComponentAttachInfo DeckCardDeckHelper()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _deckHelper, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}