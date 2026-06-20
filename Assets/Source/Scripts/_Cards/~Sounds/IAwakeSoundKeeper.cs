using UnityEngine;

namespace Cards.Sounds
{
    public interface IAwakeSoundKeeper
    {
        public AudioClip AwakeSound { get; }
    }
}