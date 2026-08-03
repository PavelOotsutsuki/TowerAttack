using System.Collections.Generic;
using System.Linq;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.InformationLabels;
using GameFields.Persons;
using GameFields.Persons.LookCardMenues;
using Tools.UI;

namespace GameFields.Effects
{
    public class SharpSnakeEffect : Effect
    {
        private const string EnemyMessage = "Соперник смотрит ваши карты...";
        private const string PlayerMessage = "Карты противника";

        private readonly Person _activePerson;
        private readonly Person _deactivePerson;
        private readonly CardLocationViewRoot _cardLocationViewRoot;
        private readonly InformationLabel _informationLabel;
        private readonly ViewTransitTypesRoot _typesRoot;

        public SharpSnakeEffect(Person deactivePerson, CardLocationViewRoot cardLocationViewRoot,
            InformationLabel informationLabel, ViewTransitTypesRoot typesRoot, EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _deactivePerson = deactivePerson;

            _cardLocationViewRoot = cardLocationViewRoot;
            _informationLabel = informationLabel;
            _typesRoot = typesRoot;

            Play();
        }

        protected override string GetName() => nameof(SharpSnakeEffect);


        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Зоркой змеи закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            //_deactivePerson.ActivateSharpSnakeEffect(CompleteEffect);
            //yield return new WaitUntil(() => _isEffectComplete);
            //ViewType hand = _activePerson is Player ? ViewType.HandAI : ViewType.HandPlayer;
            //ViewType hand = ViewTransitTypeConverter.GetPersonHandViewType(_activePerson, false);
            ViewType handView = _typesRoot.GetPersonTypes(_deactivePerson).Hand.ViewType;

            IEnumerable<Card> cards = _cardLocationViewRoot.GetAllCards(handView);

            if (cards.Count() == 0)
                return;

            bool isEffectComplete = false;

            if (_activePerson is Player)
            {
                LookCardMenuActivateData lookCardMenuActivateData = new LookCardMenuActivateData(cards, PlayerMessage);
                _activePerson.LookCards(lookCardMenuActivateData, () => isEffectComplete = true);
                await UniTask.WaitUntil(() => isEffectComplete, cancellationToken: Token);
            }
            else
            {
                LabelActivateData labelActivateData = new LabelActivateData(EnemyMessage);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 8f);

                _informationLabel.Activate(informationLabelActivateData);
                await UniTask.WaitUntil(() => _informationLabel.IsComplete, cancellationToken: Token);
            }
        }
    }
}