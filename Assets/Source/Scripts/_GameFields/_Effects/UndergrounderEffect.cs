using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.LookCardMenues;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class UndergrounderEffect : Effect
    {
        private const string Message = "Соперник смотрит карты в конце колоды...";
        private const int CountLookCards = 3;

        private readonly Person _activePerson;
        private bool _isEffectComplete;

        private readonly CardLocationViewRoot _cardLocationViewRoot;

        public UndergrounderEffect(Person activePerson, CardLocationViewRoot cardLocationViewRoot, SignalBus bus, CardEffectData data)
            : base(bus, data)
        {
            _activePerson = activePerson;
            _isEffectComplete = false;

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
                CompleteEffect();
                yield break;
            }

            LookCardMenuActivateData lookCardMenuActivateData = new LookCardMenuActivateData(cards, Message);
            _activePerson.LookCards(lookCardMenuActivateData, CompleteEffect);

            yield return new WaitUntil(() => _isEffectComplete);
        }

        private void CompleteEffect()
        {
            _isEffectComplete = true;
        }
    }
}

