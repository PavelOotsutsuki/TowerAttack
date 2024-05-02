using System.Collections;
using System;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.DrawCards
{
    public class PlayerDrawCardAnimator//: SimpleDrawCardAnimation
    {
        //[SerializeField] private PlayerSimpleDrawCardAnimation _simpleDrawAnimation;
        //[SerializeField] private Transform _dragImitationContainer;

        //public override void PlayingSimpleDrawCardAnimation(Card drawnCard)
        //{
        //    Playing(drawnCard).ToUniTask();
        //}

        //private IEnumerator Playing(Card drawnCard)
        //{
        //    IsDone = false;

        //    //_simpleDrawAnimation.SetCard(drawnCard);
        //    yield return new WaitForSeconds(0.1f);
        //    //yield return _simpleDrawAnimation.StartAnimation(drawnCard, _dragImitationContainer);

        //    _hand.AddCard(drawnCard);
        //    IsDone = true;
      //  }
    }
}
