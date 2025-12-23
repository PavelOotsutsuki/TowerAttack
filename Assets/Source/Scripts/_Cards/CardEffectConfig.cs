using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/CardEffectConfig", fileName = "CardEffectConfig", order = 0)]
    public class CardEffectConfig : ScriptableObject
    {
        [field: SerializeField] public EffectType Type { get; private set; } = EffectType.Void;
        [field: SerializeField, Min(0)] public int Duration { get; private set; } = 0;
    }
}