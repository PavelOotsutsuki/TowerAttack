using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "New card", menuName = "SO/Create card", order = 51)]
    internal class CardSO : ScriptableObject
    {
        [SerializeField] private CardCharacter _cardCharacter;
        [SerializeField] private int _number;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _description;
        [SerializeField] private AudioClip _awakeSound;
        [SerializeField] private string _feature;
        [SerializeField] private string _name;
        [SerializeField] private CardEffect _effect;

        internal CardCharacter CardCharacter => _cardCharacter;
        internal int Number => _number;
        internal Sprite Icon => _icon;
        internal string Description => _description;
        internal AudioClip AwakeSound => _awakeSound;
        internal string Feature => _feature;
        internal string Name => _name;
        internal CardEffect Effect => _effect;
    }
}