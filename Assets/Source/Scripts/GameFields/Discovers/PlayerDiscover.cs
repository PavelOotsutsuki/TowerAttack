using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public class PlayerDiscover : MonoBehaviour, IDiscoverChoiceHandler, IDiscover
    {
        [SerializeField] private float _offset = 400f;
        [SerializeField] private float _positionY = 0f;
        [SerializeField] private DiscoverPanel _discoverPanel;
        [SerializeField] private DiscoverLabel _discoverLabel;
        [SerializeField] private PlayerDiscoverSeat[] _seats;

        private List<Card> _cards;
        private Action<Card> _callback;

        public int MaxSeats => _seats.Length;

        public void Init()
        {
            _discoverPanel.Init();
            _discoverLabel.Init();

            //_discoverSeatsPool.Init();
            InitSeats();

            gameObject.SetActive(false);
        }

        public void OnMakeChoice(Card card)
        {
            foreach (PlayerDiscoverSeat seat in _seats)
            {
                seat.Reset();
            }

            _callback?.Invoke(card);
            _callback = null;

            Deactivate();
        }

        public void Activate(List<Card> cards, string activateMessage, Action<Card> callback)
        {
            _cards = cards;
            _callback = callback;

            SortDiscoverSeats();

            _discoverPanel.Activate();
            _discoverLabel.Activate(activateMessage);

            gameObject.SetActive(true);

            //for (int i = 0; i < cards.Count; i++)
            //{
            //    _seats[i].SetCard(cards[i], SideType.Front, 0.5f);
            //}

            //SetCards(cards).ToUniTask();
            //PlayingSelection().ToUniTask();

            for (int i = 0; i < cards.Count; i++)
            {
                _seats[i].SetCard(cards[i]);
            }
            //TakeCards(deck, player).ToUniTask();
            //TakeCards(deck, enemy).ToUniTask();
        }

        //public IEnumerator SetCards(List<Card> cards)
        //{
        //    yield return new WaitForSeconds(1f);

        //    for (int i = 0; i < cards.Count; i++)
        //    {
        //        _seats[i].SetCard(cards[i]);
        //    }
        //}

        public void Deactivate()
        {
            _discoverPanel.Deactivate();
            _discoverLabel.Deactivate();

            gameObject.SetActive(false);
        }

        //private IEnumerator TakeCards(Deck deck, IDrawCardManager person)
        //{
        //    Card takenCard;

        //    for (int i = 0; i < _firstTurnCardsCount; i++)
        //    {
        //        takenCard = deck.TakeTopCard();
        //        person.DrawCard(takenCard);
        //        yield return new WaitForSeconds(person.DrawCardsDelay);
        //    }
        //}

        //private IEnumerator PlayingSelection()
        //{
        //    yield break;
        //}

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
            foreach (PlayerDiscoverSeat seat in _seats)
            {
                seat.Init(this);
            }
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDiscoverPanel();
            DefineDiscoverLabel();
            DefineSeats();
        }

        [ContextMenu(nameof(DefineDiscoverPanel))]
        private void DefineDiscoverPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discoverPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDiscoverLabel))]
        private void DefineDiscoverLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discoverLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSeats))]
        private void DefineSeats()
        {
            AutomaticFillComponents.DefineComponent(this, ref _seats);
        }

        #endregion
    }
}
