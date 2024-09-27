using UnityEngine;

namespace Cards
{
    internal class CardViewService
    {
        private BigCard _bigCard;
        private CardDescription _cardDescription;
        private IShowable _currentCard;

        public CardViewService(BigCard bigCard, CardDescription cardDescription)
        {
            _bigCard = bigCard;
            _cardDescription = cardDescription;
            _currentCard = null;
        }

        public void SetOverview(IShowable showableCard, Vector2 cardSize, float positionX, CardViewConfig cardViewConfig)
        {
            ShowCurrentCard();

            _currentCard = showableCard;

            _cardDescription.Show(cardViewConfig.Description);
            _bigCard.Show(cardSize, positionX, cardViewConfig);
            _currentCard.Hide();
        }

        public void SetDefaultView()
        {
            _cardDescription.Hide();
            _bigCard.Hide();

            ShowCurrentCard();
        }

        private void ShowCurrentCard()
        {
            _currentCard?.Show();
        }
    }
}
