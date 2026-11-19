using Tools;
using UnityEngine;

namespace Cards.Sounds
{
    [CreateAssetMenu(fileName = "CardSoundConfig", menuName = "Cards/CardSoundConfig", order = 51)]
    internal class CardSoundConfig : ScriptableObject, IData
    {
        [field: SerializeField] internal AudioClip[] Sounds { get; private set; }
    }
}