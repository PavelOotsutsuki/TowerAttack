using UnityEngine;

namespace Cards.Sounds
{
    public class PyromancerCardSoundLogic : DefaultCardSoundLogic, IFireSoundKeeper
    {
        public AudioClip FireSound => AudioClips[1];
    }
}