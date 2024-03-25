namespace Cards
{
    internal class CardSides : ICardSides
    {
        private readonly CardFront _front;
        private readonly CardBack _back;

        public CardSides(CardFront front, CardBack back)
        {
            _front = front;
            _back = back;
        }

        public void SetBackSide()
        {
            SetSide(true);
        }

        public void SetFrontSide()
        {
            SetSide(false);
        }

        internal void Disable()
        {
            _front.Disable();
        }

        internal void Enable()
        {
            _front.Enable();
        }

        private void SetSide(bool isBackSide)
        {
            _front.gameObject.SetActive(isBackSide == false);
            _back.gameObject.SetActive(isBackSide);
        }
    }
}
