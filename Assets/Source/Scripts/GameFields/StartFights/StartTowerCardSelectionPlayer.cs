using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.CardTransits;
using GameFields.Persons.Discovers;
using GameFields.Seats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionPlayer : StartTowerCardSelection
    {
        private readonly Deck _deck;
        private readonly IHandTransitSet _handTransitSet;
        private readonly Discover _discover;

        private readonly StartTowerCardSelectionSeat[] _seats;

        public StartTowerCardSelectionPlayer(Deck deck, Person person, StartTowerCardSelectionSeat[] seats, DiscoverPlayer discoverPlayer): base(person)
        {
            _handTransitSet = person;

            _seats = seats;
            _deck = deck;
            _discover = discoverPlayer;

            InitSeats();
        }

        public override void StartProcess()
        {
            StartPlayerProcess().ToUniTask();
        }

        private IEnumerator StartPlayerProcess()
        {
            List<Card> cards = new List<Card>();

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < _seats.Length; i++)
            {
                Card card = _deck.TakeTopCard();
                cards.Add(card);
                _seats[i].SetCard(card, 1.5f, 2f);
                yield return new WaitForSeconds(1f);
                //card.StartSelection();

            }

            yield return new WaitForSeconds(2f);

            //foreach (StartTowerCardSelectionSeat seat in _seats)
            //{
            //    seat.Card.StartSelection();
            //}
            _discover.Activate(cards, "Выберете, какая карта будет в замке", End);


            //int selectedCardIndex = Random.Range(0, _seats.Length);

            //if (TowerTransitCheck.IsFill == false)
            //{
            //    TowerTransitSet.Set(_seats[selectedCardIndex].Card);
            //    _seats[selectedCardIndex].Reset();

            //    for (int i = 0; i < _seats.Length; i++)
            //    {
            //        if (i == selectedCardIndex)
            //            continue;

            //        _handTransitSet.Set(_seats[i].Card);
            //        _seats[i].Reset();
            //    }
            //}
            //else
            //{
            //    throw new System.Exception("Что-то не так, работяги");
            //}

        }

        private void End(Card card)
        {
            if (TowerTransitCheck.IsFill == false)
            {
                TowerTransitSet.Set(card);

                foreach (StartTowerCardSelectionSeat seat in _seats)
                {
                    if (seat.Card != card)
                    {
                        _handTransitSet.Set(seat.Card);
                    }

                    seat.Reset();
                }
            }
            else
            {
                throw new System.Exception("Что-то не так, работяги");
            }
        }

        private void InitSeats()
        {
            foreach (StartTowerCardSelectionSeat seat in _seats)
            {
                seat.Init();
            }
        }
    }
}
