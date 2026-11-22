using UnityEngine;
using UnityEngine.Serialization;

namespace Cards.Effects
{
    [CreateAssetMenu(menuName = "Cards/CardEffectConfig", fileName = "CardEffectConfig", order = 0)]
    public class CardEffectConfig : ScriptableObject, ICardEffectConfig
    {
        [FormerlySerializedAs("<Type>k__BackingField")]
        [SerializeField] private EffectType _type;
        [FormerlySerializedAs("<Duration>k__BackingField")]
        [SerializeField, Min(0)] private int _duration;

        public EffectType Type => _type;
        public int Duration => _duration;
    }
}