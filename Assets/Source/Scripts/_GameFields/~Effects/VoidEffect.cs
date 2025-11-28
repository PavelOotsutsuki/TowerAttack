
using System.Collections;
using UnityEngine;

namespace GameFields.Effects
{
    public class VoidEffect : Effect
    {
        private readonly float _delay;

        public VoidEffect(EffectData data) : base(data)
        {
            _delay = 1f;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Пустой эффект закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            yield return new WaitForSeconds(_delay);
        }
    }
}