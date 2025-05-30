using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class TimeLordEffect : Effect
    {
        private readonly Effect _lastEffect;

        public TimeLordEffect(Person deactivePerson, IEffectFactory effectFactory) : base()
        {
            if (deactivePerson.LastEffect.Type == EffectType.TimeLord)
            {
                _lastEffect = new VoidEffect();
            }
            else
            {
                _lastEffect = effectFactory.Create(deactivePerson.LastEffect);
            }

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Жыжи закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            yield return new WaitUntil(() => _lastEffect.IsComplete);
            //yield return new WaitForSeconds(10f);

            //_deactivePerson.AttackDeactivate();
        }
    }
}