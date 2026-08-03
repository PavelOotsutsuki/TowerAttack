using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.Sounds;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
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
        private readonly Person _deactivePerson;
        private readonly IDrawCardManager _drawCardManager;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly CardSoundRoot _cardSoundRoot;
        private readonly InformationLabel _informationLabel;
        private readonly GunnerCardSoundLogic _gunnerCardSoundLogic;
        private readonly ViewTransitTypesRoot _typesRoot;

        public GunnerEffect(Person deactivePerson, CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            CardSoundRoot cardSoundRoot, InformationLabel informationLabel, ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _deactivePerson = deactivePerson;
            _drawCardManager = data.ActivePerson;
            _viewRoot = viewRoot;
            _transitManager = transitManager;
            _cardSoundRoot = cardSoundRoot;
            _informationLabel = informationLabel;
            _typesRoot = typesRoot;

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

        protected override string GetName() => nameof(GunnerEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Артеллериста закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            //_effectProcessSounds.Play();

            await UniTask.WaitForSeconds(2f, cancellationToken: Token); // Ждем для большего ЭПИКА
            // Эффект 1. Взятие карты
            ActivateShotSound();

            bool effectOneComplete = false;

            _drawCardManager.DrawCards(1, Token, () => effectOneComplete = true);
            await UniTask.WaitUntil(() => effectOneComplete, cancellationToken: Token);
            await UniTask.WaitForSeconds(1f, cancellationToken: Token);
            // Эффект 2. Атака
            ActivateShotSound();

            bool effectTwoComplete = false;

            _activePerson.AttackActivate(1, () => effectTwoComplete = true);
            await UniTask.WaitUntil(() => effectTwoComplete, cancellationToken: Token);

            // Эффект 3. Сжигаем карту
            //ViewType viewType = _activePerson is EnemyAI ? ViewType.HandPlayer : ViewType.HandAI;
            //ViewType handViewType = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, false);
            HandTypes deactivePersonHandTypes = _typesRoot.GetPersonTypes(_deactivePerson).Hand;
            ViewType handViewType = deactivePersonHandTypes.ViewType;
            IReadOnlyList<Card> cards = _viewRoot.GetAllCards(handViewType).ToList();

            if (cards.Count > 0)
            {
                ActivateShotSound();

                bool effectThreeComplete = false;
                int randomCardIndex = Random.Range(0, cards.Count);

                TransitFromType handFrom = deactivePersonHandTypes.FromType;
                TransitToType firePoolTo = _typesRoot.GetPersonTypes(_activePerson).FirePool;

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

                _transitManager.TransitCard(cards[randomCardIndex], handFrom, firePoolTo, () => effectThreeComplete = true, index);
                await UniTask.WaitUntil(() => effectThreeComplete, cancellationToken: Token);
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
                return;

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
                await UniTask.WaitUntil(() => effectFourComplete, cancellationToken: Token);
            }
            else
            {
                LabelActivateData labelActivateData = new LabelActivateData(MessageEnemy);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 6f);

                _informationLabel.Activate(informationLabelActivateData);
                await UniTask.WaitUntil(() => _informationLabel.IsComplete, cancellationToken: Token);
            }
        }

        private void ActivateShotSound()
        {
            if (_gunnerCardSoundLogic != null)
                _cardSoundRoot.Play(_gunnerCardSoundLogic.ShotSound);
        }
    }
}