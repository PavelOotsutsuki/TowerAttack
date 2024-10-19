using Tools;
using UnityEngine;

namespace Cards
{
    internal class CardViewService
    {
        private readonly BigCard _bigCard;
        private readonly CardDescription _cardDescription;

        private IViewable _currentCard;

        public CardViewService(BigCard bigCard, CardDescription cardDescription)
        {
            _bigCard = bigCard;
            _cardDescription = cardDescription;
            _currentCard = null;
        }

        public void SetOverview(IViewable showableCard, BigCardShowData bigCardShowData)
        {
            ShowCurrentCard();

            _currentCard = showableCard;

            _cardDescription.Show(bigCardShowData.CardViewConfig.Description);
            _bigCard.Show(bigCardShowData);
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