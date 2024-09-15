using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.CardTransits;
using GameFields.Seats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionPlayer : StartTowerCardSelection
    {
        private Deck _deck;
        private readonly IHandTransitSet _handTransitSet;

        private readonly Seat[] _seats;

        public StartTowerCardSelectionPlayer(Deck deck, Person person, Seat[] seats): base(person)
        {
            _handTransitSet = person;

            _seats = seats;
            _deck = deck;

            InitSeats();
        }

        public override void StartProcess()
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

            if (TowerTransitTrySet.TrySet(_seats[selectedCardIndex].Card))
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
