using UnityEngine;

namespace Cards
{
    internal class DiscoverCardSideFlipper
    {
        private DiscoverCardFront _front;
        private CardBack _back;

        public DiscoverCardSideFlipper(DiscoverCardFront front, CardBack back)
        {
            _front = front;
            _back = back;
        }

        public bool IsFrontSide { get; private set; }

        public void SetBackSide()
        {
            Block();

            SetSide(true);
        }

        public void SetFrontSide()
        {
            SetSide(false);

            Unblock();
        }

        public void Block()
        {
            _front.Block();
        }

        public void Unblock()
        {
            _front.Unblock();
        }

        private void SetSide(bool isBackSide)
        {
            _front.gameObject.SetActive(isBackSide == false);
            _back.gameObject.SetActive(isBackSide);
            IsFrontSide = isBackSide == false;
        }
    }
}
