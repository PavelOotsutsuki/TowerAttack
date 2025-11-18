using System.Collections;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class DeadOgreEffect : Effect
    {
        private const int CountTurns = 3;

        private readonly Person _deactivePerson;

        public DeadOgreEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Мертвого огра закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.AddCurse(CountTurns);
            yield break;
        }
    }
}