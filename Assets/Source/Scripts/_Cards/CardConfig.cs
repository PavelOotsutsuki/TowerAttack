using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardConfig", menuName = "Cards/Card Config", order = 51)]
    internal class CardConfig : ScriptableObject
    {
        [field: SerializeField] public CardViewConfig CardViewConfig { get; private set; }
        [field: SerializeField] internal CardCharacter CardCharacter { get; private set; }
        [field: SerializeField] internal AudioClip AwakeSound { get; private set; }
        [field: SerializeField] internal CardEffectConfig Effect { get; private set; }
    }
}