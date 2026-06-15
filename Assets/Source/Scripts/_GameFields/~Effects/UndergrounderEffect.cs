using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CardTransits;
using GameFields.InformationLabels;
using GameFields.Persons;
using GameFields.Persons.LookCardMenues;
using Tools.UI;

namespace GameFields.Effects
{
    public class UndergrounderEffect : Effect
    {
        private const string MessageEnemy = "Соперник смотрит карты в конце колоды...";
        private const string MessagePlayer = "Карты в конце колоды, начиная с нижней";
        private const int CountLookCards = 3;

        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _cardLocationViewRoot;
        private readonly InformationLabel _informationLabel;

        public UndergrounderEffect(CardLocationViewRoot cardLocationViewRoot, InformationLabel informationLabel,
            EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;
            _informationLabel = informationLabel;
            _cardLocationViewRoot = cardLocationViewRoot;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Подпольщика закончен");
        //}

        protected override async UniTask OnPlaying()
        {
            //_deactivePerson.ActivateSharpSnakeEffect(CompleteEffect);
            //yield return new WaitUntil(() => _isEffectComplete);
            if (_cardLocationViewRoot.TryViewDeckLastCards(out IReadOnlyList<Card> cards, CountLookCards) == false)
            {
                return;
            }

            if (_activePerson is Player)
            {
                bool isEffectComplete = false;
                LookCardMenuActivateData lookCardMenuActivateData = new LookCardMenuActivateData(cards, MessagePlayer);
                _activePerson.LookCards(lookCardMenuActivateData, () => isEffectComplete = true);

                await UniTask.WaitUntil(() => isEffectComplete, cancellationToken: Token);
            }
            else
            {
                LabelActivateData labelActivateData = new LabelActivateData(MessageEnemy);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 6f);

                _informationLabel.Activate(informationLabelActivateData);
                await UniTask.WaitUntil(() => _informationLabel.IsComplete, cancellationToken: Token);
            }
        }
    }
}