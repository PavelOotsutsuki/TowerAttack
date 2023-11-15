using UnityEngine;

namespace Cards
{
    internal class CardDragAndDropActions
    {
        private CardFront _cardFront;
        private CardMovement _cardMovement;

        internal CardDragAndDropActions(CardFront cardFront, CardMovement cardMovement)
        {
            _cardFront = cardFront;
            _cardMovement = cardMovement;
        }

        //internal void EndReview()
        //{
        //    if (_cardFront.IsBlock == false)
        //    {
        //        _cardFront.EndReview();
        //    }
        //}

        internal void StartDrag()
        {
            //_cardFront.Block();

            if (_cardFront.IsBlock == false)
            {
                _cardFront.EndReview();
            }

            _cardFront.Block();
        }

        internal void EndDrag(bool isPointerOnCard)
        {
            _cardFront.Unblock();

            if (isPointerOnCard)
            {
                if (_cardFront.IsBlock == false)
                {
                    _cardFront.StartReview();
                }
            }
        }

        //internal void BlockReview()
        //{
        //    _cardFront.Block();
        //}
        //internal void DisableRaycasts()
        //{
        //    _cardFront.DisableRaycasts();
        //}

        //internal void EnableRaycasts()
        //{
        //    _cardFront.EnableRaycasts();
        //}

        internal void ReturnInHand(Vector2 positon, Vector3 rotation, float duration)
        {
            Vector3 scaleVector = new Vector3(1f, 1f, 1f);
            _cardMovement.TranslateSmoothly(positon, rotation, duration, scaleVector);
        }
    }
}
