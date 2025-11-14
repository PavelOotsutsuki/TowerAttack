using System.Collections;
using Cards;
using GameFields.Persons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class ImpArmyEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly Card _card;

        public ImpArmyEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        public override void End()
        {
            base.End();
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.AddCurse(_card);
            yield break;
        }
    }
}
