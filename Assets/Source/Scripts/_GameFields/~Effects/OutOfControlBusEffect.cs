using System.Collections.Generic;
using System.Linq;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.Persons;
using Tools;

namespace GameFields.Effects
{
    public class OutOfControlBusEffect : Effect
    {
        private const int CountNumbers = 3;

        private readonly Person _activePerson;
        private readonly IPersonObject _deactivePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;
        private readonly ViewTransitTypesRoot _typesRoot;

        public OutOfControlBusEffect(IPersonObject deactivePerson, CardLocationViewRoot cardLocationViewRoot,
            CardTransitManager transitManager, ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _deactivePerson = deactivePerson;
            _viewRoot = cardLocationViewRoot;
            _transitManager = transitManager;
            _typesRoot = typesRoot;

            Play();
        }

        protected override string GetName() => nameof(OutOfControlBusEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект неуправляемого автобуса закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            bool endChoice = false;

            _activePerson.ChoiceActivate(CountNumbers, () => endChoice = true);
            await UniTask.WaitUntil(() => endChoice, cancellationToken: Token);

            IEnumerable<int> newCheckedNumbers = _activePerson.LastSelectedNumbers;

            CallbackHandler fireActivePersonCallbackHandler = new CallbackHandler();
            CallbackHandler fireDeactivePersonCallbackHandler = new CallbackHandler();

            //IEnumerable<Card> handEnemyCards = _viewRoot.GetAllCards(ViewType.HandAI);
            //IEnumerable<Card> handPlayerCards = _viewRoot.GetAllCards(ViewType.HandPlayer);

            //IEnumerable<Card> firedCardsEnemy = handEnemyCards.Where(c => newCheckedNumbers.Contains(c.ViewData.Number));
            //IEnumerable<Card> firedCardsPlayer = handPlayerCards.Where(c => newCheckedNumbers.Contains(c.ViewData.Number));


            //if (firedCardsEnemy.Count() > 0)
            //{
            //    FiringCards(firedCardsEnemy, TransitFromType.HandEnemy, ViewType.HandAI, TransitToType.EnemyFirePool, fireActivePersonCallbackHandler).ToUniTask();
            //}
            //else
            //{
            //    fireActivePersonCallbackHandler.Complete();
            //}

            //if (firedCardsPlayer.Count() > 0)
            //{
            //    FiringCards(firedCardsPlayer, TransitFromType.HandPlayer, ViewType.HandPlayer, TransitToType.PlayerFirePool, fireDeactivePersonCallbackHandler).ToUniTask();
            //}
            //else
            //{
            //    fireDeactivePersonCallbackHandler.Complete();
            //}

            FirePersonsCards(_activePerson, newCheckedNumbers, fireActivePersonCallbackHandler);
            FirePersonsCards(_deactivePerson, newCheckedNumbers, fireDeactivePersonCallbackHandler);

            await UniTask.WaitUntil(() => fireActivePersonCallbackHandler.IsComplete && fireDeactivePersonCallbackHandler.IsComplete);
        }

        private void FirePersonsCards(IPersonObject person, IEnumerable<int> newCheckedNumbers, CallbackHandler fireCallbackHandler)
        {
            HandTypes personHandTypes = _typesRoot.GetPersonTypes(person).Hand;

            IEnumerable<Card> handCards = _viewRoot.GetAllCards(personHandTypes.ViewType);
            IEnumerable<Card> firedCards = handCards.Where(c => newCheckedNumbers.Contains(c.ViewData.Number));

            if (firedCards.Count() > 0)
            {
                FiringCards(firedCards, personHandTypes.FromType, personHandTypes.ViewType, fireCallbackHandler).Forget();
            }
            else
            {
                fireCallbackHandler.Complete();
            }
        }

        private async UniTask FiringCards(IEnumerable<Card> firedCards, TransitFromType handFrom, ViewType handView,
            CallbackHandler callbackHandler)
        {
            TransitToType firePoolTo = _typesRoot.GetPersonTypes(_activePerson).FirePool; // Кто сжег - того и пул
            int indexOffset = 0; // Смещение индекса для спавна пиромантов из-за сжигания других карт
            Dictionary<Card, int> firedCardIndexPair = new Dictionary<Card, int>();
            Card lastPyromancersManuscript = null;
            Card lastCard = null;

            foreach (Card firedCard in firedCards)
            {
                int index = _viewRoot.IndexOf(handView, firedCard);
                firedCardIndexPair.Add(firedCard, index);

                if (firedCard.IsPyromancersManuscript)
                {
                    lastPyromancersManuscript = firedCard;
                }

                lastCard = firedCard;
            }

            CallbackHandler lastPyromancersManuscriptFireCallback = null;
            CallbackHandler lastCardFireCallback = null;

            foreach (Card firedCard in firedCards)
            {
                int realIndex = firedCardIndexPair[firedCard] + indexOffset;

                if (lastPyromancersManuscript != null && firedCard == lastPyromancersManuscript)
                {
                    lastPyromancersManuscriptFireCallback = new CallbackHandler();
                    _transitManager.TransitCard(firedCard, handFrom, firePoolTo, () => lastPyromancersManuscriptFireCallback.Complete(), index: realIndex);
                }
                else
                {
                    if (firedCard == lastCard)
                    {
                        lastCardFireCallback = new CallbackHandler();
                        _transitManager.TransitCard(firedCard, handFrom, firePoolTo, () => lastCardFireCallback.Complete(), index: realIndex);
                    }
                    else
                    {
                        _transitManager.TransitCard(firedCard, handFrom, firePoolTo, index: realIndex);
                    }
                }

                if (firedCard.IsPyromancersManuscript)
                {
                    indexOffset += 1; // -1 за счет минус карты, +2 за счет 2 пиромантов
                }
                else
                {
                    indexOffset -= 1; // -1 за счет минус карты
                }

                await UniTask.WaitForSeconds(0.2f, cancellationToken: Token);
            }

            if (lastCardFireCallback != null)
                await UniTask.WaitUntil(() => lastCardFireCallback.IsComplete, cancellationToken: Token);

            if (lastPyromancersManuscriptFireCallback != null)
                await UniTask.WaitUntil(()=> lastPyromancersManuscriptFireCallback.IsComplete, cancellationToken: Token);

            callbackHandler.Complete();
        }
    }
}