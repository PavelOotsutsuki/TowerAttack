using Tools;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardConfig", menuName = "Cards/Card Config", order = 51)]
    internal class CardConfig : ScriptableObject, IData
    {
        [field: SerializeField] public CardViewConfig CardViewConfig { get; private set; }
        [field: SerializeField] internal CardCharacter CardCharacter { get; private set; }
        [field: SerializeField] internal AudioClip AwakeSound { get; private set; }
        [field: SerializeField] internal CardEffectConfig Effect { get; private set; }
        [field: SerializeField] public CardCapability CardCapability { get; private set; }
        [field: SerializeField] internal CardPersonType CardPersonType { get; private set; }
    }
}