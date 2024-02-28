using System.Collections;
using System.Collections.Generic;
using GameFields.DiscardPiles;
using GameFields.EndTurnButtons;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using GameFields.Seats;
using Cards;
using GameFields.Persons;

namespace GameFields.FirstTurns
{
    public class FirstTurn : MonoBehaviour//, IPlayerFirstTurnManager, IEnemyFirstTurnManager
    {
        [SerializeField] private FirstTurnPanel _firstTurnPanel;
        [SerializeField] private FirstTurnLabel _firstTurnLabel;
        [SerializeField] private Seat[] _seats;

        public void Init()
        {
            _firstTurnPanel.Init();
            _firstTurnLabel.Init();

            InitSeats();
        }

        public void Activate(Card[] playerCards, Card[] enemyCards, IPerson player, IPerson enemy)
        {
            //float delay;

            gameObject.SetActive(true);

            //yield return _firstTurnPanel.Activate();
            _firstTurnPanel.Activate();
            _firstTurnLabel.Activate();

            //for (int i=0;i<cards.Length;i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        player.DrawCard(cards[i]);
            //        delay = player.DrawCardsDelay - enemy.DrawCardsDelay;
            //    }
            //    else
            //    {
            //        enemy.DrawCard(cards[i]);
            //        delay = enemy.DrawCardsDelay - enemy.DrawCardsDelay;
            //    }

            //    yield return new WaitForSeconds(delay);
            //}

            TakeCards(playerCards, player).ToUniTask();
            TakeCards(enemyCards, enemy).ToUniTask();
        }

        public void Deactivate()
        {
            //yield return _firstTurnPanel.Deactivate();
            _firstTurnPanel.Deactivate(()=> Destroy(gameObject));

            //gameObject.SetActive(false);
        }

        private IEnumerator TakeCards(Card[] cards, IPerson person)
        {
            foreach (Card card in cards)
            {
                person.DrawCard(card);
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
