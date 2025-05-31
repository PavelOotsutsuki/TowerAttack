using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class ImpArmyEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly EffectedCard _effectedCard;

        public ImpArmyEffect(Person deactivePerson, int countTurns) : base(countTurns)
        {
            _deactivePerson = deactivePerson;
            _effectedCard = new EffectedCard();

            Play();
        }

        public override void End()
        {
            _effectedCard.EndEffect();
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.AddCurse(_effectedCard);
            yield break;
        }
    }
}
