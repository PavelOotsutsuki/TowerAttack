using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Cards
{
    public abstract class Effect
    {
        private readonly float _endEffectDelay = 1f;

        public abstract void End();

        public bool IsPlayed { get; private set; }

        public Effect()
        {
            IsPlayed = false;
        }

        protected void Play()
        {
            Playing().ToUniTask();
        }

        protected abstract IEnumerator OnPlaying();

        private IEnumerator Playing()
        {
            yield return OnPlaying();

            yield return new WaitForSeconds(_endEffectDelay);

            IsPlayed = true;
        }
    }
}