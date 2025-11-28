using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using System.Collections.Generic;
using GameFields.InformationLabels;
using Tools.UI;
using Tools.Utils;
using System;
using GameFields.Persons.Discovers;

namespace GameFields.Effects
{
    public class MimeEffect : Effect
    {
        private const string TrueChoice = "ВЕРНО";
        private const string FalseChoice = "НЕВЕРНО";

        private readonly string _activateDeckDiscoverMessage = "Какая карта верхняя в колоде?";

        private readonly Person _activePerson;
        private readonly Person _deactivePerson;

        private readonly CardLocationViewRoot _viewRoot;
        private readonly InformationLabel _informationLabel;

        private readonly Card _card;
        private readonly EffectDuration _effectDuration;

        public MimeEffect(Person activePerson, Person deactivePerson, CardLocationViewRoot viewRoot,
            InformationLabel informationLabel, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;

            _viewRoot = viewRoot;
            _informationLabel = informationLabel;

            _card = data.CardEffectData.Card;
            _effectDuration = data.EffectDuration;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            string activateMessage;
            LabelActivateData labelActivateData;
            InformationLabelActivateData informationLabelActivateData;

            //ViewType enemyhandType = _activePerson is Player ? ViewType.HandAI : ViewType.HandPlayer;
            ViewType enemyhandType = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, false);

            //Card deckCard = null;

            //if (_viewRoot.TryView(out IReadOnlyList<Card> cardDeck, 1, ViewType.Deck))
            //{
            //    deckCard = cardDeck[0];
            //}

            Card handCard = null;

            if (_viewRoot.TryView(out IReadOnlyList<Card> cardHand, 1, enemyhandType))
            {
                handCard = cardHand[0];
            }

            Card deckTopCard = null;

            if (_viewRoot.TryViewDeckTopCards(out IReadOnlyList<Card> cardTopDeck, 1))
            {
                deckTopCard = cardTopDeck[0];
            }

            Card deckEndCard = null;

            if (_viewRoot.TryViewDeckLastCards(out IReadOnlyList<Card> cardEndDeck, 1))
            {
                deckEndCard = cardEndDeck[0];
            }

            if (deckEndCard == null || handCard == null || deckTopCard == deckEndCard)
            {
                //string activateMessage;

                if (deckEndCard == null && handCard == null)
                {
                    activateMessage = "И в колоде, и в руке противника пусто!";
                }
                else if (deckEndCard == null)
                {
                    activateMessage = "В колоде пусто!";
                }
                else if (deckTopCard == deckEndCard && handCard == null)
                {
                    activateMessage = "В колоде всего одна карта, а в руке карт нет вообще!";
                }
                else if (deckTopCard == deckEndCard)
                {
                    activateMessage = "В колоде всего одна карта!";
                }
                else if (handCard == null)
                {
                    activateMessage = "В руке противника пусто!";
                }
                else
                {
                    throw new Exception("Ошибка условия вывода сообщения для " + typeof(MimeEffect));
                }

                labelActivateData = new LabelActivateData(activateMessage);
                informationLabelActivateData = new InformationLabelActivateData(labelActivateData);
                _informationLabel.Activate(informationLabelActivateData);

                yield return new WaitUntil(() => _informationLabel.IsComplete);
                yield break;
            }

            DiscoverResult discoverResult = new DiscoverResult();

            List<Card> discoverCards = new List<Card>()
            {
                handCard,
                deckTopCard,
                deckEndCard
            };

            discoverCards = Utils.Shuffle(discoverCards);

            _activePerson.DiscoverCards(discoverCards, _activateDeckDiscoverMessage, discoverResult);

            yield return new WaitUntil(() => discoverResult.IsComplete);

            if (discoverResult.Result == deckTopCard)
            {
                _deactivePerson.ActivateSkipTurns(_card);
                _effectDuration.SetDuration(2);
                activateMessage = TrueChoice;
            }
            else
            {
                activateMessage = FalseChoice;
            }

            labelActivateData = new LabelActivateData(activateMessage);
            informationLabelActivateData = new InformationLabelActivateData(labelActivateData);

            yield return new WaitUntil(() => discoverResult.IsComplete);
            _informationLabel.Activate(informationLabelActivateData);

            yield return new WaitUntil(() => _informationLabel.IsComplete);
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Мима окончен");
        }
    }
}