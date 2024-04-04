using UnityEngine;
using Cards;
using System;
using System.Collections;

namespace GameFields.Persons.DrawCards
{
    [Serializable]
    internal class PlayerSimpleDrawCardAnimation
    {
        [SerializeField] private float _invertCardFrontDuration = 1f;
        [SerializeField] private float _invertCardBackDuration = 1f;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private float _invertCardBackRotation = 90f;
        [SerializeField] private float _invertCardBackScaleFactor = 1.8f;
        [SerializeField] private float _indent = 15f;

        internal IEnumerator StartAnimation(IHandSeatable drawnCard, Transform parent)
        {
            drawnCard.transform.SetParent(parent);
            drawnCard.transform.SetAsLastSibling();

            CardMovement cardMovement = new CardMovement(drawnCard.transform);

            drawnCard.CardMovement.InvertCardBackOnDraw(_invertCardBackDuration, _invertCardBackRotation, _invertCardBackScaleFactor, _indent);
            yield return new WaitForSeconds(_invertCardBackDuration);

            drawnCard.FlipOnPlayerDraw();

            drawnCard.CardMovement.InvertCardFrontOnDraw(_invertCardFrontDuration, _invertCardBackScaleFactor, _indent);
            yield return new WaitForSeconds(_invertCardFrontDuration + _delay);
            //yield return new WaitForSeconds(GetFullDelay());
        }

        //private float GetFullDelay()
        //{
        //    return _invertCardBackDuration + _invertCardFrontDuration + _delay;
        //}

        //private IEnumerator PlayingAnimation()
        //{
        //    _cardMovement.InvertCardBackOnDraw(_invertCardBackDuration, _invertCardBackRotation, _invertCardBackScaleFactor, _indent);
        //    yield return new WaitForSeconds(_invertCardBackDuration);

        //    _sideFlipper.SetFrontSide();
        //    _sideFlipper.Block();

        //    _cardMovement.InvertCardFrontOnDraw(_invertCardFrontDuration, _invertCardBackScaleFactor, _indent);
        //    yield return new WaitForSeconds(_invertCardFrontDuration);
        //}
    }
}
