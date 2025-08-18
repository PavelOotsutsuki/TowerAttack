using System.Collections;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Settings;
using UnityEngine;

namespace Cards
{
    public abstract class Effect: ICompletable
    {
        //private readonly float _endEffectDelay = 1f;
        private readonly int _duration;

        public Effect(int duration = 0)
        {
            IsComplete = false;
            _duration = duration;
        }

        public int Duration => _duration;
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

            yield return new WaitForSeconds(GameSettings.DefaultEffectDelayBeforeComplete);

            IsComplete = true;
        }
    }
}