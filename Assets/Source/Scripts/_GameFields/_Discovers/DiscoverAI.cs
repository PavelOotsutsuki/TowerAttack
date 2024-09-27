using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Discovers
{
    public class DiscoverAI : Discover
    {
        [SerializeField] private float _minWaitDuration = 1.5f;
        [SerializeField] private float _maxWaitDuration = 5f;

        public override void Activate(List<Card> cards, string activateMessage, Action<Card> callback)
        {
            base.Activate(cards, activateMessage, callback);

            WaitingToSelect().ToUniTask();
        }

        private IEnumerator WaitingToSelect()
        {
            int selectedCardNumber = Random.Range(0, Cards.Count);
            float waitDuration = Random.Range(_minWaitDuration, _maxWaitDuration);

            yield return new WaitForSeconds(waitDuration);

            Seats[selectedCardNumber].StartClick();
        }
    }
}
