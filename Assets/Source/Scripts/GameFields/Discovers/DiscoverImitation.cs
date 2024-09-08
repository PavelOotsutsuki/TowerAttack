using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Discovers
{
    public class DiscoverImitation : MonoBehaviour, IDiscoverChoiceHandler, IDiscover
    {
        [SerializeField] private float _minWaitDuration = 1.5f;
        [SerializeField] private float _maxWaitDuration = 5f;
        [SerializeField, Min(1f)] private float _scaleFactor = 1.5f;
        [SerializeField] private float _offset = 400f;
        [SerializeField] private float _positionY = 0f;
        [SerializeField] private DiscoverSeatImitation[] _seats;

        private List<Card> _cards;
        private Action<Card> _callback;

        public int MaxSeats => _seats.Length;

        public void Init()
        {
            InitSeats();

            gameObject.SetActive(false);
        }

        public void Activate(List<Card> cards, string activateMessage, Action<Card> callback, float waitDuration)
        {
            _cards = cards;
            _callback = callback;

            SortDiscoverSeats();

            gameObject.SetActive(true);

            //for (int i = 0; i < cards.Count; i++)
            //{
            //    _seats[i].SetCard(cards[i], SideType.Front, 0.5f);
            //}

            //SetCards(cards).ToUniTask();
            //PlayingSelection().ToUniTask();

            for (int i = 0; i < cards.Count; i++)
            {
                _seats[i].SetCard(cards[i], waitDuration);
            }

            WaitingToSelect().ToUniTask();
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void OnMakeChoice(Card card)
        {
            foreach (DiscoverSeatImitation seat in _seats)
            {
                seat.Reset();
            }

            _callback?.Invoke(card);
            _callback = null;

            Deactivate();
        }

        private IEnumerator WaitingToSelect()
        {
            int selectedCardNumber = Random.Range(0, _cards.Count);
            float waitDuration = Random.Range(_minWaitDuration, _maxWaitDuration);

            yield return new WaitForSeconds(waitDuration);

            _seats[selectedCardNumber].StartClickImitation();
        }

        private void SortDiscoverSeats()
        {
            float startPositionX;
            Vector3 seatPosition;

            if (_cards.Count % 2 == 1)
            {
                startPositionX = (_cards.Count / 2 * _offset) * -1;
            }
            else
            {
                startPositionX = ((_cards.Count / 2 - 1) * _offset + _offset / 2) * -1;
            }

            for (int i = 0; i < _cards.Count; i++)
            {
                seatPosition = new Vector3(startPositionX + _offset * i, _positionY);

                _seats[i].SetLocalPositionValues(seatPosition, Quaternion.identity.eulerAngles);
            }
        }

        private void InitSeats()
        {
            foreach (DiscoverSeatImitation seat in _seats)
            {
                seat.Init(this, _scaleFactor);
            }
        }
    }
}
