using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardViewConfig", menuName = "Cards/Card View Config", order = 51)]
    public class CardViewConfig : ScriptableObject
    {
        [field: SerializeField] internal int Number { get; private set; }
        [field: SerializeField] internal Sprite Icon { get; private set; }
        [field: SerializeField] internal string Description { get; private set; }
        [field: SerializeField] internal string Feature { get; private set; }
        [field: SerializeField] internal string Name { get; private set; }
    }
}
