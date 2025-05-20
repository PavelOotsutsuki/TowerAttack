using System.Collections.Generic;
using GameFields.Persons.Hands;
using Tools.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameFields.Decks
{
    public class DeckHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private FadableLabel _handPlayerHelpView;
        [SerializeField] private FadableLabel _handEnemyHelpView;
        [SerializeField] private FadableLabel _deckHelpView;

        private ICardsCounter _handPlayerCounter;
        private ICardsCounter _handEnemyCounter;
        private ICardsCounter _deckCounter;

        private Dictionary<FadableLabel, ICardsCounter> _hookups;

        [Inject]
        public void Constuct(HandPlayer handPlayer, HandAI handAI, Deck deck)
        {
            _handPlayerCounter = handPlayer;
            _handEnemyCounter = handAI;
            _deckCounter = deck;
        }

        public void Init()
        {
            _hookups = new Dictionary<FadableLabel, ICardsCounter>()
            {
                { _handPlayerHelpView, _handPlayerCounter},
                { _handEnemyHelpView, _handEnemyCounter},
                { _deckHelpView, _deckCounter}
            };

            foreach (FadableLabel helpView in _hookups.Keys)
            {
                helpView.Init();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (ICardsCounter cardsCounter in _hookups.Values)
            {
                cardsCounter.OnSeatsCountChange += Show;
            }

            Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (ICardsCounter cardsCounter in _hookups.Values)
            {
                cardsCounter.OnSeatsCountChange -= Show;
            }

            Hide();
        }

        private void Show()
        {
            foreach (KeyValuePair<FadableLabel, ICardsCounter> keyValuePair in _hookups)
            {
                LabelActivateData labelActivateData = new LabelActivateData("Карт: " + keyValuePair.Value.CountCards.ToString());
                keyValuePair.Key.Show(labelActivateData);
            }
        }

        private void Hide()
        {
            foreach (FadableLabel helpView in _hookups.Keys)
            {
                helpView.Hide();
            }
        }
    }
}