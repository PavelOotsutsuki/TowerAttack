using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardConfig", menuName = "Cards/Card Config", order = 51)]
    internal class CardConfig : ScriptableObject
    {
        [field: SerializeField] internal CardCharacter CardCharacter { get; private set; }
        [field: SerializeField] internal int Number { get; private set; }
        [field: SerializeField] internal Sprite Icon { get; private set; }
        [field: SerializeField] internal string Description { get; private set; }
        [field: SerializeField] internal AudioClip AwakeSound { get; private set; }
        [field: SerializeField] internal string Feature { get; private set; }
        [field: SerializeField] internal string Name { get; private set; }
        [field: SerializeField] internal CardEffectConfig Effect { get; private set; }
    }
}