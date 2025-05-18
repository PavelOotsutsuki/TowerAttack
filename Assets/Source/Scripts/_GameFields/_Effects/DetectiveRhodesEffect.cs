using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.CardTransits;
using GameFields.InformationLabels;
using Tools.UI;
using System.Linq;
using Tools.Utils;
using System;
using GameFields.Persons.Discovers;

namespace GameFields.Effects
{
    public class DetectiveRhodesEffect : Effect
    {
        private const string TrueChoice = "ВЕРНО";
        private const string FalseChoice = "НЕВЕРНО";

        private readonly int _countDeckDiscoverCards = 3;
        private readonly int _countHandDiscoverCards = 3;

        private readonly string _activateDeckDiscoverMessage = "Какая карта в колоде?";
        private readonly string _activateHandDiscoverMessage = "Какая карта в руке у противника?";

        private readonly Person _activePerson;
        //private readonly Person _deactivePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly InformationLabel _informationLabel;

        private readonly IHandTransitTryGet _handTransitTryGet;
        private readonly IHandTransitSet _handTransitSet;
        private readonly IDrawCardManager _drawCardManager;

        //private List<Card> _cards;
        //private bool _endPlaying;

        private bool _isDeckDiscoverComplete;
        private Card _deckDiscoverChoice;

        private bool _isHandDiscoverComplete;
        private Card _handDiscoverChoice;

        public DetectiveRhodesEffect(Person activePerson, Person deactivePerson, CardLocationViewRoot viewRoot,
            InformationLabel informationLabel) : base()
        {
            _activePerson = activePerson;
            //_deactivePerson = deactivePerson;
            _viewRoot = viewRoot;
            _informationLabel = informationLabel;
            //_handTransitTryGet = activePerson;
            _handTransitSet = activePerson;
            _handTransitTryGet = deactivePerson;
            _drawCardManager = activePerson;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            //_endPlaying = false;
            _isDeckDiscoverComplete = false;
            _isHandDiscoverComplete = false;

            ViewType enemyhandType = _activePerson is Player ? ViewType.HandAI : ViewType.HandPlayer;

            Card deckCard = null;

            if (_viewRoot.TryView(out IReadOnlyList<Card> cardDeck, 1, ViewType.Deck))
            {
                deckCard = cardDeck[0];
            }

            Card handCard = null;

            if (_viewRoot.TryView(out IReadOnlyList<Card> cardHand, 1, enemyhandType))
            {
                handCard = cardHand[0];
            }

            if (deckCard == null || handCard == null)
            {
                string activateMessage;

                if (deckCard == null && handCard == null)
                {
                    activateMessage = "И в колоде, и в руке противника пусто!";
                }
                else if (deckCard == null)
                {
                    activateMessage = "В колоде пусто!";
                }
                else if (handCard == null)
                {
                    activateMessage = "В руке противника пусто!";
                }
                else
                {
                    throw new System.Exception("Ошибка условия вывода сообщения для " + typeof(DetectiveRhodesEffect));
                }

                LabelActivateData labelActivateData = new LabelActivateData(activateMessage);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);
                _informationLabel.Activate(informationLabelActivateData);

                yield return new WaitUntil(() => _informationLabel.IsComplete);
                yield break;
            }

            Discover(deckCard, _countDeckDiscoverCards, _activateDeckDiscoverMessage, ContinueAfterFindDeckCard);
            yield return new WaitUntil(() => _isDeckDiscoverComplete);
            yield return new WaitForSeconds(2f);

            Discover(handCard, _countHandDiscoverCards, _activateHandDiscoverMessage, ContinueAfterFindHandCard);
            yield return new WaitUntil(() => _isHandDiscoverComplete);

            if (_deckDiscoverChoice == deckCard && _handDiscoverChoice == handCard)
            {
                if (_handTransitTryGet.TryGet(handCard))
                {
                    _handTransitSet.Set(handCard);
                }
                else
                {
                    throw new Exception("Не удалось получить карту из руки противника");
                }

                _drawCardManager.DrawCard(deckCard);
            }
            else
            {
                string activateMessage = "";

                activateMessage += "Выбор из колоды: ";
                activateMessage += _deckDiscoverChoice == deckCard ? TrueChoice : FalseChoice;
                activateMessage += "\n";

                activateMessage += "Выбор из руки: ";
                activateMessage += _handDiscoverChoice == handCard ? TrueChoice : FalseChoice;

                LabelActivateData labelActivateData = new LabelActivateData(activateMessage);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);
                _informationLabel.Activate(informationLabelActivateData);

                yield return new WaitUntil(() => _informationLabel.IsComplete);
            }
        }

        public override void End()
        {
            //Debug.Log("End patriarch corall effect");
        }

        private void Discover(Card firstFindedCard, int countDiscoverCards, string activateDiscoverMessage,
            Action<Card> continueAfterFindCard)
        {
            List<Card> cardsGuess = new List<Card>();

            cardsGuess.Add(firstFindedCard);

            for (int i = 0; i < countDiscoverCards - 1; i++)
            {
                cardsGuess.Add(_viewRoot.ViewRandomCard(cardsGuess.Select(c => c.ViewConfig.Number)));
            }

            cardsGuess = Utils.Shuffle(cardsGuess);

            _activePerson.DiscoverCards(cardsGuess, activateDiscoverMessage, continueAfterFindCard);
        }

        private void ContinueAfterFindDeckCard(Card card)
        {
            _deckDiscoverChoice = card;

            _isDeckDiscoverComplete = true;
        }

        private void ContinueAfterFindHandCard(Card card)
        {
            _handDiscoverChoice = card;

            _isHandDiscoverComplete = true;
        }
    }
}