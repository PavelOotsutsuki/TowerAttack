using System.Collections;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace Cards
{
    public abstract class Effect: ICompletable
    {
        private readonly float _endEffectDelay = 1f;

        public Effect()
        {
            IsComplete = false;
        }

        public bool IsComplete { get; private set; }

        public abstract void End();

        protected void Play()
        {
            Playing().ToUniTask();
        }

        protected abstract IEnumerator OnPlaying();

        private IEnumerator Playing()
        {
            yield return OnPlaying();

            yield return new WaitForSeconds(_endEffectDelay);

            IsComplete = true;
        }
    }
}