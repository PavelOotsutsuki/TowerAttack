using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.CardTransits;
using GameFields.InformationLabels;
using GameFields.Persons;
using GameFields.Persons.LookCardMenues;
using Tools.UI;
using UnityEngine;

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

        public UndergrounderEffect(Person activePerson, CardLocationViewRoot cardLocationViewRoot,
            InformationLabel informationLabel, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _informationLabel = informationLabel;
            _cardLocationViewRoot = cardLocationViewRoot;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Подпольщика закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            //_deactivePerson.ActivateSharpSnakeEffect(CompleteEffect);
            //yield return new WaitUntil(() => _isEffectComplete);
            if (_cardLocationViewRoot.TryViewDeckLastCards(out IReadOnlyList<Card> cards, CountLookCards) == false)
            {
                yield break;
            }

            if (_activePerson is Player)
            {
                bool isEffectComplete = false;
                LookCardMenuActivateData lookCardMenuActivateData = new LookCardMenuActivateData(cards, MessagePlayer);
                _activePerson.LookCards(lookCardMenuActivateData, () => isEffectComplete = true);
                yield return new WaitUntil(() => isEffectComplete);
            }
            else
            {
                LabelActivateData labelActivateData = new LabelActivateData(MessageEnemy);
                InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 6f);

                _informationLabel.Activate(informationLabelActivateData);
                yield return new WaitUntil(() => _informationLabel.IsComplete);
            }
        }
    }
}