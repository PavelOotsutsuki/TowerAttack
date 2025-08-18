using Tools;
using UnityEngine;

namespace Cards
{
    internal class CardViewService
    {
        private readonly BigCardRoot _bigCardRoot;
        private readonly CardDescription _cardDescription;

        private IViewable _currentCard;
        private IViewable _currentCardFrame;

        public CardViewService(BigCardRoot bigCardRoot, CardDescription cardDescription)
        {
            _bigCardRoot = bigCardRoot;
            _cardDescription = cardDescription;
            _currentCard = null;
        }

        public bool IsView(IViewable viewable)
        {
            return _currentCard == viewable;
        }

        public void SetOverview(IViewable showableCard, BigCardRootActivateData BigCardRootActivateData, IViewable cardFrame)
        {
            ShowCurrentCard();

            _currentCard = showableCard;
            _currentCardFrame = cardFrame;

            _cardDescription.Show(BigCardRootActivateData.BigCardShowData.LabelData);
            _bigCardRoot.Activate(BigCardRootActivateData);
            _currentCard.Hide();
            _currentCardFrame.Hide();
        }

        public void SetDefaultView()
        {
            _cardDescription.Hide();
            _bigCardRoot.Deactivate();

            ShowCurrentCard();
        }

        private void ShowCurrentCard()
        {
            _currentCard?.Show();
            _currentCardFrame?.Show();
        }
    }
}