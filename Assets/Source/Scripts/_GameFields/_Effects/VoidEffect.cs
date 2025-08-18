
using System.Collections;
using Cards;
using UnityEngine;

namespace GameFields.Effects
{
    public class VoidEffect : Effect
    {
        private readonly float _delay;

        public VoidEffect() : base()
        {
            _delay = 1f;

            Play();
        }

        public override void End()
        {
            Debug.Log("Пустой эффект закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            yield return new WaitForSeconds(_delay);
        }
    }
}