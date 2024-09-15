using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.CardTransits;
using GameFields.Persons.Towers;
using GameFields.Seats;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.StartTowerCardSelections
{
    public class StartTowerCardSelectionPlayer
    {
        private Deck _deck;
        //private Tower _tower;
        private readonly ITowerTransitCheck _towerTransitCheck;
        private readonly ITowerTransitTrySet _towerTransitTrySet;
        private readonly IHandTransitSet _handTransitSet;

        private readonly Seat[] _seats;

        public bool IsComplete => _towerTransitCheck.IsFill;

        public StartTowerCardSelectionPlayer(Deck deck, Person person, Seat[] seats)
        {
            _towerTransitCheck = person;
            _towerTransitTrySet = person;
            _handTransitSet = person;

            _seats = seats;
            _deck = deck;

            InitSeats();
        }

        //public void Init(Person person, Seat[] seats)
        //{
        //    _person = person;
        //    _seats = seats;

        //    InitSeats();
        //}

        public void StartProcess()
        {
            StartPlayerProcess().ToUniTask();
        }

        private IEnumerator StartPlayerProcess()
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < _seats.Length; i++)
            {
                _seats[i].SetCard(_deck.TakeTopCard(), SideType.Front, 1.5f, 2f);
                yield return new WaitForSeconds(1f);
                //card.StartSelection();

            }

            yield return new WaitForSeconds(3f);

            int selectedCardIndex = Random.Range(0, _seats.Length);

            if (_towerTransitTrySet.TrySet(_seats[selectedCardIndex].Card))
            {
                _seats[selectedCardIndex].Reset();

                for (int i = 0; i < _seats.Length; i++)
                {
                    if (i == selectedCardIndex)
                        continue;

                    _handTransitSet.Set(_seats[i].Card);
                    _seats[i].Reset();
                }
            }
            else
            {
                throw new System.Exception("Что-то не так, работяги");
            }

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
