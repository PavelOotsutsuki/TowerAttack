using System;
using Tools;
using UnityEngine;

namespace Cards
{
    public class CardRoot : MonoBehaviour
    {
        [SerializeField] private CardDescription _cardDescription;
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private Card[] _cards;
        //[SerializeField] private CardBack _cardBack;

        public Card[] Cards => _cards;

        public void Init()
        {
            InitAll();
        }

        private void InitAll()
        {
            InitCardDescription();
            InitBigCard();
            InitCards();
            //InitDeckCard();
        }

        //private void InitDeckCard()
        //{
        //    _cardBack.Init();
        //    _cardBack.TakeCard();
        //}

        private void InitCardDescription()
        {
            _cardDescription.Init();
        }

        private void InitBigCard()
        {
            _bigCard.Init();
        }

        private void InitCards()
        {
            foreach (Card card in _cards)
            {
                card.Init(_cardDescription, _bigCard, true);
            }
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineAllCards();
            DefineCardDescription();
            DefineBigCard();
            //DefineDeckCard();
        }

        [ContextMenu(nameof(DefineAllCards))]
        private void DefineAllCards()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cards);
        }

        [ContextMenu(nameof(DefineCardDescription))]
        private void DefineCardDescription()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardDescription, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineBigCard))]
        private void DefineBigCard()
        {
            AutomaticFillComponents.DefineComponent(this, ref _bigCard, ComponentLocationTypes.InChildren);
        }

        //[ContextMenu(nameof(DefineDeckCard))]
        //private void DefineDeckCard()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _deckCard, ComponentLocationTypes.InChildren);
        //}
    }
}
