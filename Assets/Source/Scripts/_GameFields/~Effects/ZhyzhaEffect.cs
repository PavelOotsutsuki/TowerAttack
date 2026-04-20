using System.Collections;
using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly Card _card;

        public ZhyzhaEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Жыжи закончен");
        //}

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.ActivateSlimeEffect(_card);
            yield break;
        }
    }
}