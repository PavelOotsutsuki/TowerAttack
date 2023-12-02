using UnityEngine;

namespace Cards
{
    public class CardSideFlipper
    {
        private GameObject _front;
        private GameObject _back;

        public CardSideFlipper(GameObject front, GameObject back)
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

        private void SetSide(bool isBackSide)
        {
            _front.SetActive(isBackSide == false);
            _back.SetActive(isBackSide);
        }
    }
}
