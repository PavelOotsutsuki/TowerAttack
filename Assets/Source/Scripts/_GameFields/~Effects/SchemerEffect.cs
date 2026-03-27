using System.Collections;
using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class SchemerEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly Person _activePerson;
        private readonly Card _card;

        public SchemerEffect(Person activePerson, Person deactivePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Шулера закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            //_deactivePerson.ActivateDoubleEffect(1);
            //_activePerson.ActivateDoubleEffect(2);
            _deactivePerson.ActivateDoubleEffect(_card);
            _activePerson.ActivateDoubleEffect(_card);
            yield break;
            //yield return new WaitForSeconds(10f);

            //_deactivePerson.AttackDeactivate();
        }
    }
}