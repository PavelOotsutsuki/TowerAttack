namespace Cards
{
    internal class CardViewService
    {
        private BigCard _bigCard;
        private CardDescription _cardDescription;
        private IShowable _currentShowableCard;

        public CardViewService(BigCard bigCard, CardDescription cardDescription)
        {
            _bigCard = bigCard;
            _cardDescription = cardDescription;
            _currentShowableCard = null;
        }

        public void Show(IShowable showableCard, CardSize cardSize, float positionX, CardSO cardSO)
        {
            if (_currentShowableCard is not null)
            {
                _currentShowableCard.Show();
            }

            _currentShowableCard = showableCard;

            _cardDescription.Show(cardSO.Description);
            _bigCard.Show(cardSize, positionX, cardSO);
            _currentShowableCard.Hide();
        }

        public void Hide()
        {
            _cardDescription.Hide();
            _bigCard.Hide();
            _currentShowableCard.Show();
        }
    }
}
