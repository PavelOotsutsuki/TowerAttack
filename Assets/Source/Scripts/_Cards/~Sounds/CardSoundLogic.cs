using UnityEngine;

namespace Cards.Sounds
{
    public abstract class CardSoundLogic : MonoBehaviour
    {
        protected AudioClip[] AudioClips;

        public void Init(AudioClip[] audioClips)
        {
            AudioClips = audioClips;
        }
    }
} 