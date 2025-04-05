namespace Cards
{
    internal class CardSideFlipper
    {
        private readonly CardFront _front;
        private readonly CardBack _back;
        private readonly CardDragAndDrop _cardDragAndDrop;
        private readonly CardFrame _cardFrame;

        private SideType _currentSide;

        public CardSideFlipper(CardFront front, CardBack back, CardDragAndDrop cardDragAndDrop, CardFrame cardFrame)
        {
            _front = front;
            _back = back;
            _cardDragAndDrop = cardDragAndDrop;
            _cardFrame = cardFrame;
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
        }

        public void DeactivateInteraction()
        {
            _cardDragAndDrop.enabled = false;
            _front.Block();

            //switch (_currentSide)
            //{
            //    case SideType.Front:
            //        _cardFrame.Block();
            //        break;
            //    case SideType.Back:
            //        break;
            //    default:
            //        throw new System.Exception("Неизвестный SideType карты: " + _currentSide.ToString());
            //}

            //if (_currentSide == SideType.Front)
            //{
            //    _cardFrame.Block();
            //}

            //if (_currentSide == SideType.Back)
            //{
            //    _cardFrame.Block();
            //}
        }

        public void ActivateInteraction()
        {
            _cardDragAndDrop.enabled = true;
            _front.Unblock();

            //if (_currentSide == SideType.Front)
            //{
            //    _cardFrame.Unblock();
            //}
        }
    }
}