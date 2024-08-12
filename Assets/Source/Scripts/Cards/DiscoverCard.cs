using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Cards
{
    public class DiscoverCard : MonoBehaviour//, ISeatable
    {
        [SerializeField] private CardBack _cardBack;
        [SerializeField] private DiscoverCardFront _discoverCardFront;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector3 _defaultScaleVector;

        //private CardAnimator _cardAnimator;
        private CardConfig _cardConfig;
        //private CardMovement _cardMovement;
        private DiscoverCardSideFlipper _discoverCardSideFlipper;

        internal void Init(CardConfig cardConfig, CardDescription cardDescription)
        {
            _cardConfig = cardConfig;

            _rectTransform.localScale = _defaultScaleVector;

            //_cardMovement = new CardMovement(_rectTransform, _defaultScaleVector);

            _discoverCardFront.Init(_cardConfig, cardDescription);

            _discoverCardSideFlipper = new DiscoverCardSideFlipper(_discoverCardFront, _cardBack);
            _discoverCardSideFlipper.SetBackSide();

            //_cardAnimator = new CardAnimator(_rectTransform, _cardMovement, _discoverCardSideFlipper);
        }

        public void BindSeat(Transform transform, bool isFrontSide, float duration = 0f)
        {
            _rectTransform.SetParent(transform);
            //_cardMovement.BindSeatMovement(duration);

            if (isFrontSide)
            {
                _discoverCardSideFlipper.SetFrontSide();
            }
            else
            {
                _discoverCardSideFlipper.SetBackSide();
            }
        }

        //public void Drawn(float cardBackDuration, float cardBackRotation, float cardBackScaleFactor, float cardFrontDuration, float indent)
        //{
        //    _cardAnimator.PlayDrawnCardAnimation(cardBackDuration, cardBackRotation, cardBackScaleFactor, cardFrontDuration, indent);
        //}

        //public void PlayOnPlace(Vector3 center, float duration)
        //{
        //    //_cardMovement.MoveOnPlace(center, duration);
        //}

        //public void ReturnToHand(float duration)
        //{
        //    _cardMovement.MoveReturnToHand(duration);
        //}

        private void Activate()
        {
            gameObject.SetActive(true);
        }

        private void Destroy()
        {
            gameObject.SetActive(false);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineCardBack();
            DefineCardFront();
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCardBack))]
        private void DefineCardBack()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardBack, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardFront))]
        private void DefineCardFront()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discoverCardFront, ComponentLocationTypes.InChildren);
        }
    }
}
