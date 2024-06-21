using Tools;
using UnityEngine;

namespace Cards
{
    public class CardRoot : MonoBehaviour
    {
        [SerializeField] private CardDescription _cardDescription;
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private Card[] _cards;
        [SerializeField] private Transform _dragContainer;

        internal CardViewService _cardViewService;
        public Card[] Cards => _cards;

        public void Init()
        {
            InitCardDescription();
            InitBigCard();

            _cardViewService = new CardViewService(_bigCard, _cardDescription);

            InitCards();
        }

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
                card.Init(_cardViewService, _dragContainer);
            }
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineAllCards();
            DefineCardDescription();
            DefineBigCard();
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
    }
}
