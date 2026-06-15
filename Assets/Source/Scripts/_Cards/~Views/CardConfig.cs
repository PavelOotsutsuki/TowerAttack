using Cards.Effects;
using Cards.Insides;
using Cards.Sounds;
using Tools;
using UnityEngine;

namespace Cards.Views
{
    [CreateAssetMenu(fileName = "CardConfig", menuName = "Cards/CardConfig", order = 51)]
    internal class CardConfig : ScriptableObject, IData
    {
        [field: SerializeField] internal CardViewConfig CardViewConfig { get; private set; }
        [field: SerializeField] internal CardCharacter CardCharacter { get; private set; }
        [field: SerializeField] internal CardSoundConfig SoundConfig { get; private set; }
        [field: SerializeField] internal CardEffectConfig Effect { get; private set; }
        [field: SerializeField] internal CardCapability CardCapability { get; private set; }
        [field: SerializeField] internal CardPersonType CardPersonType { get; private set; }
        [field: SerializeField] internal CardName CardName { get; private set; }
    }
}