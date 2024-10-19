using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardViewConfig", menuName = "Cards/Card View Config", order = 51)]
    public class CardViewConfig : ScriptableObject
    {
        [field: SerializeField] public int Number { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public string Feature { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
    }
}