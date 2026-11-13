using System.Collections.Generic;
using System.Threading;
using GameFields.Persons.Hands;
using Tools;
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

        private CancellationToken _fightToken;

        private Dictionary<FadableLabel, ICardsCounter> _hookups;

        [Inject]
        public void Constuct(HandPlayer handPlayer, HandAI handAI, Deck deck)
        {
            _handPlayerCounter = handPlayer;
            _handEnemyCounter = handAI;
            _deckCounter = deck;
        }

        public void Init(CancellationToken fightToken)
        {
            _fightToken = fightToken;

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

            foreach (ICardsCounter cardsCounter in _hookups.Values)
            {
                cardsCounter.OnSeatsCountChange += SetText;
            }

        }

        public void OnDestroy()
        {
            foreach (ICardsCounter cardsCounter in _hookups.Values)
            {
                if (cardsCounter != null)
                    cardsCounter.OnSeatsCountChange -= SetText;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter: DeckHelper");

            //foreach (ICardsCounter cardsCounter in _hookups.Values)
            //{
            //    cardsCounter.OnSeatsCountChange += Show;
            //}

            Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("OnPointerExit: DeckHelper");

            //foreach (ICardsCounter cardsCounter in _hookups.Values)
            //{
            //    cardsCounter.OnSeatsCountChange -= Show;
            //}

            Hide();
        }

        private void SetText()
        {
            foreach (KeyValuePair<FadableLabel, ICardsCounter> keyValuePair in _hookups)
            {
                keyValuePair.Key.SetText("Карт: " + keyValuePair.Value.CountCards.ToString());
            }
        }

        private void Show()
        {
            foreach (KeyValuePair<FadableLabel, ICardsCounter> keyValuePair in _hookups)
            {
                LabelActivateDataAsync labelActivateData = new LabelActivateDataAsync(new LabelActivateData("Карт: " + keyValuePair.Value.CountCards.ToString()), _fightToken);
                keyValuePair.Key.Show(labelActivateData);
            }
        }

        private void Hide()
        {
            foreach (FadableLabel helpView in _hookups.Keys)
            {
                helpView.Hide(new CancellationTokenData(_fightToken));
            }
        }
    }
}