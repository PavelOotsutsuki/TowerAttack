
using System.Collections;
using Cards;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class VoidEffect : Effect
    {
        private readonly float _delay;

        public VoidEffect(SignalBus bus, CardEffectData data) : base(bus, data)
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