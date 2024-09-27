using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(menuName = "Cards/Card Effect Config", fileName = "CardEffectConfig", order = 0)]
    public class CardEffectConfig : ScriptableObject
    {
        [field: SerializeField] public EffectType Type { get; private set; }
        [field: SerializeField, Min(1)] public int Duration { get; private set; }
    }
}