using System.Collections;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.LookCardMenues;
using UnityEngine;

namespace GameFields.Effects
{
    public class SharpSnakeEffect : Effect
    {
        private readonly Person _deactivePerson;
        private bool _isEffectComplete;

        private readonly LookCardMenu _lookCardMenu;
        private readonly CardLocationViewRoot _cardLocationViewRoot;

        public SharpSnakeEffect(Person deactivePerson, LookCardMenu lookCardMenu, CardLocationViewRoot cardLocationViewRoot) : base()
        {
            _deactivePerson = deactivePerson;
            _isEffectComplete = false;

            _lookCardMenu = lookCardMenu;
            _cardLocationViewRoot = cardLocationViewRoot;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Пироманта закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            //_deactivePerson.ActivateSharpSnakeEffect(CompleteEffect);
            //yield return new WaitUntil(() => _isEffectComplete);
            ViewType hand = _deactivePerson is EnemyAI ? ViewType.HandAI : ViewType.HandPlayer;
            IEnumerable<Card> cards = _cardLocationViewRoot.GetAllCards(hand);

            LookCardMenuActivateData lookCardMenuActivateData = new LookCardMenuActivateData(cards);
            _lookCardMenu.Activate(lookCardMenuActivateData);

            yield return new WaitUntil(() => _lookCardMenu.IsComplete);
        }

        private void CompleteEffect()
        {
            _isEffectComplete = true;
        }
    }
}
