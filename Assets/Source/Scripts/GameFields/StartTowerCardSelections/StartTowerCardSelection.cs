using System.Collections;
using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Seats;
using Cards;
using GameFields.Persons;
using System;

namespace GameFields.StartTowerCardSelections
{
    public class StartTowerCardSelection : MonoBehaviour
    {
        [SerializeField] private StartTowerCardSelectionPanel _startTowerCardSelectionPanel;
        [SerializeField] private StartTowerCardSelectionLabel _startTowerCardSelectionLabel;
        [SerializeField] private Seat[] _seats;
        [SerializeField] private int _firstTurnCardsCount = 3;

        public void Init()
        {
            _startTowerCardSelectionPanel.Init();
            _startTowerCardSelectionLabel.Init();

            InitSeats();
        }

        public void Activate(IDrawCardManager player, IDrawCardManager enemy, Deck deck)
        {
            if (deck.IsHasCards(_firstTurnCardsCount * 2) == false)
            {
                throw new ArgumentOutOfRangeException("Недостаточно карт в колоде");
            }

            gameObject.SetActive(true);

            _startTowerCardSelectionPanel.Activate();
            _startTowerCardSelectionLabel.Activate();

            TakeCards(deck, player).ToUniTask();
            TakeCards(deck, enemy).ToUniTask();
        }

        public void Deactivate()
        {
            _startTowerCardSelectionPanel.Deactivate(()=> Destroy(gameObject));
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
            AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelectionPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFirstTurnLabel))]
        private void DefineFirstTurnLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelectionLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSeats))]
        private void DefineSeats()
        {
            AutomaticFillComponents.DefineComponent(this, ref _seats);
        }
    }
}
