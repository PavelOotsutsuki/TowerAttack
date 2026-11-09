using UnityEngine;

namespace Cards.Views
{
    [CreateAssetMenu(fileName = "CardViewConfig", menuName = "Cards/CardViewConfig", order = 51)]
    internal class CardViewConfig : ScriptableObject
    {
        [field: SerializeField] public int Number { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public string Feature { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
    }
}