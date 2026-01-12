using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using System.Collections.Generic;
using GameFields.InformationLabels;
using Tools.UI;
using System.Linq;
using Tools.Utils;
using System;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using Zenject;

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

        private readonly CardLocationViewRoot _viewRoot;
        private readonly InformationLabel _informationLabel;
        private readonly CardTransitManager _transitManager;

        private readonly IDrawCardManager _drawCardManager;

        public DetectiveRhodesEffect(Person activePerson, CardTransitManager transitManager, CardLocationViewRoot viewRoot,
            InformationLabel informationLabel, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _transitManager = transitManager;

            _viewRoot = viewRoot;
            _informationLabel = informationLabel;

            _drawCardManager = activePerson;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
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

            DiscoverResult deckResult = new DiscoverResult();
            List<ViewType> noContainsForDeck = new List<ViewType>() { ViewType.Deck, ViewType.TablePlayer, ViewType.TableAI };
            Discover(deckCard, _countDeckDiscoverCards, _activateDeckDiscoverMessage, deckResult, noContainsForDeck);
            yield return new WaitUntil(() => deckResult.IsComplete);

            DiscoverResult handResult = new DiscoverResult();
            List<ViewType> noContainsForHand = new List<ViewType>() { enemyhandType, ViewType.TablePlayer, ViewType.TableAI };
            Discover(handCard, _countHandDiscoverCards, _activateHandDiscoverMessage, handResult, noContainsForHand);
            yield return new WaitUntil(() => handResult.Result != null);

            //bool isTest = true;

            //if ((deckResult.Result == deckCard && handResult.Result == handCard) || isTest)
            if (deckResult.Result == deckCard && handResult.Result == handCard)
            {
                TransitFromType handFrom = _activePerson is Player ? TransitFromType.HandEnemy : TransitFromType.HandPlayer;
                TransitToType handTo = _activePerson is Player ? TransitToType.HandPlayer : TransitToType.HandEnemy;

                _transitManager.TransitCard(handCard, handFrom, handTo);
                _drawCardManager.DrawCard(deckCard);
            }
            else
            {
                string activateMessage = "";

                activateMessage += "Выбор из колоды: ";
                activateMessage += deckResult.Result == deckCard ? TrueChoice : FalseChoice;
                activateMessage += "\n";

                activateMessage += "Выбор из руки: ";
                activateMessage += handResult.Result == handCard ? TrueChoice : FalseChoice;

                LabelActivateData labelActivateData = new LabelActivateData(activateMessage);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);

                yield return new WaitUntil(() => handResult.IsComplete);
                _informationLabel.Activate(informationLabelActivateData);

                yield return new WaitUntil(() => _informationLabel.IsComplete);
            }
        }

        public override void End()
        {
            base.End();

            Debug.Log("End DetectiveRhodesEffect");
        }

        private void Discover(Card firstFindedCard, int countDiscoverCards, string activateDiscoverMessage,
            DiscoverResult discoverResult, IEnumerable<ViewType> noContains)
        {
            List<Card> cardsGuess = new List<Card>();

            cardsGuess.Add(firstFindedCard);

            for (int i = 0; i < countDiscoverCards - 1; i++)
            {
                cardsGuess.Add(_viewRoot.ViewRandomCardFromAllCards(cardsGuess.Select(c => c.ViewData.Number), noContains));
            }

            cardsGuess = Utils.Shuffle(cardsGuess);

            _activePerson.DiscoverCards(cardsGuess, activateDiscoverMessage, discoverResult);
        }
    }
}