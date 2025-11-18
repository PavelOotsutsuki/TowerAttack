using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class ScarecrowEffect : Effect
    {
        private const int CountUsed = 1;

        private readonly Person _deactivePerson;
        private readonly Card _card;

        public ScarecrowEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Чучела закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.ActivateScarecrowEffect(CountUsed, _card);
            yield break;
        }
    }
}