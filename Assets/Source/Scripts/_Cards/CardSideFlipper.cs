namespace Cards
{
    internal class CardSideFlipper
    {
        private readonly CardFront _front;
        private readonly CardBack _back;
        private readonly CardDragAndDrop _cardDragAndDrop;
        private readonly CardFrame _cardFrame;
        private readonly CardSpriteManager _cardSpriteManager;

        private SideType _currentSide;

        public CardSideFlipper(CardFront front, CardBack back, CardDragAndDrop cardDragAndDrop, CardFrame cardFrame,
            CardSpriteManager cardSpriteManager)
        {
            _front = front;
            _back = back;
            _cardDragAndDrop = cardDragAndDrop;
            _cardFrame = cardFrame;
            _cardSpriteManager = cardSpriteManager;
        }

        public SideType CurrentSide => _currentSide;

        public void SetSide(SideType side)
        {
            _front.gameObject.SetActive(side == SideType.Front);
            _back.gameObject.SetActive(side == SideType.Back);

            _currentSide = side;

            if (_currentSide == SideType.Back)
            {
                _cardFrame.Neutral();
            }

            if (side == SideType.Front)
            {
                _cardSpriteManager.Activate();
            }
            else
            {
                _cardSpriteManager.Deactivate();
            }
        }

        public void ActivateInteraction()
        {
            _cardDragAndDrop.enabled = true;
            _front.Unblock();
        }

        public void DeactivateInteraction()
        {
            _cardDragAndDrop.enabled = false;
            _front.Block();
        }
    }
}