using UnityEngine;

namespace Cards.Sounds
{
    public class DefaultCardSoundLogic : CardSoundLogic, IAwakeSoundKeeper
    {
        public AudioClip AwakeSound => AudioClips[0];
    }
}