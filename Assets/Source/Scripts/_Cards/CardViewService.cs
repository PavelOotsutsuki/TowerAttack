using Tools;

namespace Cards
{
    internal class CardViewService
    {
        private readonly BigCardRoot _bigCardRoot;

        private IViewable _currentCard;
        private IViewable _currentCardFrame;

        public CardViewService(BigCardRoot bigCardRoot)
        {
            _bigCardRoot = bigCardRoot;
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

            _bigCardRoot.Activate(BigCardRootActivateData);
            _currentCard.Hide();
            _currentCardFrame.Hide();
        }

        public void SetDefaultView()
        {
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