using System.Collections;
using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Seats;
using Cards;
using GameFields.Persons;
using System;

namespace GameFields.FirstTurns
{
    public class FirstTurn : MonoBehaviour
    {
        [SerializeField] private FirstTurnPanel _firstTurnPanel;
        [SerializeField] private FirstTurnLabel _firstTurnLabel;
        [SerializeField] private Seat[] _seats;
        [SerializeField] private int _firstTurnCardsCount = 3;

        public void Init()
        {
            _firstTurnPanel.Init();
            _firstTurnLabel.Init();

            InitSeats();
        }

        public void Activate(IDrawCardManager player, IDrawCardManager enemy, Deck deck)
        {
            if (deck.IsHasCards(_firstTurnCardsCount * 2) == false)
            {
                throw new ArgumentOutOfRangeException("Недостаточно карт в колоде");
            }

            gameObject.SetActive(true);

            _firstTurnPanel.Activate();
            _firstTurnLabel.Activate();

            TakeCards(deck, player).ToUniTask();
            TakeCards(deck, enemy).ToUniTask();
        }

        public void Deactivate()
        {
            _firstTurnPanel.Deactivate(()=> Destroy(gameObject));
        }

        private IEnumerator TakeCards(Deck deck, IDrawCardManager person)
        {
            Card takenCard;

            for (int i = 0; i < _firstTurnCardsCount; i++)
            {
                takenCard = deck.TakeTopCard();
                person.DrawCard(takenCard);
                yield return new WaitForSeconds(person.DrawCardsDelay);
            }
        }

        private void InitSeats()
        {
            foreach (Seat seat in _seats)
            {
                seat.Init();
            }
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFirstTurnPanel();
            DefineFirstTurnLabel();
            DefineSeats();
        }

        [ContextMenu(nameof(DefineFirstTurnPanel))]
        private void DefineFirstTurnPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _firstTurnPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFirstTurnLabel))]
        private void DefineFirstTurnLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _firstTurnLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSeats))]
        private void DefineSeats()
        {
            AutomaticFillComponents.DefineComponent(this, ref _seats);
        }
    }
}
