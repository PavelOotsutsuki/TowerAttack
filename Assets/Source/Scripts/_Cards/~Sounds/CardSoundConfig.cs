using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards.Sounds
{
    [CreateAssetMenu(fileName = "CardSoundConfig", menuName = "Cards/CardSoundConfig", order = 51)]
    internal class  CardSoundConfig : ScriptableObject, IData, ICardSoundConfig
    {
        [FormerlySerializedAs("<Sounds>k__BackingField")]
        [SerializeField] private AudioClip[] _sounds;

        public AudioClip[] Sounds => _sounds;
    }
}