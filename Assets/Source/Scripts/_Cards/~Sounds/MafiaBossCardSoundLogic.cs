using UnityEngine;

namespace Cards.Sounds
{
    public class MafiaBossCardSoundLogic : CardSoundLogic
    {
        public AudioClip DefaultAwakeSound => AudioClips[0];
        public AudioClip OneCardAwakeSound => AudioClips[1];
        public AudioClip ZeroCardAwakeSound => AudioClips[2];
    }
}