using Cards.Effects;
using Tools;
using UnityEngine;

namespace Cards.Views
{
    [CreateAssetMenu(fileName = "VariantCardConfig", menuName = "Cards/VariantCardConfig", order = 51)]
    internal class VariantCardConfig : ScriptableObject, IData
    {
        [field: SerializeField] public CardViewConfig CardViewConfig { get; private set; }
        [field: SerializeField] internal CardEffectConfig Effect { get; private set; }
        [field: SerializeField] public CardCapability CardCapability { get; private set; }
    }
}