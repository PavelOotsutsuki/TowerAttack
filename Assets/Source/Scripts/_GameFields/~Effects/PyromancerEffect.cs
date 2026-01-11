using System.Collections;
using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class PyromancerEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly Card _card;

        public PyromancerEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Пироманта закончен");
        //}

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.ActivateFireDraw(_card);
            yield break;
        }
    }
}