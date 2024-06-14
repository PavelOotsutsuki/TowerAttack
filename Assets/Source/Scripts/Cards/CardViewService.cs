using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    internal class CardViewService
    {
        private BigCard _bigCard;
        private CardDescription _cardDescription;
        private CardFront _currentCardFront;

        public CardViewService(BigCard bigCard, CardDescription cardDescription)
        {
            _bigCard = bigCard;
            _cardDescription = cardDescription;
            _currentCardFront = null;
        }

        public void ShowBigView(CardFront currentCardFront)
        {
            if (_currentCardFront is not null)
            {
                _currentCardFront.EndReview();
            }

            _currentCardFront = currentCardFront;

            //_currentCardCanvasGroup.alpha = 0;
            //_bigCard.Show();

        }
    }
}
