using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class ImpArmyEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly EffectedCard _effectedCard;

        public ImpArmyEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;
            _effectedCard = new EffectedCard();

            Play();
        }

        public override void End()
        {
            base.End();

            _effectedCard.EndEffect();
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.AddCurse(_effectedCard);
            yield break;
        }
    }
}
