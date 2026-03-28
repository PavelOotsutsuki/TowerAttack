using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using System.Collections.Generic;
using GameFields.InformationLabels;
using Tools.UI;
using System.Linq;
using Tools.Utils;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using System;
using GameFields.CardTransits;

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
        private readonly Person _deactivePerson;

        private readonly CardLocationViewRoot _viewRoot;
        private readonly InformationLabel _informationLabel;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        private readonly IDrawCardManager _drawCardManager;

        public DetectiveRhodesEffect(Person deactivePerson, CardTransitManager transitManager, CardLocationViewRoot viewRoot,
            InformationLabel informationLabel, ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _deactivePerson = deactivePerson;
            _transitManager = transitManager;

            _viewRoot = viewRoot;
            _informationLabel = informationLabel;
            _typesRoot = typesRoot;

            _drawCardManager = data.ActivePerson;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            //ViewType enemyhandType = _activePerson is Player ? ViewType.HandAI : ViewType.HandPlayer;
            //ViewType enemyHandType = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, false);
            HandTypes deactiveHandTypes = _typesRoot.GetPersonTypes(_deactivePerson).Hand;
            ViewType deactiveHandView = deactiveHandTypes.ViewType;

            Card deckCard = null;

            if (_viewRoot.TryView(out IReadOnlyList<Card> cardDeck, 1, ViewType.Deck))
            {
                deckCard = cardDeck[0];
            }

            Card handCard = null;

            if (_viewRoot.TryView(out IReadOnlyList<Card> cardHand, 1, deactiveHandView))
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
                    throw new Exception("Ошибка условия вывода сообщения для " + typeof(DetectiveRhodesEffect));
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
            List<ViewType> noContainsForHand = new List<ViewType>() { deactiveHandView, ViewType.TablePlayer, ViewType.TableAI };
            Discover(handCard, _countHandDiscoverCards, _activateHandDiscoverMessage, handResult, noContainsForHand);
            yield return new WaitUntil(() => handResult.Result != null);

            //bool isTest = true;

            //if ((deckResult.Result == deckCard && handResult.Result == handCard) || isTest)
            if (deckResult.Result == deckCard && handResult.Result == handCard)
            {
                //TransitFromType handFrom = _activePerson is Player ? TransitFromType.HandEnemy : TransitFromType.HandPlayer;
                //TransitToType handTo = _activePerson is Player ? TransitToType.HandPlayer : TransitToType.HandEnemy;
                TransitFromType handFrom = deactiveHandTypes.FromType;
                TransitToType handTo = _typesRoot.GetPersonTypes(_activePerson).Hand.ToType;

                bool isTransit = false;
                bool isDraw = false;

                _transitManager.TransitCard(handCard, handFrom, handTo, callback: () => isTransit = true);
                _drawCardManager.DrawCard(deckCard, callback: () => isDraw = true);

                yield return new WaitUntil(() => isTransit && isDraw);
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

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("End DetectiveRhodesEffect");
        //}

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