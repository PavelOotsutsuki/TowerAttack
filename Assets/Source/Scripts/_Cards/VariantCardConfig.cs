using Tools;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "VariantCardConfig", menuName = "Cards/VariantCardConfig", order = 51)]
    internal class VariantCardConfig : ScriptableObject, IData
    {
        [field: SerializeField] public CardViewConfig CardViewConfig { get; private set; }
        [field: SerializeField] internal CardEffectConfig Effect { get; private set; }
    }
}