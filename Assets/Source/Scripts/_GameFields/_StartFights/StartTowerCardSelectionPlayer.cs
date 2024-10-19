using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.CardTransits;
using GameFields.Persons.Discovers;
using GameFields.Seats;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionPlayer : StartTowerCardSelection
    {
        private readonly Deck _deck;
        private readonly IHandTransitSet _handTransitSet;
        private readonly Discover _discover;

        private readonly StartTowerCardSelectionPlayerData _data;

        private readonly Seat[] _seats;

        public StartTowerCardSelectionPlayer(Deck deck, Person person, Seat[] seats, Discover discover, StartTowerCardSelectionPlayerData data) : base(person)
        {
            _handTransitSet = person;
            _data = data;

            _seats = seats;
            _deck = deck;
            _discover = discover;

            InitSeats();
        }

        public override void StartProcess()
        {
            StartPlayerProcess().ToUniTask();
        }

        private IEnumerator StartPlayerProcess()
        {
            List<Card> cards = new List<Card>();

            yield return new WaitForSeconds(_data.WaitDurationForEnemyFirstCardsDraw);

            for (int i = 0; i < _seats.Length; i++)
            {
                Card card = _deck.TakeTopCard();
                cards.Add(card);
                _seats[i].SetCard(card, SideType.Front, _data.DrawCardsDuration, _data.DrawCardsScaleFactor);

                yield return new WaitForSeconds(_data.WaitDurationBetweenDrawCards);
            }

            if (_data.DrawCardsDuration - _data.WaitDurationBetweenDrawCards > 0)
            {
                yield return new WaitForSeconds(_data.DrawCardsDuration - _data.WaitDurationBetweenDrawCards);
            }

            foreach (Card card in cards)
            {
                card.gameObject.SetActive(false);
            }

            _discover.Activate(cards, _data.LabelMessage, OnCardChoiceDone);
        }

        private void OnCardChoiceDone(Card card)
        {
            foreach (Seat seat in _seats)
            {
                seat.Card.gameObject.SetActive(true);
            }

            EndProcessing(card).ToUniTask();
        }

        private IEnumerator EndProcessing(Card card)
        {
            yield return new WaitForSeconds(_data.DelayAfterCardChoiceDone);

            if (TowerTransitCheck.IsFill == false)
            {
                foreach (Seat seat in _seats)
                {
                    if (seat.Card != card)
                    {
                        _handTransitSet.Set(seat.Card);
                        seat.Card.SetActiveInteraction(false);
                        seat.Reset();
                    }
                    else
                    {
                        SeatCardInTower(seat).ToUniTask();
                    }
                }
            }
            else
            {
                throw new System.Exception("Что-то не так, работяги");
            }
        }

        private IEnumerator SeatCardInTower(Seat mySeat)
        {
            Card card = mySeat.Card;

            yield return new WaitForSeconds(_data.DelayBeforeStartProcessSeatCardInTower);

            InvertCardFront(card);
            yield return new WaitForSeconds(_data.InvertCardFrontDuration);

            card.SetSide(SideType.Back);

            InvertCardBack(card);
            yield return new WaitForSeconds(_data.InvertCardBackDuration + _data.DelayAfterInvert);

            mySeat.Reset();
            TowerTransitSet.Set(card);
        }

        private void InvertCardFront(Card card)
        {
            Vector3 position = card.Transform.position;

            Movement cardMovement = card.CardMovement;

            cardMovement.MoveLinear(position, new Vector3(0f, -90f, 0f), _data.InvertCardFrontDuration);
        }

        private void InvertCardBack(Card card)
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = card.Transform.position;

            Movement cardMovement = card.CardMovement;

            cardMovement.MoveSmoothly(position, endRotationVector, _data.InvertCardBackDuration, card.Transform.localScale);
        }

        private void InitSeats()
        {
            foreach (Seat seat in _seats)
            {
                seat.Init();
            }
        }
    }
}
