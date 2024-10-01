using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.CardTransits;
using GameFields.Persons.Discovers;
using GameFields.Seats;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionPlayer : StartTowerCardSelection
    {
        private const float WaitDurationForEnemyFirstCardsDraw = 1f;
        private const float DrawCardsDuration = 1.5f;
        private const float DrawCardsScaleFactor = 2f;
        private const float WaitDurationBetweenDrawCards = 1f;
        private const string LabelMessage = "Выберете, какая карта будет в замке";

        private readonly Deck _deck;
        private readonly IHandTransitSet _handTransitSet;
        private readonly Discover _discover;

        private readonly Seat[] _seats;

        public StartTowerCardSelectionPlayer(Deck deck, Person person, Seat[] seats, Discover discover): base(person)
        {
            _handTransitSet = person;

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

            yield return new WaitForSeconds(WaitDurationForEnemyFirstCardsDraw);

            for (int i = 0; i < _seats.Length; i++)
            {
                Card card = _deck.TakeTopCard();
                cards.Add(card);
                _seats[i].SetCard(card, SideType.Front, DrawCardsDuration, DrawCardsScaleFactor);

                yield return new WaitForSeconds(WaitDurationBetweenDrawCards);
            }

            if (DrawCardsDuration - WaitDurationBetweenDrawCards > 0)
            {
                yield return new WaitForSeconds(DrawCardsDuration - WaitDurationBetweenDrawCards);
            }

            //foreach (Card card in cards)
            //{
            //    card.gameObject.SetActive(false);
            //}

            _discover.Activate(cards, LabelMessage, OnCardChoiceDone);
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
            yield return new WaitForSeconds(0.5f);

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
                        SeatCardInTower(card, seat).ToUniTask();
                    }
                }
            }
            else
            {
                throw new System.Exception("Что-то не так, работяги");
            }
        }

        private IEnumerator SeatCardInTower(Card card, Seat mySeat)
        {
            float invertCardFrontDuration = 0.5f;
            float invertCardBackDuration = 0.5f;
            float delayAfterInvert = 0.5f;

            yield return new WaitForSeconds(1f);

            InvertCardFront(card, invertCardFrontDuration);
            yield return new WaitForSeconds(invertCardFrontDuration);

            card.SetSide(SideType.Back);

            InvertCardBack(card, invertCardBackDuration);
            yield return new WaitForSeconds(invertCardBackDuration + delayAfterInvert);

            mySeat.Reset();
            TowerTransitSet.Set(card);
        }

        private void InvertCardFront(Card card, float invertCardFrontDuration)
        {
            Vector3 position = card.Transform.position;

            CardMovement cardMovement = card.CardMovement;

            cardMovement.MoveLinear(position, new Vector3(0f, -90f, 0f), invertCardFrontDuration);
        }

        private void InvertCardBack(Card card, float invertCardBackDuration)
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = card.Transform.position;

            CardMovement cardMovement = card.CardMovement;

            cardMovement.MoveSmoothly(position, endRotationVector, invertCardBackDuration, card.Transform.localScale);
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
