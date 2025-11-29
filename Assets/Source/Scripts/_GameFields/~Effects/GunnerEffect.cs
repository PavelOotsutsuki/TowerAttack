using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Sounds;
using GameFields.InformationLabels;
using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.LookCardMenues;
using Tools.UI;
using UnityEngine;

namespace GameFields.Effects
{
    public class GunnerEffect : Effect
    {
        private const string MessagePlayer = "Верхняя и нижняя карта колоды";
        private const string MessageEnemy = "Противник смотрит нижнюю и верхнюю карту колоды";

        private readonly Person _activePerson;
        private readonly IDrawCardManager _drawCardManager;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly CardSoundRoot _cardSoundRoot;
        private readonly InformationLabel _informationLabel;
        private readonly GunnerCardSoundLogic _gunnerCardSoundLogic;

        public GunnerEffect(Person activePerson, CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            CardSoundRoot cardSoundRoot, InformationLabel informationLabel, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _drawCardManager = activePerson;
            _viewRoot = viewRoot;
            _transitManager = transitManager;
            _cardSoundRoot = cardSoundRoot;
            _informationLabel = informationLabel;

            if (data.CardEffectData.CardSoundLogic is GunnerCardSoundLogic) // Потому что есть Повторитель, и он уже это не воспроизведет
            {
                _gunnerCardSoundLogic = data.CardEffectData.CardSoundLogic as GunnerCardSoundLogic;
            }
            else
            {
                _gunnerCardSoundLogic = null;
            }

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Артеллериста закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            //_effectProcessSounds.Play();

            yield return new WaitForSeconds(2f); // Ждем для большего ЭПИКА
            // Эффект 1. Взятие карты
            ActivateShotSound();

            bool effectOneComplete = false;

            _drawCardManager.DrawCards(1, () => effectOneComplete = true);
            yield return new WaitUntil(() => effectOneComplete);
            yield return new WaitForSeconds(1f);
            // Эффект 2. Атака
            ActivateShotSound();

            bool effectTwoComplete = false;

            _activePerson.AttackActivate(1, () => effectTwoComplete = true);
            yield return new WaitUntil(() => effectTwoComplete);

            // Эффект 3. Сжигаем карту
            //ViewType viewType = _activePerson is EnemyAI ? ViewType.HandPlayer : ViewType.HandAI;
            ViewType handViewType = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, false);
            IReadOnlyList<Card> cards = _viewRoot.GetAllCards(handViewType).ToList();

            if (cards.Count > 0)
            {
                ActivateShotSound();

                bool effectThreeComplete = false;
                int randomCardIndex = Random.Range(0, cards.Count);

                TransitFromType transitFromType = ViewTransitTypeConverter.ConvertToTransitFromType(handViewType);
                TransitToType transitToType = ViewTransitTypeConverter.GetPersonFirePoolTransitToType(_activePerson, true);

                //if (viewType == ViewType.HandPlayer)
                //{
                //    transitFromType = TransitFromType.HandPlayer;
                //    transitToType = TransitToType.PlayerFirePool;
                //}
                //else
                //{
                //    transitFromType = TransitFromType.HandEnemy;
                //    transitToType = TransitToType.EnemyFirePool;
                //}

                Card firedCard = cards[randomCardIndex];
                int index = _viewRoot.IndexOf(handViewType, firedCard);

                _transitManager.TransitCard(cards[randomCardIndex], transitFromType, transitToType, () => effectThreeComplete = true, index);
                yield return new WaitUntil(() => effectThreeComplete);
            }

            // Эффект 4. Смотрим верхнюю и нижнюю карту

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

            if (deckTopCard == null || deckEndCard == null)
                yield break;

            ActivateShotSound();

            if (_activePerson is Player)
            {
                bool effectFourComplete = false;
                List<Card> lookCards = new List<Card>();

                if (deckTopCard == deckEndCard)
                {
                    lookCards.Add(deckTopCard);
                }
                else
                {
                    lookCards.Add(deckTopCard);
                    lookCards.Add(deckEndCard);
                }

                LookCardMenuActivateData lookCardMenuActivateData = new LookCardMenuActivateData(lookCards, MessagePlayer);
                _activePerson.LookCards(lookCardMenuActivateData, () => effectFourComplete = true);
                yield return new WaitUntil(() => effectFourComplete);
            }
            else
            {
                LabelActivateData labelActivateData = new LabelActivateData(MessageEnemy);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 6f);

                _informationLabel.Activate(informationLabelActivateData);
                yield return new WaitUntil(() => _informationLabel.IsComplete);
            }
        }

        private void ActivateShotSound()
        {
            if (_gunnerCardSoundLogic != null)
                _cardSoundRoot.Play(_gunnerCardSoundLogic.ShotSound);
        }
    }
}