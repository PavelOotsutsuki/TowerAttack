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

        public void SetOverview(IShowable showableCard, CardSize cardSize, float positionX, CardSO cardSO)
        {
            ShowCurrentCard();

            _currentCard = showableCard;

            _cardDescription.Show(cardSO.Description);
            _bigCard.Show(cardSize, positionX, cardSO);
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
            if (_currentCard is not null)
            {
                _currentCard.Show();
            }
        }
    }
}
