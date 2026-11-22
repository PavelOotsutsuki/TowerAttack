using Tools;
using UnityEngine;

namespace Cards.Sounds
{
    public interface ICardSoundConfig : IData
    {
        public AudioClip[] Sounds { get; }
    }
}