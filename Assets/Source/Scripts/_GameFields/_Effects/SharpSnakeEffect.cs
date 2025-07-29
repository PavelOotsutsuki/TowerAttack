using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.LookCardMenues;
using UnityEngine;

namespace GameFields.Effects
{
    public class SharpSnakeEffect : Effect
    {
        private const string Message = "Соперник смотрит ваши карты...";

        private readonly Person _activePerson;
        private bool _isEffectComplete;

        private readonly CardLocationViewRoot _cardLocationViewRoot;

        public SharpSnakeEffect(Person activePerson, CardLocationViewRoot cardLocationViewRoot) : base()
        {
            _activePerson = activePerson;
            _isEffectComplete = false;

            _cardLocationViewRoot = cardLocationViewRoot;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Зоркой змеи закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            //_deactivePerson.ActivateSharpSnakeEffect(CompleteEffect);
            //yield return new WaitUntil(() => _isEffectComplete);
            ViewType hand = _activePerson is Player ? ViewType.HandAI : ViewType.HandPlayer;
            IEnumerable<Card> cards = _cardLocationViewRoot.GetAllCards(hand);

            if (cards.Count() == 0)
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
